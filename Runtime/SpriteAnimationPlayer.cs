using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demegraunt.Framework {
    public sealed class SpriteAnimationPlayer {
        public event Action<Sprite> SpriteChanged;
        
        public SpritePlayback Playback { get; private set; }
        public float Speed { get; set; } = 1f;
        
        [CanBeNull] private IEnumerable<SpriteAnimation> SpriteAnimations { get; }

        public SpriteAnimationPlayer(IEnumerable<SpriteAnimation> spriteAnimations) {
            SpriteAnimations = spriteAnimations;
        }

        public void Update(float deltaTime) {
            if (Playback == null) {
                return;
            }

            Playback.Update(deltaTime * Speed);

            if (Playback.Finished) {
                Playback = null;
            }
        }

        public bool Play(string animationName, float speed = 1f, Action finishedFrameLoop = null, SpriteAnimationPlayerSettings settings = default) {
            if (SpriteAnimations != null) {
                foreach (var anim in SpriteAnimations) {
                    if (anim.AnimationName.Equals(animationName)) {
                        Play(anim, speed, finishedFrameLoop, settings);
                        return true;
                    }
                }
            }

            Debug.LogWarning("Can't find animation \"" + animationName + "\" in storage.");
            return false;
        }

        public void Play(ISpriteAnimation animation, float speed = 1f, Action finishedFrameLoop = null, SpriteAnimationPlayerSettings settings = default) {
            // synchronize speed
            Speed = speed;

            Playback = new SpritePlayback(animation, (sprite) => {
                SpriteChanged?.Invoke(sprite);
            }, finishedFrameLoop, settings);
        }

        public void Stop() {
            Playback = null;
        }

        public void ForceSpriteChange(Sprite sprite) {
            SpriteChanged?.Invoke(sprite);
        }

        public float GetCurrentAnimationTime => Playback?.GetTotalTime() ?? 0;
    }
}