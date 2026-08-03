using IGIJam.OrderInDisorder.GridSystem;
using System.Collections.Generic;
using UnityEngine;

namespace IGIJam.OrderInDisorder.ItemSystem {
    public abstract class ItemRuleBase : ScriptableObject {
        public string ID;
        public string Name;

        public abstract bool Evaluate(Cell cell, GridSystem.Grid grid, List<Cell> affectedCells);

        public abstract List<Cell> GetAffectedCells(Cell cell, GridSystem.Grid grid);
    }
}