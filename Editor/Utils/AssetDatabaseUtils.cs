using UnityEditor;
using UnityEngine;

namespace Demegraunt.Framework.Editor {
    internal static class AssetDatabaseUtils {
        /// <summary>
        /// Saves asset within 'Assets' folder. Doesn't create directory if is missing.
        /// </summary>
        /// <param name="asset">Asset object.</param>
        /// <param name="absolutePath">Full path with directory, file name, and file extension.</param>
        /// <typeparam name="T">UnityEngine.Object inheritor.</typeparam>
        public static bool TrySaveAsset<T>(T asset, string absolutePath) where T : Object {
            absolutePath = absolutePath.Replace('\\', '/');
            
            if (!absolutePath.StartsWith(Application.dataPath)) {
                EditorUtility.DisplayDialog("Invalid absolute path",
                    "The path should be absolute and within the project 'Assets' folder.", "Ok");
                return false;
            }
            
            var relativePath = "Assets" + absolutePath.Substring(Application.dataPath.Length);
            
            AssetDatabase.CreateAsset(asset, relativePath);
            AssetDatabase.SaveAssets();

            return true;
        }
    }
}