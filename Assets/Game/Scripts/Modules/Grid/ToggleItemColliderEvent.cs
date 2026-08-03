#if UNITY_EDITOR
#endif

namespace IGIJam.OrderInDisorder.GridSystem {
    public struct ToggleItemColliderEvent {
        public bool Enabled { get; private set; }

        public ToggleItemColliderEvent(bool enabled) {
            Enabled = enabled;
        }
    }
}