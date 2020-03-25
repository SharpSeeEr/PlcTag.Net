using System;
using System.Collections.Generic;

namespace PlcTag
{
    /// <summary>
    /// PLC Controller
    /// Manages connections and tag IO operations to an AB PLC
    /// </summary>
    public interface IPlcController : IDisposable
    {
        /// <summary>
        /// Type of CPU
        /// </summary>
        CPUType CPUType { get; }

        /// <summary>
        /// Verbosity of console output from the underlying libplctag library
        /// </summary>
        int DebugLevel { get; set; }

        /// <summary>
        /// IP Address of the PLC
        /// </summary>
        string IPAddress { get; }

        /// <summary>
        /// Required for LGX, Optional for PLC/SLC/MLGX IOI path to access the PLC from the gateway.
        /// </summary>
        string Path { get; }

        /// <summary>
        /// Communication timeout in milliseconds
        /// </summary>
        int Timeout { get; set; }

        /// <summary>
        /// List of all Tags created by this controller
        /// </summary>
        IEnumerable<ITag> Tags { get; }

        /// <summary>
        /// Tag Groups, including the automatically created default group
        /// </summary>
        IEnumerable<TagGroup> Groups { get; }

        /// <summary>
        /// Verify if exists tag with name and return.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool TagExists(string name);

        /// <summary>
        /// Connects all PLC tags
        /// </summary>
        void Connect();

        /// <summary>
        /// Destroys all PLC tags
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Creates new TagGroup
        /// </summary>
        /// <returns></returns>
        TagGroup CreateGroup(string name);

        /// <summary>
        /// Create Tag custom Type Class
        /// </summary>
        /// <param name="name">The textual name of the tag to access. The name is anything allowed by the protocol.
        /// E.g. myDataStruct.rotationTimer.ACC, myDINTArray[42] etc.</param>
        /// <typeparam name="T">Class to create</typeparam>
        /// <returns></returns>
        Tag<T> CreateTag<T>(string name);

        /// <summary>
        /// Create Tag using free definition
        /// </summary>
        /// <param name="name">The textual name of the tag to access. The name is anything allowed by the protocol.
        /// E.g. myDataStruct.rotationTimer.ACC, myDINTArray[42] etc.</param>
        /// <param name="length">elements count: 1- single, n-array.</param>
        /// <returns></returns>
        Tag<T> CreateTag<T>(string name, int length);
    }
}