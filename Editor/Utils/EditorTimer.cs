using UnityEditor;

namespace Demegraunt.Framework.Editor {
    internal sealed class EditorTimer {
        public double DeltaTime => editorDeltaTime;
        
        private double editorDeltaTime;
        private double lastTimeSinceStartup;

        public void SetEditorDeltaTime() {
            if (lastTimeSinceStartup == 0f) {
                lastTimeSinceStartup = EditorApplication.timeSinceStartup;
            }
            editorDeltaTime = EditorApplication.timeSinceStartup - lastTimeSinceStartup;
            lastTimeSinceStartup = EditorApplication.timeSinceStartup;
        }
    }
}