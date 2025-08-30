using UnityEditor;
using UnityEngine;

namespace Demegraunt.Framework.Editor {
    internal static class EditorUtils {
        public static Texture2D GetUnityEditorIcon(string iconName) {
            return (Texture2D)EditorGUIUtility.IconContent(iconName).image;
        }
    }
}