using System.IO;
using System.Text;
using GZipTest.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GZipTestTests.Utils
{
    [TestClass]
    public sealed class ExtensionsTests
    {
        [TestMethod]
        public void TestEmptyStreamCopyTo()
        {
            var inputStream = new MemoryStream(new byte[0]);
            var outpupStream = new MemoryStream();
            inputStream.Copy(outpupStream);

            Assert.AreEqual(inputStream.Length, outpupStream.Length);
        }

        [TestMethod]
        public void TestSomeStreamCopyTo()
        {
            var inputStream = new MemoryStream(Encoding.UTF8.GetBytes("Hello, Veeam!"));
            var outpupStream = new MemoryStream();
            inputStream.Copy(outpupStream);

            Assert.AreEqual(inputStream.Length, outpupStream.Length);
        }

        [TestMethod]
        public void TestRealAllBytesFrommEmptyFile()
        {
            var file = new FileInfo("empty.txt");
            var originalBytes = File.ReadAllBytes(file.Name);
            using (var inputFileStream = new FileStream(file.Name, FileMode.Open, FileAccess.Read))
            {
                var readBytes = inputFileStream.ReadAllBytes();
                Assert.AreEqual(originalBytes.Length, readBytes.Length);
                CollectionAssert.AreEqual(originalBytes, readBytes);
            }
        }

        [TestMethod]
        public void TestRealAllBytesFromSomeFile()
        {
            var file = new FileInfo("input.txt");
            var originalBytes = File.ReadAllBytes(file.Name);
            using (var inputFileStream = new FileStream(file.Name, FileMode.Open, FileAccess.Read))
            {
                var readBytes = inputFileStream.ReadAllBytes();
                Assert.AreEqual(originalBytes.Length, readBytes.Length);
                CollectionAssert.AreEqual(originalBytes, readBytes);
            }
        }

        [TestMethod]
        public void TestGetRangeSubsetFromEmptyArray()
        {
            var emptyArray = new byte[0];
            var subset = emptyArray.RangeSubset(0, 0);
            CollectionAssert.AreEqual(subset, emptyArray);
        }

        [TestMethod]
        public void TestGetRangeSubsetFromSomeArray()
        {
            var array = Encoding.UTF8.GetBytes("AVAILABILITY for the Always-On Enterprise™");
            var subset1 = array.RangeSubset(0, 12);
            Assert.AreEqual("AVAILABILITY", Encoding.UTF8.GetString(subset1));
            var subset2 = array.RangeSubset(21, 9);
            Assert.AreEqual("Always-On", Encoding.UTF8.GetString(subset2));
            var subset3 = array.RangeSubset(array.Length - 13, 13);
            Assert.AreEqual("Enterprise™", Encoding.UTF8.GetString(subset3));
        }
    }
}