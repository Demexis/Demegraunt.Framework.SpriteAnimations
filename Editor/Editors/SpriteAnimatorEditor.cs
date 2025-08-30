using UnityEditor;

namespace Demegraunt.Framework.Editor {
    [CustomEditor(typeof(SpriteAnimator))]
    internal sealed class SpriteAnimatorEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            var spriteAnimator = (SpriteAnimator)target;
            var debugString = GetDebugString(spriteAnimator);

            EditorGUILayout.TextField("Debug", debugString);
        }

        private string GetDebugString(SpriteAnimator spriteAnimator) {
            var str = string.Empty;
            if (spriteAnimator.SpriteAnimationPlayer == null) {
                str += "[NO PLAYER]";
            } else if (spriteAnimator.SpriteAnimationPlayer.Playback == null) {
                str += "[NO PLAYBACK]";
            } else {
                str += spriteAnimator.SpriteAnimationPlayer.Playback.animation.AnimationName;
                str += $"\nPlayback index: {spriteAnimator.SpriteAnimationPlayer.Playback.Index}";
                str += $"\nPlayback timer: {spriteAnimator.SpriteAnimationPlayer.Playback.Timer}";
                str += $"\nFrames count: {spriteAnimator.SpriteAnimationPlayer.Playback.animation.FrameSprites?.Count}";
            }
                
            return str;
        }
    }
}