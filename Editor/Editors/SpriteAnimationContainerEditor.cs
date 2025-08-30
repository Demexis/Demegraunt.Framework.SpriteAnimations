using UnityEditor;

namespace Demegraunt.Framework.Editor {
    [CustomEditor(typeof(SpriteAnimationContainer))]
    internal sealed class SpriteAnimationContainerEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            var spriteAnimation = (SpriteAnimationContainer)target;

            EditorGUILayout.LabelField("Animation Length", spriteAnimation.SpriteAnimation.GetAnimationLength().ToString("F3"));
        }
    }
}