using System.Threading.Tasks;

namespace _Source.Scripts.Util
{
    public static class TaskUtil
    {
        public static void Forget(this Task task)
        {
            if (!task.IsCompleted || task.IsFaulted)
                _ = ForgetAwaited(task);

            static async Task ForgetAwaited(Task task)
            {
                try
                {
                    await task.ConfigureAwait(false);
                }
                catch
                {
                }
            }
        }
    }
}