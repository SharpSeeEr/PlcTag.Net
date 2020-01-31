using System;

namespace Corsinvest.AllenBradley.PLC.Api
{
    /// <summary>
    /// Interface Tag
    /// </summary>
    public interface ITag<TType> : ITag
    {
        /// <summary>
        /// Value tag.
        /// </summary>
        /// <value></value>
        new TType Value { get; set; }

        /// <summary>
        /// Value manager
        /// </summary>
        /// <value></value>
        TagValueManager<TType> ValueManager { get; }
    }

    /// <summary>
    /// Interface Tag
    /// </summary>
    public interface ITag : IDisposable
    {
        /// <summary>
        /// Handle, or Id, of the created Tag
        /// </summary>
        /// <value></value>
        Int32 Handle { get; }

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
        /// Type value.
        /// </summary>
        /// <value></value>
        Type ValueType { get; }

        /// <summary>
        /// Value tag.
        /// </summary>
        /// <value></value>
        object Value { get; }

        /// <summary>
        /// Indicate if Value changed OldValue 
        /// </summary>
        /// <value></value>
        bool HasChangedValue { get; }

        /// <summary>
        /// Indicate if Tag is in read only.async Write raise exception.
        /// </summary>
        /// <value></value>
        bool ReadOnly { get; set; }

        /// <summary>
        /// Determines if this tag references an array of values
        /// </summary>
        /// <returns></returns>
        bool IsArray();

        /// <summary>
        /// Performs read of Tag 
        /// </summary>
        /// <returns></returns>
        OperationResult Read();

        /// <summary>
        /// Perform write of Tag
        /// </summary>
        /// <returns></returns>
        OperationResult Write();

        /// <summary>
        /// Indicates whether or not a value has been read from the PLC.
        /// </summary>
        /// <value></value>
        bool HasBeenRead { get; }

        /// <summary>
        /// Indicates whether or not a value has been written to the PLC.
        /// </summary>
        bool HasBeenWritten { get; }

        /// <summary>
        /// Abort any outstanding IO to the PLC. <see cref="StatusCodeOperation"/>
        /// </summary>
        /// <returns></returns>
        int Abort();

        /// <summary>
        /// Get size tag.
        /// </summary>
        /// <returns></returns>
        int GetSize();

        /// <summary>
        /// Get status operation. <see cref="StatusCodeOperation"/>
        /// </summary>
        /// <returns></returns>
        int GetStatus();

        /// <summary>
        /// Lock for multitrading. <see cref="StatusCodeOperation"/>
        /// </summary>
        /// <returns></returns>
        int Lock();

        /// <summary>
        /// Unlock for multitrading <see cref="StatusCodeOperation"/>
        /// </summary>
        /// <returns></returns>
        int Unlock();
    }
}