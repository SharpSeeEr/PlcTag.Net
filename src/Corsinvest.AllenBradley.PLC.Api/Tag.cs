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
        private bool _disposed = false;

        /// <summary>
        /// Event changed value
        /// </summary>
        public event EventHandlerOperation Changed;

        private Tag() { }

        /// <summary>
        /// Creates a tag. If the CPU type is LGX, the port type and slot has to be specified.
        /// </summary>
        /// <param name="controller">Controller reference</param>
        /// <param name="name">The textual name of the tag to access. The name is anything allowed by the protocol.
        /// E.g. myDataStruct.rotationTimer.ACC, myDINTArray[42] etc.</param>
        /// <param name="size">The size of an element in bytes. The tag is assumed to be composed of elements of the same size.
        /// For structure tags, use the total size of the structure.</param>
        /// <param name="length">elements count: 1- single, n-array.</param>
        internal Tag(Controller controller, string name, int length)
        {
            Controller = controller;
            Name = name;
            Size = TagSize.GetSize<T>();
            Length = length;
            ValueManager = new TagValueManager<T>(this);
            ValueType = typeof(T);

            var url = $"protocol=ab_eip&gateway={controller.IPAddress}";
            if (!string.IsNullOrEmpty(controller.Path)) { url += $"&path={controller.Path}"; }
            url += $"&cpu={controller.CPUType}&elem_size={Size}&elem_count={Length}&name={Name}";
            if (controller.DebugLevel > 0) { url += $"&debug={controller.DebugLevel}"; }

            //create reference
            Handle = NativeLibrary.plc_tag_create(url, controller.Timeout);

            Value = TagHelper.CreateObject<T>(Length);
        }

        /// <summary>
        /// Handle, or Id, of the created Tag
        /// </summary>
        /// <value></value>
        public Int32 Handle { get; }

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
        public bool ReadOnly { get; set; } = false;

        /// <summary>
        /// Value manager
        /// </summary>
        /// <value></value>
        public TagValueManager<T> ValueManager { get; }

        /// <summary>
        /// Old value tag.
        /// </summary>
        /// <value></value>
        private T _previousValueRead;

        /// <summary>
        /// Old value tag.
        /// </summary>
        /// <value></value>
        private T _previousValueWritten;

        private T _value;
        /// <summary>
        /// Value tag.
        /// </summary>
        /// <value></value>
        public T Value
        {
            get
            {
                if (Controller.AutoReadValue) { Read(); }
                return  ValueManager.Get();
            }

            set
            {
                _value = value;
                ValueManager.Set(value, 0);
                if (Controller.AutoWriteValue) { Write(); }
            }
        }

        object ITag.Value
        {
            get => Value;
        }

        /// <summary>
        /// Indicates whether or not a value has been read from the PLC.
        /// </summary>
        /// <value></value>
        public bool HasBeenRead { get; private set; } = false;

        /// <summary>
        /// Indicates whether or not a value has been written to the PLC.
        /// </summary>
        public bool HasBeenWritten { get; private set; } = false;

        /// <summary>
        /// Indicate if Value changed OldValue 
        /// </summary>
        /// <value></value>
        public bool HasChangedValue
        {
            get
            {
                return _value.Equals(_previousValueRead);
            }
        }

        /// <summary>
        /// Determines if this tag references an array of values
        /// </summary>
        /// <returns></returns>
        public bool IsArray()
        {
            return Length > 1;
        }

        /// <summary>
        /// Performs read of Tag
        /// </summary>
        /// <returns></returns>
        public OperationResult Read()
        {
            //save old value
            _previousValueRead = _value;

            var timestamp = DateTime.Now;
            var watch = Stopwatch.StartNew();
            var statusCode = NativeLibrary.plc_tag_read(Handle, Controller.Timeout);

            watch.Stop();
            HasBeenRead = true;

            var result = new OperationResult(this, timestamp, watch.ElapsedMilliseconds, statusCode);

            //check raise exception
            if (Controller.FailOperationRaiseException && StatusCodeOperation.IsError(statusCode))
            {
                throw new TagOperationException(result);
            }

            //event change value
            if (HasChangedValue) { Changed?.Invoke(result); }

            return result;
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
        public OperationResult Write()
        {
            if (ReadOnly) { throw new InvalidOperationException("Tag is set read only!"); }

            var timestamp = DateTime.Now;
            var watch = Stopwatch.StartNew();
            var statusCode = NativeLibrary.plc_tag_write(Handle, Controller.Timeout);
            watch.Stop();
            HasBeenWritten = true;

            var result = new OperationResult(this, timestamp, watch.ElapsedMilliseconds, statusCode);

            //check raise exception
            if (Controller.FailOperationRaiseException && result.IsError())
            {
                throw new TagOperationException(result);
            }

            return result;
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
        void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing) { NativeLibrary.plc_tag_destroy(Handle); }
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
        #endregion
    }
}