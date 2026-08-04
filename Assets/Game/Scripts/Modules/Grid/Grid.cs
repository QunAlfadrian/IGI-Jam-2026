using System;
using System.Collections.Generic;
using UnityEngine;

namespace IGIJam.OrderInDisorder.GridSystem {
    public class Grid : MonoBehaviour, IInitializable, IDisposable {
        [SerializeField] private List<Cell> _cellList;
        [SerializeField] private int _width;
        [SerializeField] private int _height;

        public List<Cell> CellList => _cellList;
        public int Width => _width;
        public int Height => _height;
        public int Count { get; private set; }

        private void Awake() {
            Count = Width * Height;
        }

        private void Start() {
            // initialize cell in cell list
            for (int i = 0; i < _cellList.Count; i++) {
                _cellList[i].SetGrid(this, i);
            }
        }

        public void Initialize() {
            GameContext.SceneEvents.Subscribe<ToggleCellColliderEvent>(OnToggleCellCollider);
        }

        public void Dispose() {
            GameContext.SceneEvents.Unsubscribe<ToggleCellColliderEvent>(OnToggleCellCollider);
        }

        private void OnToggleCellCollider(ToggleCellColliderEvent evt) {
            for (int i = 0; i < _cellList.Count; i++) {
                _cellList[i].ToggleCollider(evt.Enabled);
            }
        }

        public int ToIndex(int row, int col) {
            return row * Width + col;
        }

        public Vector2Int ToRowCol(int index) {
            int row = index / Width;
            int col = index % Width;
            return new Vector2Int(row, col);
        }

        public Cell GetCell(int row, int col) {
            if (row < 0 || row >= Height || col < 0 || col >= Width) {
                return null;
            }

            return _cellList[ToIndex(row, col)];
        }

        public static readonly Vector2Int[] Adjacent = {
            new Vector2Int(0, 1), new Vector2Int(0, -1),
            new Vector2Int(1, 0), new Vector2Int(-1, 0)
        };

        public static readonly Vector2Int[] Diagonal = {
            new Vector2Int(1, 1), new Vector2Int(-1, 1),
            new Vector2Int(1, -1), new Vector2Int(-1, -1)
        };

        public static readonly Vector2Int[] AllDirections = {
            new Vector2Int(0, 1), new Vector2Int(0, -1),
            new Vector2Int(1, 0), new Vector2Int(-1, 0),
            new Vector2Int(1, 1), new Vector2Int(-1, 1),
            new Vector2Int(1, -1), new Vector2Int(-1, -1)
        };
    }
}