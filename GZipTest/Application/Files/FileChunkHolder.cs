using System;
using System.Collections.Generic;
using GZipTest.Application.Interfaces;

namespace GZipTest.Application.Files
{
    /// <summary>
    /// Thread-safe holder for file chunks.
    /// </summary>
    public sealed class FileChunkHolder : IHolder
    {
        private readonly Dictionary<int, byte[]> _chunks = new Dictionary<int, byte[]>(0);
        private readonly object _lock = new object(); 

        public void Add(int index, byte[] bytes)
        {
            lock (_lock)
            {
                if (_chunks.ContainsKey(index))
                    throw new ArgumentException($"Chunk with index {index} is already exists");

                _chunks.Add(index, bytes);
            }
        }

        public byte[] Get(int index, bool remove = true)
        {
            lock (_lock)
            {
                if (!_chunks.ContainsKey(index))
                    throw new ArgumentException($"Chunk with index {index} is not exist.");

                var chunk = _chunks[index];
                if (remove)
                    _chunks.Remove(index);

                return chunk;
            }
        }

        public int Count()
        {
            lock (_lock)
            {
                return _chunks.Count;
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                _chunks.Clear();
            }
        }
    }
}