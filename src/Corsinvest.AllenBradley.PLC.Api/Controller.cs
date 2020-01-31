using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

namespace Corsinvest.AllenBradley.PLC.Api
{
    /// <summary>
    /// Controller
    /// </summary>
    public class Controller : IDisposable
    {
        private const string _defaultGroupName = "default";

        private readonly Dictionary<string, TagGroup> _tagGroups = new Dictionary<string, TagGroup>();
        private readonly Dictionary<string, ITag> _tags = new Dictionary<string, ITag>();
        private readonly ILogger<Controller> _logger;

        private bool _disposed;

        /// <summary>
        /// Controller definition
        /// </summary>
        /// <param name="ipAddress">IP address of the gateway for this protocol.
        /// Could be the IP address of the PLC you want to access.</param>
        /// <param name="path">Required for LGX, Optional for PLC/SLC/MLGX IOI path to access the PLC from the gateway.
        /// <para></para>Communication Port Type: 1- Backplane, 2- Control Net/Ethernet, DH+ Channel A, DH+ Channel B, 3- Serial
        /// <para></para>Slot number where cpu is installed: 0,1.. </param>
        /// <param name="cpuType">AB CPU models</param>
        /// <param name="logger">ILogger</param>
        public Controller(string ipAddress, string path, CPUType cpuType, ILogger<Controller> logger)
        {
            if (cpuType == CPUType.LGX && string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("PortType and Slot must be specified for ControlLogix / CompactLogix processors");
            }

            _logger = logger;

            IPAddress = ipAddress;
            Path = path;
            CPUType = cpuType;

            _tagGroups.Add(_defaultGroupName, new TagGroup(this, _defaultGroupName));
        }

        /// <summary>
        /// Raise Exception on failed operation
        /// </summary>
        /// <value></value>
        public bool FailOperationRaiseException { get; set; } = false;

        /// <summary>
        /// Automatic Write when using value.
        /// </summary>
        /// <value></value>
        public bool AutoReadValue { get; set; } = false;

        /// <summary>
        /// Automatic Write when using value.
        /// </summary>
        /// <value></value>
        public bool AutoWriteValue { get; set; } = false;

        /// <summary>
        /// Communication timeout millisec.
        /// </summary>
        /// <value></value>
        public int Timeout { get; set; } = 5000;

        /// <summary>
        /// Optional allows the selection of varying levels of debugging output.
        /// 1 shows only the more urgent problems.
        /// 5 shows almost every action within the library and will generate a very large amount of output.
        /// Generally 3 or 4 is most useful when debugging.
        /// </summary>
        /// <value></value>
        public int DebugLevel { get; set; } = 0;

        /// <summary>
        /// Groups
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TagGroup> Groups { get { return _tagGroups.Values.AsEnumerable(); } }

        /// <summary>
        /// All Tags
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ITag> Tags { get { return _tags.Values.AsEnumerable(); } }

        /// <summary>
        /// Verify if exists tag with name and return.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool TagExists(string name) => _tags.ContainsKey(name);

        /// <summary>
        /// IP address of the gateway for this protocol. Could be the IP address of the PLC you want to access.
        /// </summary>
        public string IPAddress { get; }

        /// <summary>
        /// AB CPU models
        /// </summary>
        public CPUType CPUType { get; }

        /// <summary>
        /// Required for LGX, Optional for PLC/SLC/MLGX IOI path to access the PLC from the gateway.
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Ping controller.
        /// </summary>
        /// <param name="echo">True echo result to standard output</param>
        /// <returns></returns>
        public bool Ping(bool echo = false)
        {
            using (var ping = new Ping())
            {
                var reply = ping.Send(IPAddress);
                if (echo)
                {
                    if (reply.Status == IPStatus.Success)
                    {
                        _logger.LogInformation($"Ping Reply from {reply.Address}: time {reply.RoundtripTime} TTL={reply.Options?.Ttl} size={reply.Buffer?.Length}");
                    }
                    else
                    {
                        _logger.LogInformation($"Pinging {reply.Address}: {reply.Status.ToString()}");
                    }
                }

                return reply.Status == IPStatus.Success;
            }
        }

        /// <summary>
        /// Creates new TagGroup
        /// </summary>
        /// <returns></returns>
        public TagGroup CreateGroup(string name)
        {
            if (name == _defaultGroupName)
            {
                throw new ArgumentException("Invalid group name");
            }

            var group = new TagGroup(this, name);
            _tagGroups.Add(name, group);
            return group;
        }

        #region Create Tags

        /// <summary>
        /// Create Tag custom Type Class
        /// </summary>
        /// <param name="name">The textual name of the tag to access. The name is anything allowed by the protocol.
        /// E.g. myDataStruct.rotationTimer.ACC, myDINTArray[42] etc.</param>
        /// <typeparam name="T">Class to create</typeparam>
        /// <returns></returns>
        public Tag<T> CreateTag<T>(string name)
        {
            return CreateTag<T>(name, 1);
        }

        /// <summary>
        /// Create Tag using free definition
        /// </summary>
        /// <param name="name">The textual name of the tag to access. The name is anything allowed by the protocol.
        /// E.g. myDataStruct.rotationTimer.ACC, myDINTArray[42] etc.</param>
        /// <param name="size">The size of an element in bytes. The tag is assumed to be composed of elements of the same size.
        /// For structure tags, use the total size of the structure.</param>
        /// <param name="length">elements count: 1- single, n-array.</param>
        /// <returns></returns>
        public Tag<T> CreateTag<T>(string name, int length)
        {
            if (length < 1)
            {
                throw new ArgumentException("Tag length must be at least 1");
            }

            var tag = new Tag<T>(this, name, length);
            _tags.Add(name, tag);
            _tagGroups[_defaultGroupName].Add(tag);
            return tag;
        }
        #endregion



        #region IDisposable Support

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    foreach (var tag in _tags.Values)
                    {
                        tag.Dispose();
                    }
                    _tags.Clear();
                    _tagGroups.Clear();
                }

                _disposed = true;
            }
        }

        /// <summary>
        /// Destructor
        /// </summary>
        /// <returns></returns>
        ~Controller() { Dispose(false); }

        /// <summary>
        /// Dispose object
        /// </summary>
        public void Dispose() { Dispose(true); }

        #endregion IDisposable Support
    }
}
