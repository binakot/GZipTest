using System;
using System.Collections.Generic;
using System.IO;
using GZipTest.Application.Context;
using GZipTest.Application.Files;
using GZipTest.Application.Interfaces;
using GZipTest.Application.Tasks;

namespace GZipTest.Application.Compression
{
    public sealed class GZipArchiver : IArchiver
    {
        private readonly IHolder _inputChunkHolder = new FileChunkHolder();
        private readonly IHolder _outputChunkHolder = new FileChunkHolder();
        private readonly ICompressor _compressor = new GZipCompressor();
        private readonly IExecutor<BaseTask> _executor;
        private readonly int _bufferSize;

        public GZipArchiver(int threadsCount, int bufferSize)
        {
            _executor = new TaskExecutor(threadsCount);
            _bufferSize = bufferSize;
        }

        public void Process(string inputFilePath, string outputFilePath, OperationType operation)
        {
            switch (operation)
            {
                case OperationType.Compress:
                    _executor.AddTasks(
                        PrepareTasksForCompression(inputFilePath, outputFilePath, _bufferSize));
                    break;
                case OperationType.Decompress:
                    _executor.AddTasks(
                        PrepareTasksForDecompression(inputFilePath, outputFilePath, _bufferSize));
                    break;
                default:
                    throw new ArgumentException("Unknown archiver operation type.");
            }
            _executor.Start();
        }

        public void Abort()
        {
            _executor.Stop();
            _inputChunkHolder.Clear();
            _outputChunkHolder.Clear();
        }

        /// <summary>
        /// Create all necessary tasks to compress a input file to a output one.
        /// Use a "by buffer size" strategy to separate a file to set of chunks.
        /// </summary>
        /// <param name="inputFilePath">Input file to compress</param>
        /// <param name="outputFilePath">Result output file with compressed data</param>
        /// <param name="bufferSize">File's chunks size</param>
        /// <returns>Tasks to compress a whole file by its chunks</returns>
        private IEnumerable<BaseTask> PrepareTasksForCompression(string inputFilePath, string outputFilePath, int bufferSize)
        {
            var tasks = new List<BaseTask>(0);
            var fileLength = new FileInfo(inputFilePath).Length;
            var availableBytes = fileLength;
            var chunkIndex = 0;
            while (availableBytes > 0)
            {
                var readCount = availableBytes < bufferSize
                    ? (int) availableBytes
                    : bufferSize;

                tasks.Add(new CompressionTask(chunkIndex, _inputChunkHolder, _outputChunkHolder, _compressor,
                    inputFilePath, fileLength - availableBytes, readCount, outputFilePath));

                availableBytes -= readCount;
                chunkIndex++;
            }

            return tasks;
        }

        /// <summary>
        /// Create all necessary tasks to decompress a input file to a output one.
        /// Use a "by gzip blocks" strategy to separate a file to set of compressed chunks.
        /// </summary>
        /// <param name="inputFilePath">Input file to decompress</param>
        /// <param name="outputFilePath">Result output file with decompressed data</param>
        /// <param name="bufferSize">Reading stream's buffer size</param>
        /// <returns>Tasks to decompress a whole file by its GZip-compressed chunks</returns>
        private IEnumerable<BaseTask> PrepareTasksForDecompression(string inputFilePath, string outputFilePath, int bufferSize)
        {
            var tasks = new List<BaseTask>(0);
            using (var reader = new BinaryReader(File.Open(inputFilePath, FileMode.Open, FileAccess.Read)))
            {
                var gzipHeader = Constants.GZipDefaultHeader;

                var fileLength = new FileInfo(inputFilePath).Length;
                var availableBytes = fileLength;
                var chunkIndex = 0;
                while (availableBytes > 0)
                {
                    var gzipBlock = new List<byte>(bufferSize);

                    // GZip header.
                    if (chunkIndex == 0) // Get first GZip header in the file. All internal gzip blocks have the same one.
                    {
                        gzipHeader = reader.ReadBytes(gzipHeader.Length);
                        availableBytes -= gzipHeader.Length;
                    }
                    gzipBlock.AddRange(gzipHeader);

                    // GZipped data.
                    var gzipHeaderMatchsCount = 0;
                    while (availableBytes > 0)
                    {
                        var curByte = reader.ReadByte();
                        gzipBlock.Add(curByte);
                        availableBytes--;

                        // Check a header of the next gzip block.
                        if (curByte == gzipHeader[gzipHeaderMatchsCount])
                        {
                            gzipHeaderMatchsCount++;
                            if (gzipHeaderMatchsCount != gzipHeader.Length)
                                continue;

                            gzipBlock.RemoveRange(gzipBlock.Count - gzipHeader.Length, gzipHeader.Length); // Remove gzip header of the next block from a rear of this one.
                            break;
                        }

                        gzipHeaderMatchsCount = 0;
                    }

                    var gzipBlockStartPosition = 0L;
                    var gzipBlockLength = gzipBlock.ToArray().Length;
                    if (chunkIndex > 0)
                    {
                        gzipBlockStartPosition = fileLength - availableBytes - gzipHeader.Length - gzipBlockLength;
                        if (gzipBlockStartPosition + gzipHeader.Length + gzipBlockLength == fileLength) // The last gzip block in a file.
                            gzipBlockStartPosition += gzipHeader.Length;
                    }

                    tasks.Add(new DecompressionTask(chunkIndex, _inputChunkHolder, _outputChunkHolder, _compressor,
                        inputFilePath, gzipBlockStartPosition, gzipBlockLength, outputFilePath));

                    chunkIndex++;
                }
            }

            return tasks;
        }
    }
}