using System;
using System.Collections.Generic;
using System.Threading;
using GZipTest.Application.Interfaces;

namespace GZipTest.Application.Tasks
{
    public abstract class BaseTask : ITask
    {
        public event EventHandler<TaskEventArgs> TaskDone;

        private readonly List<ICommand> _pipeline = new List<ICommand>(0);

        public void Start()
        {
            foreach (var command in _pipeline)
                command.Execute();
            _pipeline.Clear();

            TaskDone?.Invoke(this, new TaskEventArgs(Thread.CurrentThread));
        }

        protected void AddCommand(ICommand command)
        {
            _pipeline.Add(command);
        }
    }
}