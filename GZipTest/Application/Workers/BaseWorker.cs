using GZipTest.Application.Interfaces;

namespace GZipTest.Application.Workers
{
    public abstract class BaseWorker : IWorker
    {
        protected volatile bool IsStopped;

        public abstract void Start();

        public void Stop()
        {
            IsStopped = true;
        }
    }
}