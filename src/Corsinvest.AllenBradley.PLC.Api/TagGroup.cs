using System;
using System.Collections.Generic;
using System.Linq;

namespace Corsinvest.AllenBradley.PLC.Api
{
    /// <summary>
    /// Group of Tags
    /// </summary>
    public class TagGroup
    {
        //private readonly Timer _timer;
        //private object _lockScan = new object();
        private readonly Dictionary<string, ITag> _tags = new Dictionary<string, ITag>();

        /// <summary>
        /// Event changed value
        /// </summary>
        //public event EventHandlerOperations Changed;

        /// <summary>
        /// Event on timed scan.
        /// </summary>
        //public event EventHandler OnTimedScan;

        private TagGroup()
        {
        }

        internal TagGroup(Controller controller, string name)
        {
            Controller = controller;
            Name = name;
            //_timer = new Timer();
            //_timer.Elapsed += OnTimedEvent;
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
        /// Performs read of Group of Tags
        /// </summary>
        //public IEnumerable<OperationResult> Read(bool onlyChanged = false)
        //{
        //    var results = Tags.Select(a => a.Read()).ToArray();
        //    var resultsOnlyChanged = results.Where(a => a.Tag.HasChangedValue);
        //    if (resultsOnlyChanged.Count() > 0) { Changed?.Invoke(resultsOnlyChanged); }

        //    return onlyChanged ? resultsOnlyChanged : results;
        //}

        /// <summary>
        /// Performs write of Group of Tags
        /// </summary>
        //public IEnumerable<OperationResult> Write() { return Tags.Select(a => a.Write()).ToArray(); }

        /// <summary>
        /// Scan operation behavior of Tags
        /// </summary>
        /// <value></value>
        //public ScanMode ScanMode { get; set; } = ScanMode.Read;

        /// <summary>
        /// Scanning update (refresh) interval in milliseconds
        /// </summary>
        /// <value></value>
        //public double ScanInterval
        //{
        //    get => _timer.Interval;
        //    set => _timer.Interval = value;
        //}

        /// <summary>
        /// Begins background scanning of Tags
        /// </summary>
        //public void ScanStart()
        //{
        //    if (Enabled) { throw new Exception("TagGroup cannot scan when disabled"); }
        //    _timer.Start();
        //}

        /// <summary>
        /// Stops scanning from previously called ScanStart.
        /// Terminates scan thread and frees any allocated resources.
        /// </summary>
        //public void ScanStop() { _timer.Stop(); }

        //private void OnTimedEvent(Object source, ElapsedEventArgs e)
        //{
        //    lock (_lockScan)
        //    {
        //        switch (ScanMode)
        //        {
        //            case ScanMode.Read: Read(); break;
        //            case ScanMode.Write: Write(); break;
        //            case ScanMode.ReadAndWrite: break;
        //            default: break;
        //        }

        //        OnTimedScan?.Invoke(this, EventArgs.Empty);
        //    }
        //}
    }
}
