using System;
using System.Collections.Generic;
using System.Linq;

namespace PlcTag
{
    /// <summary>
    /// Group of Tags
    /// </summary>
    public class TagGroup
    {
        private readonly Dictionary<string, ITag> _tags = new Dictionary<string, ITag>();

        private TagGroup()
        {
        }

        internal TagGroup(Controller controller, string name)
        {
            Controller = controller;
            Name = name;
        }

        /// <summary>
        /// Friendly name for this group
        /// </summary>
        public string Name { get; }

        #region Collection

        /// <summary>
        /// Tags
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ITag> Tags { get { return _tags.Values.AsEnumerable(); } }

        /// <summary>
        /// Add tag
        /// </summary>
        /// <param name="tag"></param>
        public void Add(ITag tag)
        {
            if (_tags.ContainsKey(tag.Name))
            {
                throw new ArgumentException("Tag already exists in this group!");
            }
            _tags.Add(tag.Name, tag);
        }

        /// <summary>
        /// Remove tag
        /// </summary>
        /// <param name="tag"></param>
        public void Remove(ITag tag)
        {
            if (!_tags.ContainsKey(tag.Name))
            {
                throw new ArgumentException("Tag not found in this collection!");
            }
            _tags.Remove(tag.Name);
        }

        /// <summary>
        /// Clears all Tags from the group
        /// </summary>
        public void Clear() { _tags.Clear(); }

        #endregion Collection

        /// <summary>
        /// Enabled status.
        /// </summary>
        /// <value></value>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Controller
        /// </summary>
        /// <value></value>
        public Controller Controller { get; }

        /// <summary>
        /// Connects all PLC tags
        /// </summary>
        public void Connect()
        {
            foreach (var tag in _tags.Values)
            {
                tag.Connect();
            }
        }

        /// <summary>
        /// Destroys all PLC tags
        /// </summary>
        public void Disconnect()
        {
            foreach (var tag in _tags.Values)
            {
                tag.Disconnect();
            }
        }
    }
}
