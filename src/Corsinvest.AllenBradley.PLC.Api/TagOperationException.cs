using System;

namespace Corsinvest.AllenBradley.PLC.Api
{
    /// <summary>
    /// Tag operation exception
    /// </summary>
    [System.Serializable]
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
    }
}
