using JetBrains.Annotations;
using System;
using UnityEngine;

namespace Demegraunt.Framework {
    public sealed class SpritePlayback {
        public const float ONE_FRAME_TIME = 0.083f;
        
        public bool Finished { get; private set; }
        public int Index { get; private set; }
        public float Timer { get; private set; }
        
        /// <summary>
        /// Is invoked whenever animation loops through all sprite frames.
        /// </summary>
        private readonly Action finishedFrameLoop;

        public readonly ISpriteAnimation animation;
        private readonly Action<Sprite> spriteSet;
        private readonly SpriteAnimationPlayerSettings settings;

        public SpritePlayback(ISpriteAnimation animation, Action<Sprite> spriteSet, Action finishedFrameLoop = null, SpriteAnimationPlayerSettings settings = default) {
            this.finishedFrameLoop = finishedFrameLoop;
            this.spriteSet = spriteSet;
            this.settings = settings;
            this.animation = animation;
            
            if (animation == null) {
                Finished = true;
                Debug.LogError($"{nameof(SpritePlayback)} initialized with no {nameof(SpriteAnimation)}.");
                return;
            }
            
            if (animation.FrameSprites.Count == 0) {
                Finished = true;
                finishedFrameLoop?.Invoke();
                Debug.LogWarning($"{nameof(SpritePlayback)} initialized with {nameof(SpriteAnimation)} containing no frames.");
                return;
            }
            
            if (settings.FinishInstantly) {
                Finished = true;
                spriteSet.Invoke(settings.Reversed ? animation.FrameSprites[0] : animation.FrameSprites[^1]);
                finishedFrameLoop?.Invoke();
                return;
            }
            
            this.spriteSet.Invoke(settings.Reversed ? animation.FrameSprites[^1] : animation.FrameSprites[0]);
            Index = settings.Reversed ? animation.FrameSprites.Count - 1 : 0;
            Timer = ONE_FRAME_TIME;
        }

        public void Update(float deltaTime) {
            if (Finished) {
                return;
            }
            
            Timer -= deltaTime * animation.Speed;

            while (Timer < 0) {
                Timer += ONE_FRAME_TIME;
                
                if (!ChangeIndex()) {
                    return;
                }
                
                spriteSet.Invoke(animation.FrameSprites[Index]);
            }
        }

        private bool ChangeIndex() {
            var index = Index + (settings.Reversed ? -1 : 1);

            return ChangeIndex(index);
        }

        private bool ChangeIndex(int index) {
            Index = index;
            
            if (!IsLastFrame()) {
                return true;
            }
            
            if (animation.Loop) {
                Index = settings.Reversed ? animation.FrameSprites.Count - 1 : 0;
                finishedFrameLoop?.Invoke();
            } else {
                Finished = true;
                finishedFrameLoop?.Invoke();
                return false;
            }

            return true;

            bool IsLastFrame() {
                if (settings.Reversed) {
                    return Index == -1;
                }
                
                return Index == animation.FrameSprites.Count;
            }
        }

        public float GetPlaybackProgress01() {
            if (animation == null || animation.FrameSprites.Count == 0) {
                return 1f;
            }
            
            var indexFraction = 1f / animation.FrameSprites.Count;
            var timeProportion = (ONE_FRAME_TIME - Timer) / ONE_FRAME_TIME;

            var progress = indexFraction * Index + indexFraction * timeProportion;
            
            return progress;
        }

        public float GetTotalTime() {
            if (animation == null || animation.FrameSprites.Count == 0) {
                return 0;
            }
            
            return animation.FrameSprites.Count * ONE_FRAME_TIME / animation.Speed;
        }

        public float GetPlayedTime() {
            return GetTotalTime() * GetPlaybackProgress01();
        }

        public float GetLeftTime() {
            return GetTotalTime() - GetPlayedTime();
        }

        public void SetIndex(int index) {
            if (animation.FrameSprites.Count == 0) {
                return;
            }

            index = Mathf.Clamp(index, 0, animation.FrameSprites.Count - 1);

            Timer = ONE_FRAME_TIME;
            Finished = false;
            ChangeIndex(index);
        }

        [CanBeNull] public Sprite GetCurrentFrame() {
            if (animation.FrameSprites.Count == 0) {
                return null;
            }
            
            return animation.FrameSprites[Index];
        }
    }
}