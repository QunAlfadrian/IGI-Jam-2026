using IGIJam.OrderInDisorder.GridSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IGIJam.OrderInDisorder.ItemSystem {
    [CreateAssetMenu(menuName = "IGIJam/Item Rule/Area Of Effect Rule", order = 0)]
    public class AreaOfEffectRule : ItemRuleBase {
        public Attribute SourceAttribute;
        public Attribute TargetAttribute;

        public override bool Evaluate(Cell cell, GridSystem.Grid grid, List<Cell> affectedCells) {
            if (TargetAttribute == null) {
                return true;
            }

            bool result = true;
            for (int i = 0; i < affectedCells.Count; i++) {
                if (!affectedCells[i].Occupiable || !affectedCells[i].Occupied) {
                    continue;
                }

                if (affectedCells[i].Item.AttributeArray.Contains(TargetAttribute)) {
                    result = false;
                    break;
                }
            }

            return result;
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

            return affectedCellList;
        }

        public override string Hint() {
            return $"I do not want next to {TargetAttribute.Name}!";
        }
    }
}