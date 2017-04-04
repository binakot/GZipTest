using System.Collections.Generic;
using GZipTest.Application.Interfaces;

namespace GZipTest.Application.Files
{
    /// <summary>
    /// Thread-safe file chunks queue.
    /// </summary>
    public sealed class FileChunkQueue : IQueue<byte[]>
    {
        private readonly Queue<byte[]> _queue = new Queue<byte[]>(0);
        private readonly object _lock = new object();

        private volatile bool _isLast;
        public bool IsLast
        {
            get { return _isLast; }
            set { _isLast = value; }
        }

        public void Enqueue(byte[] item)
        {
            lock (_lock)
            {
                _queue.Enqueue(item);
            }
        }

        public byte[] Dequeue()
        {
            lock (_lock)
            {
                return _queue.Dequeue();
            }
        }

        public byte[] Peek()
        {
            lock (_lock)
            {
                return _queue.Peek();
            }
        }

        public int Count()
        {
            lock (_lock)
            {
                return _queue.Count;
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                _queue.Clear();
            }
        }
    }
}