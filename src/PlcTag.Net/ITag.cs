using System;

namespace PlcTag
{
    /// <summary>
    /// Interface Tag
    /// </summary>
    public interface ITag<TType> : ITag
    {
        /// <summary>
        /// Performs read of Tag
        /// </summary>
        /// <returns></returns>
        TType Read();

        /// <summary>
        /// Perform write of Tag
        /// </summary>
        /// <returns></returns>
        void Write(TType value);
    }

    /// <summary>
    /// Interface Tag
    /// </summary>
    public interface ITag : IDisposable
    {
        /// <summary>
        /// Connects and creates the PLC tag
        /// </summary>
        void Connect();

        /// <summary>
        /// Destroys the PLC tag
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Handle, or Id, of the created Tag
        /// </summary>
        /// <value></value>
        int Handle { get; }

        /// <summary>
        /// Controller reference.
        /// </summary>
        /// <value></value>
        Controller Controller { get; }

        /// <summary>
        /// The textual name of the tag to access. The name is anything allowed by the protocol.
        /// E.g. myDataStruct.rotationTimer.ACC, myDINTArray[42] etc.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The size of an element in bytes. The tag is assumed to be composed of elements of the same size. For structure tags,
        /// use the total size of the structure.
        /// </summary>
        int Size { get; }

        /// <summary>
        /// elements length: 1- single, n-array.
        /// </summary>
        int Length { get; }

        /// <summary>
        /// Indicate if Tag is in read only.async Write raise exception.
        /// </summary>
        /// <value></value>
        bool IsReadOnly { get; set; }

        /// <summary>
        /// Determines if this tag references an array of values
        /// </summary>
        /// <returns></returns>
        bool IsArray();

        /// <summary>
        /// Abort any outstanding IO to the PLC. <see cref="StatusCodeOperation"/>
        /// </summary>
        /// <returns></returns>
        OperationStatusCode Abort();

        /// <summary>
        /// Get size tag.
        /// </summary>
        /// <returns></returns>
        int GetSize();

        /// <summary>
        /// Get status operation. <see cref="StatusCodeOperation"/>
        /// </summary>
        /// <returns></returns>
        OperationStatusCode GetStatus();

        /// <summary>
        /// Lock for multitrading. <see cref="StatusCodeOperation"/>
        /// </summary>
        /// <returns></returns>
        OperationStatusCode Lock();

        /// <summary>
        /// Unlock for multitrading <see cref="StatusCodeOperation"/>
        /// </summary>
        /// <returns></returns>
        OperationStatusCode Unlock();
    }
}
