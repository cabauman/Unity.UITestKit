using System;
using System.Collections;
using System;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameCtor.UITestKit
{
    public partial class UITest
    {
        protected IEnumerator WaitUntilActive<T>(float timeout = 1f)
            where T : Component
        {
            return WaitAsync(new WaitUntilActiveInHierarchy<T>(), timeout, Environment.StackTrace).AsIEnumerator();
        }

        protected IEnumerator WaitUntilInactive<T>(float timeout = 1f)
            where T : Component
        {
            return WaitAsync(new WaitUntilInactiveInHierarchy<T>(), timeout, Environment.StackTrace).AsIEnumerator();
        }

        protected IEnumerator Tap(string buttonName)
        {
            return TapInternal(buttonName).AsIEnumerator();
        }

        protected async Task WaitAsync(Condition condition, float timeout, string stackTrace)
        {
            var startTime = Time.time;

            while (!condition.IsFulfilled())
            {
                var duration = Time.time - startTime;
                if (duration > timeout)
                {
                    SimplifyStackTrace(ref stackTrace);
                    throw new TimeoutException("Operation timed out: " + condition.GetResult() + "\n" + stackTrace);
                }

                await Task.Delay(100);
            }
        }

        private static void SimplifyStackTrace(ref string stackTrace)
        {
            var sb = new StringBuilder();
            using (StringReader reader = new StringReader(stackTrace))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (!line.StartsWith("  at System") && !line.StartsWith("  at UnityEngine") && !line.EndsWith(":0 "))
                    {
                        sb.Append(line + "\n");
                    }
                }
            }

            stackTrace = sb.ToString();
        }

        private async Task TapInternal(string buttonName)
        {
            var condition = new WaitForInteractableButton(buttonName);
            await WaitAsync(condition, 1f, Environment.StackTrace);
            ExecuteEvents.Execute(condition.ButtonObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
        }
    }
}
