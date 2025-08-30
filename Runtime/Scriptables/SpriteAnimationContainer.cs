using UnityEngine;

namespace Demegraunt.Framework {
    [CreateAssetMenu(fileName = nameof(SpriteAnimationContainer), menuName = "ScriptableObjects/" + nameof(SpriteAnimationContainer), order = 1)]
    public sealed class SpriteAnimationContainer : ScriptableObject {
        [field: SerializeField] public SpriteAnimation SpriteAnimation { get; set; } = new();
    }
}