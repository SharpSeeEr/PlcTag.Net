using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Corsinvest.AllenBradley.PLC.Api
{
    /// <summary>
    /// Tag base definition
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class Tag<T> : ITag<T>, IDisposable
    {
        private bool _hasRead;
        private bool _hasWritten;
        private T _lastValueWritten;
        private bool _isConnected;

        private readonly TagValueManager<T> _valueManager;

        /// <summary>
        /// Event Handler called when the value read from the PLC changes.
        /// </summary>
        /// <param name="sender">The tag</param>
        /// <param name="value">The new value</param>
        public delegate void ValueChangeEventHandler(Tag<T> sender, T value);

        /// <summary>
        /// Event changed value
        /// </summary>
        public event ValueChangeEventHandler Changed;

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
        internal Tag(Controller controller, string name, int length)
        {
            Controller = controller;
            Name = name;
            Size = TagSize.GetSize<T>();
            Length = length;
            _valueManager = new TagValueManager<T>(this);
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
        public Controller Controller { get; }

        /// <summary>
        /// The textual name of the tag to access. The name is anything allowed by the protocol.
        /// E.g. myDataStruct.rotationTimer.ACC, myDINTArray[42] etc.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The size of an element in bytes. The tag is assumed to be composed of elements of the same size.For structure tags,
        /// use the total size of the structure.
        /// </summary>
        public int Size { get; }

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
            Handle = NativeLibrary.plc_tag_create(GetTagString(), Controller.Timeout);
            _isConnected = true;
        }

        /// <summary>
        /// Disconnects and destroys the PLC tag.
        /// </summary>
        public void Disconnect()
        {
            NativeLibrary.plc_tag_destroy(Handle);
            _isConnected = false;
            Handle = 0;
        }

        private string GetTagString()
        {
            var tagString = $"protocol=ab_eip&gateway={Controller.IPAddress}";

            if (!string.IsNullOrEmpty(Controller.Path))
            {
                tagString += $"&path={Controller.Path}";
            }
            tagString += $"&cpu={Controller.CPUType}&elem_size={Size}&elem_count={Length}&name={Name}";

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
            var timestamp = DateTime.Now;
            var watch = Stopwatch.StartNew();
            var statusCode = NativeLibrary.plc_tag_read(Handle, Controller.Timeout);

            watch.Stop();

            var result = new OperationResult(this, timestamp, watch.ElapsedMilliseconds, statusCode);

            //check raise exception
            if (result.IsError())
            {
                throw new TagOperationException("Read Operation Error", result);
            }

            var value = _valueManager.Get();
            if (!_hasRead || !value.Equals(LastValueRead))
            {
                Changed?.Invoke(this, value);
            }
            LastValueRead = value;

            return value;
        }

        private static int GetHashCode(byte[] data)
        {
            if (data == null) { return 0; }

            var i = data.Length;
            var hc = i + 1;

            while (--i >= 0)
            {
                hc *= 257;
                hc ^= data[i];
            }

            return hc;
        }

        private static T DeepClone<T>(T obj)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// Performs write of Tag
        /// </summary>
        /// <returns></returns>
        public void Write(T value)
        {
            if (IsReadOnly) { throw new InvalidOperationException("Tag is set read only!"); }

            var timestamp = DateTime.Now;
            var watch = Stopwatch.StartNew();
            var statusCode = NativeLibrary.plc_tag_write(Handle, Controller.Timeout);
            watch.Stop();

            var result = new OperationResult(this, timestamp, watch.ElapsedMilliseconds, statusCode);

            if (result.IsError())
            {
                throw new TagOperationException("Write Operation Error", result);
            }
        }

        /// <summary>
        /// Abort any outstanding IO to the PLC. <see cref="StatusCodeOperation"/>
        /// </summary>
        /// <returns></returns>
        public int Abort() { return NativeLibrary.plc_tag_abort(Handle); }

        /// <summary>
        /// Get size tag read from PLC.
        /// </summary>
        /// <returns></returns>
        public int GetSize() { return NativeLibrary.plc_tag_get_size(Handle); }

        /// <summary>
        /// Get status operation. <see cref="StatusCodeOperation"/>
        /// </summary>
        /// <returns></returns>
        public int GetStatus() { return NativeLibrary.plc_tag_status(Handle); }

        /// <summary>
        /// Lock for multitrading. <see cref="StatusCodeOperation"/>
        /// </summary>
        /// <returns></returns>
        public int Lock() { return NativeLibrary.plc_tag_lock(Handle); }

        /// <summary>
        /// Unlock for multitrading <see cref="StatusCodeOperation"/>
        /// </summary>
        /// <returns></returns>
        public int Unlock() { return NativeLibrary.plc_tag_unlock(Handle); }

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
                        NativeLibrary.plc_tag_destroy(Handle);
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
