using System.Collections;
using System.Threading.Tasks;

namespace GameCtor.UITestKit
{
    public static class TaskExtensions
    {
        public static IEnumerator AsIEnumerator(this Task task)
        {
            while (!task.IsCompleted)
            {
                yield return null;
            }

            if (task.IsFaulted)
            {
                throw
                    task.Exception.InnerExceptions != null && task.Exception.InnerExceptions.Count == 1
                        ? task.Exception.InnerExceptions[0]
                        : task.Exception;
            }
        }
    }
}
