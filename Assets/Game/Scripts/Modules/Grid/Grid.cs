using System.Collections.Generic;
using UnityEngine;

namespace IGIJam.OrderInDisorder.GridSystem {
    public class Grid : MonoBehaviour {
        [SerializeField] private List<Cell> _cellList;
        [SerializeField] private int _width;
        [SerializeField] private int _height;

        public List<Cell> CellList => _cellList;
        public int Width => _width;
        public int Height => _height;

        private void Start() {
            // initialize cell in cell list
        }

        private int ToIndex(int row, int col) {
            return row * Width + col;
        }

        public Cell GetCell(int row, int col) {
            return _cellList[ToIndex(row, col)];
        }
    }
}