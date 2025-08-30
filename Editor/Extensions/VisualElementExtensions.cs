using UnityEngine.UIElements;

namespace Demegraunt.Framework.Editor {
    internal static class VisualElementExtensions {
        public static void SetScaleToFit(this VisualElement element) {
            element.style.backgroundPositionX = new BackgroundPosition(BackgroundPositionKeyword.Center);
            element.style.backgroundPositionY = new BackgroundPosition(BackgroundPositionKeyword.Center);
            element.style.backgroundRepeat = new BackgroundRepeat(Repeat.NoRepeat, Repeat.NoRepeat);
            element.style.backgroundSize = new BackgroundSize(BackgroundSizeType.Contain);
        }
    }
}