using System.IO;
using GZipTest.Application.Compression;
using GZipTest.Application.Files;
using GZipTest.Application.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GZipTestTests.Application.Tasks
{
    [TestClass]
    public class DecompressionTaskTests
    {
        [TestMethod]
        public void TestExecuteDecompressionTaskOnWholeFile()
        {
            var inputFile = new FileInfo("input.txt.gz");
            var outputFile = new FileInfo("output.txt");
            File.Delete(outputFile.Name);

            var inputHolder = new FileChunkHolder();
            var outputHolder = new FileChunkHolder();
            var compressor = new GZipCompressor();

            var task = new DecompressionTask(0, inputHolder, outputHolder, compressor, 
                inputFile.Name, 0, (int) inputFile.Length, outputFile.Name);
            task.Start();

            Assert.AreEqual(0, inputHolder.Count());
            Assert.AreEqual(0, outputHolder.Count());
            CollectionAssert.AreEqual(compressor.Decompress(File.ReadAllBytes(inputFile.Name)), File.ReadAllBytes(outputFile.Name));
        }
    }
}