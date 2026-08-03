#if UNITY_EDITOR
#endif

namespace IGIJam.OrderInDisorder.GridSystem {
    public struct ToggleCellColliderEvent {
        public bool Enabled { get; private set; }

        public ToggleCellColliderEvent(bool enabled) {
            Enabled = enabled;
        }
    }
}