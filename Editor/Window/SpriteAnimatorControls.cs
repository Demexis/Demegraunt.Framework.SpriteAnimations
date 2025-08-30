using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Demegraunt.Framework.Editor {
    internal sealed class SpriteAnimatorControls {
        public readonly ToolbarMenu fileToolbarMenu;
        public readonly VisualElement imagePreviewPanel;
        public readonly Image imagePreview;
        public readonly ObjectField previewObjectField;
        public readonly InspectorElement previewSelectedInspectorElement;

        public readonly ProgressBar previewProgressBar;
        public readonly ColorField backgroundColorField;
        public readonly Button previousFrameButton;
        public readonly Button playPauseButton;
        public readonly Button nextFrameButton;

        public SpriteAnimatorControls(VisualElement root) {
            fileToolbarMenu = root.Q<ToolbarMenu>("file-toolbar-menu");

            imagePreviewPanel = root.Q("preview-image-panel");
            imagePreview = new Image();
            imagePreview.style.flexGrow = new StyleFloat(1);
            imagePreviewPanel.Add(imagePreview);

            previewObjectField = root.Q<ObjectField>("preview-object-field");

            var previewInspectorPanel = root.Q("preview-inspector-panel");
            previewSelectedInspectorElement = new InspectorElement();
            previewInspectorPanel.Add(previewSelectedInspectorElement);

            previewProgressBar = root.Q<ProgressBar>("preview-progress-bar");
            backgroundColorField = root.Q<ColorField>("background-color-field");
            previousFrameButton = root.Q<Button>("previous-frame-button");
            playPauseButton = root.Q<Button>("play-pause-button");
            nextFrameButton = root.Q<Button>("next-frame-button");
        }
    }
}