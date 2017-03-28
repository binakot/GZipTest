using System;
using System.IO;

namespace GZipTest.Application.Context
{
    public struct Parameters
    {
        public OperationType Operation { get; }
        public string InputFilePath { get; }
        public string OutputFilePath { get; }

        private Parameters(OperationType operation, string inputFilePath, string outputFilePath)
        {
            Operation = operation;
            InputFilePath = inputFilePath;
            OutputFilePath = outputFilePath;
        }

        /// <summary>
        /// Parse command line arguments.
        /// Expected 3 parameters: operation type, input file path, output file path.
        /// Examples: {"compress", "movie.avi", "movie.avi.gz"}, {"decompress", "book.pdf.gz", "movie.pdf"}
        /// </summary>
        /// <param name="args">Array of string parameters</param>
        /// <returns>Application parameters</returns>
        public static Parameters Parse(string[] args)
        {
            if (args == null || args.Length != Constants.CommandLineArgumentsCount)
                throw new ArgumentException("Three command-line parameters are expected: operation type (compress or decompress), input file path, output file path.");

            var operationString = args[Constants.OperationTypeArgumentIndex];
            OperationType operation;
            switch (operationString.ToLower())
            {
                case Constants.CompressOperationString:
                    operation = OperationType.Compress;
                    break;
                case Constants.DecompressOperationString:
                    operation = OperationType.Decompress;
                    break;
                default:
                    throw new ArgumentException($"Unsupported operation: {operationString}. Supported operations list: compress, decompress.");
            }

            var inputFilePath = args[Constants.InputFilePathArgumentIndex];
            if (inputFilePath.IndexOfAny(Path.GetInvalidPathChars()) != -1)
                throw new ArgumentException($"Invalid input file parameter: {inputFilePath}. It must be correct file path.");
            if (!File.Exists(inputFilePath))
                throw new ArgumentException($"Input file with path {inputFilePath} is not exists. Nothing to compress.");

            var outputFilePath = args[Constants.OutputFilePathArgumentIndex];
            if (outputFilePath.IndexOfAny(Path.GetInvalidPathChars()) != -1)
                throw new ArgumentException($"Invalid output file parameter: {outputFilePath}. It must be correct file path.");
            if (File.Exists(outputFilePath))
                throw new ArgumentException($"Output file with path {outputFilePath} is already exists. Remove it or choose another name for output file.");

            return new Parameters(operation, inputFilePath, outputFilePath);
        }
    }
}