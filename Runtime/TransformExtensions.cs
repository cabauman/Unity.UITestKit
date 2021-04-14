using System;
using UnityEngine;

namespace GameCtor.UITestKit
{
    internal static class TransformExtensions
    {
        public static string GetPath(this Transform @this)
        {
            if (@this == null)
            {
                throw new ArgumentNullException(nameof(@this));
            }

            if (@this.parent == null)
            {
                return "/" + @this.name;
            }

            return @this.parent.GetPath() + "/" + @this.name;
        }
    }
}
