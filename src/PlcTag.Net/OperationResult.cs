using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PlcTag
{
    /// <summary>
    /// Result operation
    /// </summary>
    public class OperationResult
    {
        internal OperationResult(ITag tag, string operation)
        {
            Tag = tag;
            Operation = operation;

            Timestamp = DateTime.UtcNow;
            _watch = Stopwatch.StartNew();
        }

        internal void Finished(int statusCode)
        {
            Finished((OperationStatusCode)statusCode);
        }

        internal void Finished(OperationStatusCode statusCode)
        {
            _watch.Stop();
            StatusCode = statusCode;
        }

        internal void ThrowIfError()
        {
            if (IsError())
            {
                throw new TagOperationException($"{Operation} Operation Error", this);
            }
        }

        /// <summary>
        /// Determines if the result represents an error
        /// </summary>
        /// <returns></returns>
        public bool IsError()
        {
            return StatusCode < OperationStatusCode.STATUS_OK;
        }

        /// <summary>
        /// Tag
        /// </summary>
        /// <value></value>
        public ITag Tag { get; }

        /// <summary>
        /// Type of operation performed: Read, Write.
        /// Used only for Exception message
        /// </summary>
        public string Operation { get; }

        /// <summary>
        /// Timestamp last operation
        /// </summary>
        /// <value></value>
        public DateTime Timestamp { get; }

        private readonly Stopwatch _watch;

        /// <summary>
        /// Millisecond execution operatorion
        /// </summary>
        /// <value></value>
        public long ExecutionTime { get => _watch.ElapsedMilliseconds; }

        /// <summary>
        /// Returns the status code <see cref="StatusCodeOperation"/>
        /// STATUS_OK will be returned if the operation completed successfully.
        /// </summary>
        /// <value></value>
        public OperationStatusCode StatusCode { get; private set; }

        /// <summary>
        /// Returns the text description for the StatusCode
        /// </summary>
        public string StatusCodeText
        {
            get
            {
                return NativeLibrary.DecodeError((int)StatusCode);
            }
        }

        /// <summary>
        /// Information result.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $@"Tag Name:       {Tag.Name}
Timestamp:      {Timestamp}
ExecutionTime:  {ExecutionTime}
StatusCode:     {StatusCode}
StatusCodeText: {StatusCodeText}";
        }
    }
}
