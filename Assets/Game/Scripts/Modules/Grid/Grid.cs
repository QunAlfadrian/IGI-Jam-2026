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
            // initialize cell in cell list
            for (int i = 0; i < _cellList.Count; i++) {
                _cellList[i].SetGrid(this, i);
            }
        }

        private void Start() {
            GameContext.SceneServices.Register(this);
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

        public List<Cell> GetAllCell() {
            return _cellList;
        }

        public Cell GetCell(int row, int col) {
            if (row < 0 || row >= Height || col < 0 || col >= Width) {
                return null;
            }

            return _cellList[ToIndex(row, col)];
        }

        public Cell GetNearestEmptyCell(Cell origin, bool includeDiagonal = false) {
            if (origin == null) {
                return null;
            }

            return GetNearestEmptyCell(origin.GridPosition, includeDiagonal);
        }

        public Cell GetNearestEmptyCell(Vector2Int origin, bool includeDiagonal = false) {
            Vector2Int[] directions = includeDiagonal ? AllDirections : Adjacent;

            bool[] visited = new bool[Count];
            Queue<Vector2Int> toVisit = new Queue<Vector2Int>();

            visited[ToIndex(origin.x, origin.y)] = true;
            toVisit.Enqueue(origin);

            while (toVisit.Count > 0) {
                Vector2Int current = toVisit.Dequeue();
                Cell currentCell = GetCell(current.x, current.y);

                if (currentCell != null && currentCell.Occupiable && !currentCell.Occupied) {
                    return currentCell;
                }

                for (int i = 0; i < directions.Length; i++) {
                    Vector2Int next = current + directions[i];

                    if (next.x < 0 || next.x >= Height || next.y < 0 || next.y >= Width) {
                        continue;
                    }

                    int nextIndex = ToIndex(next.x, next.y);
                    if (visited[nextIndex]) {
                        continue;
                    }

                    visited[nextIndex] = true;
                    toVisit.Enqueue(next);
                }
            }

            return null;
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