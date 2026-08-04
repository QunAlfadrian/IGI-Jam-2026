namespace IGIJam.OrderInDisorder.LevelSystem {
    public struct UnlockLevelEvent {
        public string ID { get; private set; }

        public UnlockLevelEvent(string id) {
            ID = id;
        }
    }
}
