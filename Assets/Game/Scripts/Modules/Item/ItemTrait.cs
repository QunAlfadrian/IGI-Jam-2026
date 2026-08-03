using IGIJam.OrderInDisorder.GridSystem;
using UnityEngine;

namespace IGIJam.OrderInDisorder.ItemSystem {
    public abstract class ItemTrait : ScriptableObject {
        public string ID;
        public string Name;
        public ItemRuleBase[] RuleList;

        public abstract bool Evaluate(Cell cell, GridSystem.Grid grid);
    }
}