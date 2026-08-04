using IGIJam.OrderInDisorder.GridSystem;
using System.Collections.Generic;
using UnityEngine;

namespace IGIJam.OrderInDisorder.ItemSystem {
    [CreateAssetMenu(menuName = "IGIJam/Item Rule/Self Conflict Rule")]
    public class SelfConflictRule : ItemRuleBase {
        public override bool Evaluate(Cell cell, GridSystem.Grid grid, List<Cell> affectedCells) {
            throw new System.NotImplementedException();
        }

        public override List<Cell> GetAffectedCells(Cell cell, GridSystem.Grid grid) {
            throw new System.NotImplementedException();
        }
    }
}