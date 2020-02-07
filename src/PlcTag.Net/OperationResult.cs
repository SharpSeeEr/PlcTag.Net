using System;
using System.Collections.Generic;
using System.Linq;

namespace PlcTag
{
    /// <summary>
    /// Result operation
    /// </summary>
    public class OperationResult
    {
        internal OperationResult(ITag tag, DateTime timestamp, long executionTime, int statusCode)
            : this(tag, timestamp, executionTime, (OperationStatusCode)statusCode)
        {
        }

        internal OperationResult(ITag tag, DateTime timestamp, long executionTime, OperationStatusCode statusCode)
        {
            Tag = tag;
            Timestamp = timestamp;
            ExecutionTime = executionTime;
            StatusCode = statusCode;
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
        /// Timestamp last operation
        /// </summary>
        /// <value></value>
        public DateTime Timestamp { get; }

        /// <summary>
        /// Millisecond execution operatorion
        /// </summary>
        /// <value></value>
        public long ExecutionTime { get; }

        /// <summary>
        /// Returns the status code <see cref="StatusCodeOperation"/>
        /// STATUS_OK will be returned if the operation completed successfully.
        /// </summary>
        /// <value></value>
        public OperationStatusCode StatusCode { get; }

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
        /// Reduce multiple result to one.
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        public static OperationResult Reduce(IEnumerable<OperationResult> results)
        {
            return new OperationResult(null,
                                       results.Min(a => a.Timestamp),
                                       results.Sum(a => a.ExecutionTime),
                                       results.Min(a => a.StatusCode));
        }

        /// <summary>
        /// Information result.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $@"Tag Name:      {Tag.Name}
Timestamp:     {Timestamp}
ExecutionTime: {ExecutionTime}
StatusCode:    {StatusCode}";
        }
    }
}
