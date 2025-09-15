using JetBrains.Annotations;
using SFB;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Application = UnityEngine.Application;

namespace Demegraunt.Framework.Editor {
    public sealed class SpriteAnimatorWindow : EditorWindow {
        [field: SerializeField] private VisualTreeAsset MainUxml { get; set; }

        private const string WINDOW_NAME = "Sprite Animator";
        private const string WINDOW_ICON_NAME = "d_SpriteAtlasAsset Icon";

        private SpriteAnimatorControls controls;
        private SpriteAnimatorWindow window;

        [CanBeNull] private SpriteAnimationContainer CurrentSpriteAnimation { get; set; }
        private bool previewIsPlaying;
        private SpritePlayback spritePlayback;

        private readonly List<Action> updateActions = new();
        private readonly EditorTimer editorTimer = new();

        [MenuItem("Tools/Demegraunt/Sprite Animator")]
        public static void ShowWindow() {
            var singleWindow = (SpriteAnimatorWindow)GetWindow(typeof(SpriteAnimatorWindow));
            singleWindow.titleContent = new GUIContent(WINDOW_NAME, EditorUtils.GetUnityEditorIcon(WINDOW_ICON_NAME));
            singleWindow.Show();
        }

        private void OnEnable() {
            updateActions.Add(() => {
                if (window == null) {
                    window = this;
                }

                window.MainUxml.CloneTree(rootVisualElement);

                var openAnswer = OpenInit();

                if (!openAnswer.Success) {
                    window.Close();
                    window = null;

                    EditorUtility.DisplayDialog("Failed to open a window", openAnswer.Message, "Ok");
                }
            });
        }
        
        private void Update() {
            editorTimer.SetEditorDeltaTime();
            
            if (updateActions.Count == 0) {
                return;
            }

            for (var i = updateActions.Count - 1; i >= 0; i--) {
                updateActions[i]?.Invoke();
                updateActions.RemoveAt(i);
            }
        }
        
        private void AddSingleUpdate(Action action) {
            updateActions.Add(action);
        }

        private WindowOpenResult OpenInit() {
            controls = new SpriteAnimatorControls(rootVisualElement);

            InitFileToolbarMenu();
            InitPreviousButton();
            InitNextButton();
            InitPlayPauseButton();
            InitPreviewObjectField();
            InitPreviewColorField();
            UpdatePlaybackRecursive();

            return new WindowOpenResult(true, string.Empty);

            void InitFileToolbarMenu() {
                controls.fileToolbarMenu.menu.AppendAction("Create New...", action => {
                    var extensionList = new[] {
                        new ExtensionFilter("Asset", "asset"),
                    };
                    var path = StandaloneFileBrowser.SaveFilePanel("Save Animation SO File", Application.dataPath,
                        "animation.asset", extensionList);

                    if (string.IsNullOrWhiteSpace(path)) {
                        return;
                    }

                    var animationAsset = CreateInstance<SpriteAnimationContainer>();

                    if (!AssetDatabaseUtils.TrySaveAsset(animationAsset, path)) {
                        EditorUtility.DisplayDialog("Failed to save file",
                            "Couldn't save an asset-file.", "Ok :(");
                        return;
                    }

                    controls.previewObjectField.value = animationAsset;
                });
            }

            void InitPreviousButton() {
                var icon = EditorUtils.GetUnityEditorIcon("StepLeftButton");
                controls.previousFrameButton.style.backgroundImage = new StyleBackground(icon);
                controls.previousFrameButton.SetScaleToFit();

                controls.previousFrameButton.clicked += () => {
                    previewIsPlaying = false;
                    UpdatePlayPauseIcon();
                    spritePlayback?.SetIndex(spritePlayback.Index - 1);
                    UpdatePreviewProgressbar();
                    UpdatePreviewImage();
                };
            }

            void InitNextButton() {
                var icon = EditorUtils.GetUnityEditorIcon("StepButton");
                controls.nextFrameButton.style.backgroundImage = new StyleBackground(icon);
                controls.nextFrameButton.SetScaleToFit();

                controls.nextFrameButton.clicked += () => {
                    previewIsPlaying = false;
                    UpdatePlayPauseIcon();
                    spritePlayback?.SetIndex(spritePlayback.Index + 1);
                    UpdatePreviewProgressbar();
                    UpdatePreviewImage();
                };
            }

            void InitPlayPauseButton() {
                UpdatePlayPauseIcon();
                controls.playPauseButton.clicked += () => {
                    if (CurrentSpriteAnimation == null) {
                        previewIsPlaying = false;
                        UpdatePreviewImage();
                        EditorUtility.DisplayDialog("Select an animation",
                            "You first need to create an animation-asset (File -> Create New...) or select it in preview settings.",
                            "Ok");
                        return;
                    }

                    if (spritePlayback != null && spritePlayback.Finished) {
                        ResetPreview();
                    }

                    previewIsPlaying = !previewIsPlaying;

                    UpdatePlayPauseIcon();
                };
            }

            void InitPreviewObjectField() {
                controls.previewObjectField.objectType = typeof(SpriteAnimationContainer);
                controls.previewObjectField.RegisterValueChangedCallback(evt => {
                    SelectSpriteAnimation((SpriteAnimationContainer)evt.newValue);
                });
            }

            void InitPreviewColorField() {
                controls.imagePreviewPanel.style.backgroundColor = controls.imagePreview.style.backgroundColor =
                    controls.backgroundColorField.value;
                controls.backgroundColorField.RegisterValueChangedCallback(evt => {
                    controls.imagePreviewPanel.style.backgroundColor =
                        controls.imagePreview.style.backgroundColor = evt.newValue;
                });
            }

            void UpdatePlaybackRecursive() {
                UpdateSpritePlayback();
                AddSingleUpdate(UpdatePlaybackRecursive);
            }
        }

        private void SelectSpriteAnimation(SpriteAnimationContainer spriteAnimation) {
            CurrentSpriteAnimation = spriteAnimation;

            ResetPreview();
            UpdatePreviewImage();
            UpdatePreviewProgressbar();

            if (CurrentSpriteAnimation == null) {
                controls.previewSelectedInspectorElement.Unbind();
            } else {
                controls.previewSelectedInspectorElement.Bind(new SerializedObject(CurrentSpriteAnimation));
            }
        }

        private void ResetPreview() {
            previewIsPlaying = false;

            spritePlayback = null;

            controls.previewProgressBar.value = 0;
            controls.previewProgressBar.title = string.Empty;

            if (CurrentSpriteAnimation != null) {
                spritePlayback =
                    new SpritePlayback(CurrentSpriteAnimation.SpriteAnimation, _ => UpdatePreviewImage(), () => {
                        if (CurrentSpriteAnimation.SpriteAnimation.Loop) {
                            return;
                        }

                        previewIsPlaying = false;
                        UpdatePlayPauseIcon();
                    });

                UpdatePreviewProgressbar();
                controls.previewProgressBar.title = CurrentSpriteAnimation.SpriteAnimation.AnimationName;
            }
        }

        private void UpdateSpritePlayback() {
            if (!previewIsPlaying) {
                return;
            }

            if (CurrentSpriteAnimation == null) {
                return;
            }

            if (spritePlayback == null) {
                return;
            }

            spritePlayback.Update((float)editorTimer.DeltaTime);

            UpdatePreviewProgressbar();
        }

        private void UpdatePreviewProgressbar() {
            if (spritePlayback == null) {
                return;
            }

            if (CurrentSpriteAnimation == null) {
                return;
            }

            controls.previewProgressBar.value = spritePlayback.GetPlaybackProgress01();

            var totalCount = spritePlayback.animation.FrameSprites.Count;
            var currentFrame = spritePlayback.Index;
            
            var frameIndicator = $"({currentFrame}/{totalCount})";
            var timeIndicator = $"({spritePlayback.GetPlayedTime():F2} s /{spritePlayback.GetTotalTime():F2} s)";

            controls.previewProgressBar.title =
                CurrentSpriteAnimation.SpriteAnimation.AnimationName + $" {frameIndicator} {timeIndicator}";
        }

        private void UpdatePlayPauseIcon() {
            var icon = EditorUtils.GetUnityEditorIcon(previewIsPlaying ? "d_PauseButton@2x" : "d_PlayButton@2x");
            controls.playPauseButton.style.backgroundImage = new StyleBackground(icon);
            controls.playPauseButton.SetScaleToFit();
        }

        private void UpdatePreviewImage() {
            if (spritePlayback == null) {
                controls.imagePreview.sprite = null;
                return;
            }

            controls.imagePreview.sprite = spritePlayback.GetCurrentFrame();
        }
    }
}