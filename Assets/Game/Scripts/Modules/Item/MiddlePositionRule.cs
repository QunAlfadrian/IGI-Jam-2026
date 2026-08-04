using IGIJam.OrderInDisorder.GridSystem;
using System.Collections.Generic;
using UnityEngine;

namespace IGIJam.OrderInDisorder.ItemSystem {
    [CreateAssetMenu(menuName = "IGIJam/Item Rule/Middle Position Rule")]
    public class MiddlePositionRule : ItemRuleBase {
        public override bool Evaluate(Cell cell, GridSystem.Grid grid, List<Cell> affectedCells) {
            if (affectedCells.Count < 2) {
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
                if (offset.x != 0) {
                    continue;
                }

                affectedCell = grid.GetCell(gridPos.x + offset.x, gridPos.y + offset.y);
                if (affectedCell == null) {
                    continue;
                }
                affectedCellList.Add(affectedCell);
            }

            return affectedCellList;
        }

        public override string Hint() {
            return $"I like to be surrounded by my friends, so I need to be in the middle of them.";
        }
    }
}