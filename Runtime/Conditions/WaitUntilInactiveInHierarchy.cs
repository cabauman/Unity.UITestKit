using UnityEngine;

namespace GameCtor.UITestKit
{
    public partial class UITest
    {
        private class WaitUntilInactiveInHierarchy<T> : Condition
            where T : Component
        {
            private T obj;

            public override bool IsFulfilled()
            {
                if (obj == null)
                {
                    obj = GameObject.FindAnyObjectByType<T>(FindObjectsInactive.Include);
                }

                return obj == null || !obj.gameObject.activeInHierarchy;
            }

            public override string GetResult()
            {
                if (obj == null)
                {
                    return $"Waited for {typeof(T).Name} to become inactive but it wasn't present in the scene.";
                }

                if (obj.gameObject.activeInHierarchy)
                {
                    return $"Waited for {typeof(T).Name} to become inactive but it never did.";
                }

                return "Fulfilled";
            }
        }
    }
}
