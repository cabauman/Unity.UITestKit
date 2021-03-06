﻿using UnityEngine;

namespace GameCtor.UITestKit
{
    public partial class UITest
    {
        private class WaitUntilActiveInHierarchy<T> : Condition
            where T : Component
        {
            private T obj;

            public override bool IsFulfilled()
            {
                if (obj is null)
                {
                    obj = GameObject.FindObjectOfType(typeof(T)) as T;
                }

                return obj != null && obj.gameObject.activeInHierarchy;
            }

            public override string GetResult()
            {
                if (obj is null)
                {
                    return $"Waited for {typeof(T).Name} to become active but it wasn't present in the scene.";
                }

                if (!obj.gameObject.activeInHierarchy)
                {
                    return $"Waited for {typeof(T).Name} to become active but it never did.";
                }

                return "Fulfilled";
            }
        }

        private class WaitUntilActiveInHierarchy : Condition
        {
            private string name;
            private GameObject go;

            public WaitUntilActiveInHierarchy(string name)
            {
                this.name = name;
            }

            public override bool IsFulfilled()
            {
                if (go is null)
                {
                    go = GameObject.Find(name);
                }

                return go != null && go.activeInHierarchy;
            }

            public override string GetResult()
            {
                if (go is null)
                {
                    return $"Waited for {name} to become active but it wasn't present in the scene.";
                }

                if (!go.activeInHierarchy)
                {
                    return $"Waited for {name} to become active but it never did.";
                }

                return "Fulfilled";
            }
        }
    }
}
