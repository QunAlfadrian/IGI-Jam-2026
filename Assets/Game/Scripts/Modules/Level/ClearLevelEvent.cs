namespace IGIJam.OrderInDisorder.LevelSystem {
    public struct ClearLevelEvent {
        public string ID { get; private set; }

        public ClearLevelEvent(string id) {
            ID = id;
        }
    }
}
