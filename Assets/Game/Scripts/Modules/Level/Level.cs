using UnityEngine;

namespace IGIJam.OrderInDisorder.LevelSystem {
    public class Level {
        [SerializeField] private LevelData _data;
        public string ID => _data.ID;
        public bool Unlocked;
        public bool Cleared;
        
        public Level(LevelData data) {
            _data = data;
            Unlocked = data.Unlocked;
        }

        public void SetData(LevelData data) {
            _data = data;
        }

        public void Unlock() {
            Unlocked = true;
            GlobalEventBus.Publish(new UnlockLevelEvent(ID));
        }

        public void Clear() {
            Cleared = true;
            GlobalEventBus.Publish(new ClearLevelEvent(ID));
        }
    }
}
