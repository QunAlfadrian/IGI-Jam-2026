using IGIJam.OrderInDisorder.GridSystem;
using System.Collections.Generic;
using UnityEngine;

namespace IGIJam.OrderInDisorder.ItemSystem {
    [CreateAssetMenu(menuName = "IGIJam/Item Rule/Corner Position Rule")]
    public class CornerPositionRule : ItemRuleBase {
        public override bool Evaluate(Cell cell, GridSystem.Grid grid, List<Cell> affectedCells) {
            if (!IsCornerPosition(cell, grid)) {
                return false;
            }

            if (affectedCells.Count == 0) {
                return false;
            }

            for (int i = 0; i < affectedCells.Count; i++) {
                if (!affectedCells[i].Occupied) {
                    return false;
                }
            }

            return true;
        }

        public override List<Cell> GetAffectedCells(Cell cell, GridSystem.Grid grid) {
            List<Cell> affectedCellList = new List<Cell>();
            Cell affectedCell = null;
            Vector2Int gridPos = cell.GridPosition;

            foreach (var offset in GridSystem.Grid.Adjacent) {
                affectedCell = grid.GetCell(gridPos.x + offset.x, gridPos.y + offset.y);
                if (affectedCell == null) {
                    continue;
                }
                affectedCellList.Add(affectedCell);
            }

            foreach (var offset in GridSystem.Grid.Diagonal) {
                affectedCell = grid.GetCell(gridPos.x + offset.x, gridPos.y + offset.y);
                if (affectedCell == null) {
                    continue;
                }
                affectedCellList.Add(affectedCell);
            }

            return affectedCellList;
        }

        private bool IsCornerPosition(Cell cell, GridSystem.Grid grid) {

            Vector2Int pos = cell.GridPosition;
            bool atRowEdge = pos.x == 0 || pos.x == grid.Height - 1;
            bool atColumnEdge = pos.y == 0 || pos.y == grid.Width - 1;

            return atRowEdge && atColumnEdge;
        }
    }
}