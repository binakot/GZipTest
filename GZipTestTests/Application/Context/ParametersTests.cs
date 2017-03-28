using System;
using GZipTest.Application.Context;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GZipTestTests.Application.Context
{
    [TestClass]
    public sealed class ParametersTests
    {
        [TestMethod]
        public void TestNullArguments()
        {
            try
            {
                Parameters.Parse(null);
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                if (!"Three command-line parameters are expected: operation type (compress or decompress), input file path, output file path."
                        .Equals(ex.Message))
                    Assert.Fail();
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestEmptyArguments()
        {
            try
            {
                Parameters.Parse(new string[0]);
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                if (!"Three command-line parameters are expected: operation type (compress or decompress), input file path, output file path."
                        .Equals(ex.Message))
                    Assert.Fail();
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestRedundantArguments()
        {
            try
            {
                Parameters.Parse(new string[Constants.CommandLineArgumentsCount + 1]);
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                if (!"Three command-line parameters are expected: operation type (compress or decompress), input file path, output file path."
                        .Equals(ex.Message))
                    Assert.Fail();
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestUnsupportedOperationTypeArgument()
        {
            try
            {
                Parameters.Parse(new[] {"download", "movie.avi", "archive.gz"});
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                if (!"Unsupported operation: download. Supported operations list: compress, decompress."
                        .Equals(ex.Message))
                    Assert.Fail();
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestInvalidInputFilePathArgument()
        {
            try
            {
                Parameters.Parse(new[] {"compress", "<file>.txt", "archive.gz"});
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                if (!"Invalid input file parameter: <file>.txt. It must be correct file path."
                        .Equals(ex.Message))
                    Assert.Fail();
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestNotExistInputFilePathArgument()
        {
            try
            {
                Parameters.Parse(new[] {"compress", @"C:\Temp\file.txt", "archive.gz"});
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                if (!@"Input file with path C:\Temp\file.txt is not exists. Nothing to compress."
                        .Equals(ex.Message))
                    Assert.Fail();
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }
    }
}