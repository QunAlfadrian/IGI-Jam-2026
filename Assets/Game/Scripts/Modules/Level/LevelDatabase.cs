using System;
using System.Collections.Generic;
using UnityEngine;

namespace IGIJam.OrderInDisorder.LevelSystem {
    [Serializable]
    public class LevelDatabase : IInitializable, IDisposable {
        [SerializeField] private string _dataPath = "Data/Levels";
        [SerializeField] private List<LevelData> _levelDataList;
        [SerializeField] private List<Level> _levelList;

        public void Initialize() {
            GlobalEventBus.Subscribe<ClearLevelEvent>(OnClearLevel);
        }

        public void Dispose() {
            GlobalEventBus.Unsubscribe<ClearLevelEvent>(OnClearLevel);
        }

        public void Populate() {
            _levelDataList = new List<LevelData>();
            _levelList = new List<Level>();

            LevelData[] loadedLevelData = Resources.LoadAll<LevelData>(_dataPath);
            for (int i = 0; i < loadedLevelData.Length; i++) {
                _levelDataList.Add(loadedLevelData[i]);
                _levelList.Add(new Level(loadedLevelData[i]));
            }
        }

        public Level GetLevel(string ID) {
            return _levelList.Find(q => q.ID == ID);
        }

        public List<Level> GetAllLevel() {
            return _levelList;
        }

        public void Load() {

        }

        public void Save() {

        }

        private void OnClearLevel(ClearLevelEvent evt) {
            Level level = _levelList.Find(q => q.ID == evt.ID);
            int nextLevel = _levelList.IndexOf(level) + 1;

            if (nextLevel >= _levelList.Count) {
                return;
            }

            _levelList[nextLevel].Unlock();

            Save();
        }
    }
}
