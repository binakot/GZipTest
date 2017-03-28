using System.Collections.Generic;

namespace GZipTest.Application.Interfaces
{
    public interface IExecutor<T>
    {
        void AddTask(T task);
        void AddTasks(IEnumerable<T> tasks);
        void Start();
        void Stop();
    }
}