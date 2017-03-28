using System;
using System.Threading;

namespace GZipTest.Application.Tasks
{
    public sealed class TaskEventArgs : EventArgs
    {
        public readonly Thread Thread;

        public TaskEventArgs(Thread thread)
        {
            Thread = thread;
        }
    }
}