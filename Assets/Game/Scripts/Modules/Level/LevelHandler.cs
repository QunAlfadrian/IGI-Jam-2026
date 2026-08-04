using IGIJam.OrderInDisorder.GridSystem;
using IGIJam.OrderInDisorder.ItemSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace IGIJam.OrderInDisorder.LevelSystem {
    public class LevelHandler : MonoBehaviour, IInitializable, IDisposable {
        [SerializeField] private GridSystem.Grid _grid;
        [SerializeField] private LevelData _levelData;
        private LevelDatabase _database;
        private Level _level;

        public Transform CameraPivot;
        public string ID => _levelData.ID;

        public void Initialize() {
            GameContext.SceneEvents.Subscribe<PuzzleCompletedEvent>(OnPuzzleCompleted);
        }

        public void Dispose() {
            GameContext.SceneEvents.Unsubscribe<PuzzleCompletedEvent>(OnPuzzleCompleted);
        }

        private void Start() {
            _database = GameContext.PersistentServices.Get<LevelDatabase>();
            Debug.Log(_database == null);
            _level = _database.GetLevel(ID);

            PlaceItems();
        }

        private void PlaceItems() {
            List<Cell> allCells = _grid.GetAllCell();

            for (int i = 0; i < _levelData.ItemArray.Length; i++) {
                if (!allCells[i].Occupiable) {
                    continue;
                }

                Item item = _levelData.ItemArray[i].GetItem(transform);
                item.transform.position = allCells[i].PivotPosition;
                allCells[i].SetItem(item);
                allCells[i].EvaluateItem();
            }
        }

        private void OnPuzzleCompleted(PuzzleCompletedEvent evt) {
            _level.Clear();
        }

        public void CheckCompletion() {
            List<Cell> allCells = _grid.GetAllCell();

            foreach (var cell in allCells) {
                if (!cell.Item.IsHappy) {
                    return;
                }
            }

            GameContext.SceneEvents.Publish(new LevelUpEvent());
        }
    }
}
