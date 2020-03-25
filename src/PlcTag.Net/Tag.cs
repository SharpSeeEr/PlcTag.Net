using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace PlcTag
{
    /// <summary>
    /// Tag base definition
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class Tag<T> : ITag<T>, IDisposable
    {
        private readonly TagValueManager _valueManager;

        private Tag()
        {
        }

        /// <summary>
        /// Creates a tag. If the CPU type is LGX, the port type and slot has to be specified.
        /// </summary>
        /// <param name="controller">Controller reference</param>
        /// <param name="name">The textual name of the tag to access. The name is anything allowed by the protocol.
        /// E.g. myDataStruct.rotationTimer.ACC, myDINTArray[42] etc.</param>
        /// <param name="length">elements count: 1- single, n-array.</param>
        internal Tag(PlcController controller, string name, int length)
        {
            Controller = controller;
            Name = name;
            Length = length;
            _valueManager = TagValueManager.GetTagValueManager<T>();
        }

        /// <summary>
        /// Handle, or Id, of the created Tag
        /// </summary>
        /// <value></value>
        public int Handle { get; private set; }

        /// <summary>
        /// Controller reference.
        /// </summary>
        /// <value></value>
        public PlcController Controller { get; }

        /// <summary>
        /// The textual name of the tag to access. The name is anything allowed by the protocol.
        /// E.g. myDataStruct.rotationTimer.ACC, myDINTArray[42] etc.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// elements length: 1- single, n-array.
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// Type value.
        /// </summary>
        public Type ValueType { get; }

        /// <summary>
        /// Indicate if Tag is in read only. async Write raise exception.
        /// </summary>
        /// <value></value>
        public bool IsReadOnly { get; set; } = false;

        /// <summary>
        /// The last value read from the Tag
        /// </summary>
        public T LastValueRead { get; private set; }

        /// <summary>
        /// The size of an element in bytes. The tag is assumed to be composed of elements of the same size.For structure tags,
        /// use the total size of the structure.
        /// </summary>
        public int Size { get => _valueManager.Size; }

        /// <summary>
        /// Determines if this tag references an array of values
        /// </summary>
        /// <returns></returns>
        public bool IsArray()
        {
            return Length > 1;
        }

        /// <summary>
        /// Connects and creates the PLC tag
        /// </summary>
        public void Connect()
        {
            var result = new OperationResult(this, "Create");
            var tagString = GetTagString();
            var statusCode = NativeLibrary.plc_tag_create(tagString, Controller.Timeout);
            result.Finished(statusCode);
            result.ThrowIfError();

            Handle = statusCode;
        }

        /// <summary>
        /// Disconnects and destroys the PLC tag.
        /// </summary>
        public void Disconnect()
        {
            NativeLibrary.plc_tag_destroy(Handle);
            Handle = 0;
        }

        private string GetTagString()
        {
            var tagString = $"protocol=ab_eip&gateway={Controller.IPAddress}";

            if (!string.IsNullOrEmpty(Controller.Path))
            {
                tagString += $"&path={Controller.Path}";
            }
            tagString += $"&cpu={Controller.CPUType.ToString().ToLower()}&elem_size={Size}&elem_count={Length}&name={Name}";

            if (Controller.DebugLevel > 0)
            {
                tagString += $"&debug={Controller.DebugLevel}";
            }
            tagString += "&share_session=1";

            return tagString;
        }

        /// <summary>
        /// Performs read of Tag
        /// </summary>
        /// <returns></returns>
        public T Read()
        {
            var result = new OperationResult(this, "Read");
            var statusCode = NativeLibrary.plc_tag_read(Handle, Controller.Timeout);
            result.Finished(statusCode);
            result.ThrowIfError();

            result = new OperationResult(this, "ReadValue");
            try
            {
                LastValueRead = (T)_valueManager.GetValue(Handle, 0);
                result.Finished(OperationStatusCode.STATUS_OK);
                return LastValueRead;
            }
            catch (Exception)
            {
                result.Finished(NativeLibrary.plc_tag_status(Handle));
                throw new TagOperationException("ReadValue Operation Error", result);
            }
        }

        /// <summary>
        /// Performs write of Tag
        /// </summary>
        /// <returns></returns>
        public void Write(T value)
        {
            if (IsReadOnly) { throw new InvalidOperationException("Tag is set read only!"); }

            var result = new OperationResult(this, "WriteValue");
            var statusCode = _valueManager.SetValue(Handle, value, 0);
            result.Finished(statusCode);
            result.ThrowIfError();
            
            result = new OperationResult(this, "Write");
            statusCode = NativeLibrary.plc_tag_write(Handle, Controller.Timeout);
            result.Finished(statusCode);
            result.ThrowIfError();
        }

        /// <summary>
        /// Abort any outstanding IO to the PLC. <see cref="OperationStatusCode"/>
        /// </summary>
        /// <returns></returns>
        public OperationStatusCode Abort() { return (OperationStatusCode)NativeLibrary.plc_tag_abort(Handle); }

        /// <summary>
        /// Get size tag read from PLC.
        /// </summary>
        /// <returns></returns>
        public int GetSize() { return NativeLibrary.plc_tag_get_size(Handle); }

        /// <summary>
        /// Get status operation. <see cref="OperationStatusCode"/>
        /// </summary>
        /// <returns></returns>
        public OperationStatusCode GetStatus() { return (OperationStatusCode)NativeLibrary.plc_tag_status(Handle); }

        /// <summary>
        /// Lock for multitrading. <see cref="OperationStatusCode"/>
        /// </summary>
        /// <returns></returns>
        public OperationStatusCode Lock() { return (OperationStatusCode)NativeLibrary.plc_tag_lock(Handle); }

        /// <summary>
        /// Unlock for multitrading <see cref="OperationStatusCode"/>
        /// </summary>
        /// <returns></returns>
        public OperationStatusCode Unlock() { return (OperationStatusCode)NativeLibrary.plc_tag_unlock(Handle); }

        #region IDisposable Support

        private bool _disposed = false;

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (Handle > 0)
                    {
                        Disconnect();
                    }
                }
                _disposed = true;
            }
        }

        /// <summary>
        /// Destructor
        /// </summary>
        /// <returns></returns>
        ~Tag() { Dispose(false); }

        /// <summary>
        /// Dispose object
        /// </summary>
        public void Dispose() { Dispose(true); }

        #endregion IDisposable Support
    }
}
