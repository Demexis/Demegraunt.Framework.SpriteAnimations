using System.Collections.Generic;
using UnityEngine;

namespace Demegraunt.Framework {
    public interface ISpriteAnimation {
        List<Sprite> FrameSprites { get; }
        string AnimationName { get; }
        bool Loop { get; }
        float Speed { get; }
    }
}