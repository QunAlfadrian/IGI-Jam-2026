using IGIJam.OrderInDisorder.GridSystem;
using System.Collections.Generic;
using UnityEngine;

namespace IGIJam.OrderInDisorder.ItemSystem {
    [CreateAssetMenu(menuName = "IGIJam/Item Rule/Edge Position Rule")]
    public class EdgePositionRule : ItemRuleBase {
        public override bool Evaluate(Cell cell, GridSystem.Grid grid, List<Cell> affectedCells) {
            return IsEdgeCell(cell, grid);
        }

        public override List<Cell> GetAffectedCells(Cell cell, GridSystem.Grid grid) {
            List<Cell> affectedCellList = new List<Cell>();
            Vector2Int gridPos = cell.GridPosition;

            foreach (var offset in GridSystem.Grid.AllDirections) {
                Cell affectedCell = grid.GetCell(gridPos.x + offset.x, gridPos.y + offset.y);
                if (affectedCell == null) {
                    continue;
                }
                affectedCellList.Add(affectedCell);
            }

            return affectedCellList;
        }

        public override string Hint() {
            return $"I enjoy watching the edge of the shelf.";
        }

        private bool IsEdgeCell(Cell cell, GridSystem.Grid grid) {
            if (grid.Height == 1 || grid.Width == 1) {
                return true;
            }

            Vector2Int pos = cell.GridPosition;
            bool atRowEdge = pos.x == 0 || pos.x == grid.Height - 1;
            bool atColEdge = pos.y == 0 || pos.y == grid.Width - 1;

            return atRowEdge ^ atColEdge;
        }
    }
}