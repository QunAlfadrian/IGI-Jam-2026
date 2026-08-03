using IGIJam.OrderInDisorder.GridSystem;
using UnityEngine;

namespace IGIJam.OrderInDisorder.ItemSystem {
    [CreateAssetMenu(menuName = "IGIJam/Data/Archetype", order = 1)]
    public class Archetype : ItemTrait {
        public override bool Evaluate(Cell cell, GridSystem.Grid grid) {
            return false;
        }
    }
}