#if UNITY_EDITOR
#endif

namespace IGIJam.OrderInDisorder.ItemSystem {
    public struct ShowItemInfoEvent {
        public Item Item { get; private set; }

        public ShowItemInfoEvent(Item item) {
            Item = item;
        }
    }
}