using System;
using System.IO;
using System.Threading;
using GZipTest.Application.Compression;
using GZipTest.Application.Context;
using GZipTest.Application.Interfaces;
using GZipTest.Utils;

namespace GZipTest.Application
{
    public sealed class GZipApplication : IApplication
    {
        private readonly IArchiver _archiver;
        private readonly Parameters _parameters;

        private volatile bool _isTerminated;

        public GZipApplication(Parameters parameters)
        {
            _archiver = new GZipArchiver(Environment.ProcessorCount, 512 * Constants.MegabyteInBytes, 1024 * Constants.MemoryPageSize); // TODO Take about 75 % of total available RAM.
            _parameters = parameters;
        }

        public void Run()
        {
            var archiverThread =
                new Thread(
                    () =>
                        _archiver.Process(_parameters.InputFilePath, _parameters.OutputFilePath, _parameters.Operation))
                {
                    Name = "Archiver"
                };
            archiverThread.Start();

            using (var spinner = new ConsoleSpinner())
            {
                while (archiverThread.IsAlive)
                    spinner.Turn();
            }

            if (!_isTerminated)
                OutputResult();
        }

        public void Exit()
        {
            _isTerminated = true;
            _archiver.Abort();
        }

        private void OutputResult()
        {
            var inputFile = new FileInfo(_parameters.InputFilePath);
            var outputFile = new FileInfo(_parameters.OutputFilePath);
            switch (_parameters.Operation)
            {
                case OperationType.Compress:
                    Console.Out.WriteLine("Compressed {0} from {1} to {2} bytes. Compression efficiency {3:0.00} %.",
                        inputFile.Name, inputFile.Length, outputFile.Length, inputFile.Length * 100.0 / outputFile.Length);
                    break;

                case OperationType.Decompress:
                    Console.Out.WriteLine("Decompressed {0} from {1} to {2} bytes.",
                        inputFile.Name, inputFile.Length, outputFile.Length);
                    break;
            }
        }
    }
}