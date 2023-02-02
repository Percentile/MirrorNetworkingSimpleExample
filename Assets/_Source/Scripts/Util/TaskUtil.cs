using System.Threading;
using System.Threading.Tasks;

namespace _Source.Scripts.Util
{
    public static class TaskUtil
    {
        public static void FireAndForget(this Task task)
            => Task.Factory.StartNew(
                async () => await task, 
                CancellationToken.None, 
                TaskCreationOptions.None, 
                TaskScheduler.FromCurrentSynchronizationContext());
    }
}