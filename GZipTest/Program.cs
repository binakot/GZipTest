using System;
using System.Diagnostics;
using System.IO;
using GZipTest.Application;
using GZipTest.Application.Context;
using GZipTest.Application.Interfaces;

[assembly: CLSCompliant(false)]
namespace GZipTest
{
    internal static class Program
    {
        private static IApplication _app;
        private static volatile bool _isInterrupted;

        [STAThread]
        private static void Main(string[] args)
        {
#if DEBUG
            args = new[] {"compress", "input.txt", "input.txt.gz"};
            File.Delete("input.txt.gz");
#endif

            AddUnhandledExceptionHandler();
            AddCancelKeyHandler();

            var stopwatch = new Stopwatch();
            try
            {
                _app = new GZipApplication(Parameters.Parse(args));
                stopwatch.Start();
                _app.Run();
                stopwatch.Stop();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
#if DEBUG
                Console.ReadKey();
#endif
                Environment.Exit(Constants.ErrorExitCode);
            }

            Console.Out.WriteLine(_isInterrupted
                ? "Interrupted with Ctrl-C"
                : $"Successfully done in {stopwatch.Elapsed.TotalSeconds} seconds.");
#if DEBUG
            Console.ReadKey();
#endif
            Environment.Exit(Constants.SuccessExitCode);
        }

        private static void AddUnhandledExceptionHandler()
        {
            AppDomain.CurrentDomain.UnhandledException += (o, e) =>
            {
                Console.Error.WriteLine(e);

                _isInterrupted = true;
                _app.Exit();
            };
        }

        private static void AddCancelKeyHandler()
        {
            Console.CancelKeyPress += (o, e) =>
            {
                e.Cancel = true;

                _isInterrupted = true;
                _app.Exit();
            };
        }
    }
}