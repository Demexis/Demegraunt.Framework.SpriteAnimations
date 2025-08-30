using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demegraunt.Framework {
    [Serializable]
    public sealed class SpriteAnimation : ISpriteAnimation {
        [field: SerializeField] public List<Sprite> FrameSprites { get; set; } = new();
        [field: SerializeField] public string AnimationName { get; set; }
        [field: SerializeField] public bool Loop { get; set; }
        [field: SerializeField] public float Speed { get; set; } = 1;
        
        public float GetAnimationLength() {
            if (FrameSprites == null) {
                return 0;
            }
            
            if (Speed.IsApproximately(0)) {
                return float.MaxValue;
            }
                
            return SpritePlayback.ONE_FRAME_TIME * FrameSprites.Count / Speed;
        }
    }
}