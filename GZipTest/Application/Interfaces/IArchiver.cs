using GZipTest.Application.Context;

namespace GZipTest.Application.Interfaces
{
    public interface IArchiver
    {
        void Process(string inputFilePath, string outputFilePath, OperationType operation);
        void Abort();
    }
}