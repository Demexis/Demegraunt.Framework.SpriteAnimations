using System.Diagnostics.Contracts;
using UnityEngine;

namespace Demegraunt.Framework {
    internal static class FloatExtensions {
        [Pure]
        public static bool IsApproximately(this float f, float value, float threshold = 0.001f) {
            return Mathf.Abs(f - value) < threshold;
        }
    }
}