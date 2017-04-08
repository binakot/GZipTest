using System;
using System.Diagnostics;
using System.IO;
using GZipTest.Application.Compression;
using GZipTest.Application.Context;
using GZipTest.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GZipTestTests.Application.Compression
{
    [TestClass]
    //[Ignore]
    public sealed class GZipArchiverTests
    {
        [TestMethod]
        public void TestCompressDecompressTxtFile()
        {
            const string inputFileName = "input.txt";
            const string outputFileName = "output.txt";
            CompressDecompressFile(inputFileName, outputFileName);

            var inputFile = new FileInfo(inputFileName);
            var outputFile = new FileInfo(outputFileName);
            Assert.IsTrue(inputFile.Exists);
            Assert.IsTrue(outputFile.Exists);
            Assert.AreEqual(inputFile.Length, outputFile.Length);
        }

        [TestMethod]
        public void TestCompressDecompressDocxFile()
        {
            const string inputFileName = "input.docx";
            const string outputFileName = "output.docx";
            CompressDecompressFile(inputFileName, outputFileName);

            var inputFile = new FileInfo(inputFileName);
            var outputFile = new FileInfo(outputFileName);
            Assert.IsTrue(inputFile.Exists);
            Assert.IsTrue(outputFile.Exists);
            Assert.AreEqual(inputFile.Length, outputFile.Length);
        }

        [TestMethod]
        public void TestCompressDecompressMp3File()
        {
            const string inputFileName = "input.mp3";
            const string outputFileName = "output.mp3";
            CompressDecompressFile(inputFileName, outputFileName);

            var inputFile = new FileInfo(inputFileName);
            var outputFile = new FileInfo(outputFileName);
            Assert.IsTrue(inputFile.Exists);
            Assert.IsTrue(outputFile.Exists);
            Assert.AreEqual(inputFile.Length, outputFile.Length);
        }

        [TestMethod]
        [Ignore]
        public void TestCompressDecompressAviFile()
        {
            const string inputFileName = "input.avi";
            const string outputFileName = "output.avi";
            CompressDecompressFile(inputFileName, outputFileName);

            var inputFile = new FileInfo(inputFileName);
            var outputFile = new FileInfo(outputFileName);
            Assert.IsTrue(inputFile.Exists);
            Assert.IsTrue(outputFile.Exists);
            Assert.AreEqual(inputFile.Length, outputFile.Length);
        }

        [TestMethod]
        [Ignore]
        public void TestCompressDecompressMkvFile()
        {
            const string inputFileName = "input.mkv";
            const string outputFileName = "output.mkv";
            CompressDecompressFile(inputFileName, outputFileName);

            var inputFile = new FileInfo(inputFileName);
            var outputFile = new FileInfo(outputFileName);
            Assert.IsTrue(inputFile.Exists);
            Assert.IsTrue(outputFile.Exists);
            Assert.AreEqual(inputFile.Length, outputFile.Length);
        }

        private static void CompressDecompressFile(string inputFileName, string outputFileName)
        {
            File.Delete(inputFileName + ".gz");
            File.Delete(outputFileName);
            
            var archiver = new GZipArchiver(Environment.ProcessorCount, 512 * Constants.MegabyteInBytes, 1024 * Constants.MemoryPageSize);

            var watch = new Stopwatch();

            watch.Start();
            archiver.Process(inputFileName, inputFileName + ".gz", OperationType.Compress);
            watch.Stop();
            Console.WriteLine($"Compressed file {inputFileName} in {watch.Elapsed.TotalSeconds} seconds.");
            GCUtils.ForceGC(true);

            watch.Restart();
            archiver.Process(inputFileName + ".gz", outputFileName, OperationType.Decompress);
            watch.Stop();
            Console.WriteLine($"Decompressed file {inputFileName} in {watch.Elapsed.TotalSeconds} seconds.");
            GCUtils.ForceGC(true);
        }
    }
}