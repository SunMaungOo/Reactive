using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reactive
{
    public class UIScheduler : TaskScheduler
    {
        private Queue<Task> taskQueue = new Queue<Task>();

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return taskQueue;
        }

        protected override void QueueTask(Task task)
        {
            taskQueue.Enqueue(task);
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            taskQueue.Enqueue(task);

            return false;
        }

        public override int MaximumConcurrencyLevel
        {
            get
            {
                return 1;
            }
        }
    }
}
