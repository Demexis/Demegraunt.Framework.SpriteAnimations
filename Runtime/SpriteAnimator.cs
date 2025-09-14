using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Demegraunt.Framework {
    public sealed class SpriteAnimator : MonoBehaviour {
        [field: SerializeField] public List<SpriteAnimation> Animations { get; set; } = new();
        [field: SerializeField] public List<SpriteAnimationContainer> AnimationContainers { get; set; } = new();
        [field: SerializeField] public float Speed { get; set; } = 1f;
        
        [field: SerializeField] public bool BindSpriteRenderer { get; set; } = true;
        [field: SerializeField] public UnityEvent<Sprite> OnSpriteChanged { get; set; } = new();
        
        public SpriteAnimationPlayer SpriteAnimationPlayer => spriteAnimationPlayer ??= CreatePlayer();
        private SpriteAnimationPlayer spriteAnimationPlayer;
        
        private SpriteRenderer sR;
        
        private void Awake() {
            sR = GetComponent<SpriteRenderer>();
            spriteAnimationPlayer ??= CreatePlayer();
            spriteAnimationPlayer.SpriteChanged += PlayerOnSpriteChanged;
        }
        
        private void OnDestroy() {
            if (spriteAnimationPlayer != null) {
                spriteAnimationPlayer.SpriteChanged -= PlayerOnSpriteChanged;
            }
        }

        private void PlayerOnSpriteChanged(Sprite sprite) {
            if (BindSpriteRenderer && sR != null) {
                sR.sprite = sprite;
            }
            
            OnSpriteChanged.Invoke(sprite);
        }

        private SpriteAnimationPlayer CreatePlayer() {
            return new SpriteAnimationPlayer(Animations.Concat(AnimationContainers.Select(x => x.SpriteAnimation))) {
                Speed = Speed
            };
        }

        private void Update() {
            SpriteAnimationPlayer.Update(Time.deltaTime);
        }

        [UsedImplicitly]
        public void PlayImplicitly(string animationName) {
            Play(animationName);
        }
        
        [UsedImplicitly]
        public void PlayImplicitly(SpriteAnimationContainer animation) {
            Play(animation);
        }

        public bool Play(string animationName, float speed = 1f, Action finishedFrameLoop = null, SpriteAnimationPlayerSettings settings = default) {
            return SpriteAnimationPlayer.Play(animationName, speed, finishedFrameLoop, settings);
        }

        public void Play(SpriteAnimationContainer animation, float speed = 1f, Action finishedFrameLoop = null, SpriteAnimationPlayerSettings settings = default) {
            SpriteAnimationPlayer.Play(animation.SpriteAnimation, speed, finishedFrameLoop, settings);
        }

        public float GetCurrentAnimationTime => SpriteAnimationPlayer.GetCurrentAnimationTime;
    }
}
