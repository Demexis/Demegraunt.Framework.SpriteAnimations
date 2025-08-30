using JetBrains.Annotations;
using UnityEngine;

namespace Demegraunt.Framework {
    public readonly struct SpriteAnimationPlayerSettings {
        public bool Reversed { get; }
        public bool FinishInstantly { get; }
        public bool SetDefaultSpriteForMissingParts { get; }
        [CanBeNull] public Sprite DefaultSprite { get; }

        public SpriteAnimationPlayerSettings(bool reversed = false, bool finishInstantly = false, bool setDefaultSpriteForMissingParts = false, [CanBeNull] Sprite defaultSprite = null) {
            Reversed = reversed;
            FinishInstantly = finishInstantly;
            SetDefaultSpriteForMissingParts = setDefaultSpriteForMissingParts;
            DefaultSprite = defaultSprite;
        }
    }
}