using System;
using System.Runtime.Serialization;

namespace PlcTag
{
    /// <summary>
    /// Tag operation exception
    /// </summary>
    [Serializable]
    public class TagOperationException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public TagOperationException(string message, OperationResult result)
            : base(message)
        {
            Result = result;
        }

        /// <summary>
        /// Result operation
        /// </summary>
        /// <value></value>
        public OperationResult Result { get; }

        /// <summary>
        /// Standard serialization constructor
        /// </summary>
        /// <param name="serializationInfo"></param>
        /// <param name="streamingContext"></param>
        protected TagOperationException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
            Result = (OperationResult)serializationInfo.GetValue(nameof(Result), typeof(OperationResult));
        }

        /// <summary>
        /// Standard serialization implementation
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Result), Result);
            base.GetObjectData(info, context);
        }
    }
}
