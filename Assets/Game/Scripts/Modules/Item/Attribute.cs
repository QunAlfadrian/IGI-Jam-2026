using IGIJam.OrderInDisorder.GridSystem;
using UnityEngine;

namespace IGIJam.OrderInDisorder.ItemSystem {
    [CreateAssetMenu(menuName = "IGIJam/Data/Attribute", order = 2)]
    public class Attribute : ItemTrait {
        public override bool Evaluate(Cell cell, GridSystem.Grid grid) {
            return false;
        }
    }
}