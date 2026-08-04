namespace IGIJam.OrderInDisorder.LevelSystem {
    public struct LevelChangedEvent {
        public LevelHandler CurrentLevel { get; private set; }

        public LevelChangedEvent(LevelHandler levelHandler) {
            CurrentLevel = levelHandler;
        }
    }
}
