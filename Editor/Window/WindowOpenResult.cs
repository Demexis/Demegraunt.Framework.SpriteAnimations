namespace Demegraunt.Framework.Editor {
    internal readonly struct WindowOpenResult {
        public bool Success { get; }
        public string Message { get; }

        public WindowOpenResult(bool success, string message) {
            Success = success;
            Message = message;
        }
    }
}