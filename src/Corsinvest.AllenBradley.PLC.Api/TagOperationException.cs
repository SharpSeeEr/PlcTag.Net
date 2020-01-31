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
        /// <param name="result"></param>
        /// <returns></returns>
        public TagOperationException(OperationResult result) : base("Error execute operation!")
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