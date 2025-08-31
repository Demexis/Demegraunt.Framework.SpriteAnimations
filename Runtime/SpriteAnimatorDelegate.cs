using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

namespace Demegraunt.Framework {
    public sealed class SpriteAnimatorDelegate : MonoBehaviour {
        [field: SerializeField] public SpriteAnimator SpriteAnimator { get; set; }
        [field: SerializeField] public SpriteAnimationContainer SpriteAnimation { get; set; }
        [field: SerializeField] public float AnimationSpeed { get; set; } = 1f;

        [field: SerializeField] public UnityEvent<SpriteAnimationContainer> OnTrigger { get; set; } = new();
        [field: SerializeField] public UnityEvent OnAnimationFinished { get; set; } = new();

        [UsedImplicitly]
        public void InvokeTrigger() {
            SpriteAnimator.Play(SpriteAnimation, AnimationSpeed, () => OnAnimationFinished.Invoke());
            OnTrigger?.Invoke(SpriteAnimation);
        }
        
        [UsedImplicitly]
        public void InvokeTrigger(string animationName) {
            SpriteAnimator.Play(animationName, AnimationSpeed, () => OnAnimationFinished.Invoke());
            OnTrigger?.Invoke(SpriteAnimation);
        }
    }
}