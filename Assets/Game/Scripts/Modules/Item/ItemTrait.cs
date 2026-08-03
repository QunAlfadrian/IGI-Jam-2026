using UnityEngine;

namespace IGIJam.OrderInDisorder.ItemSystem {
    public abstract class ItemTrait : ScriptableObject {
        public string ID;
        public string Name;
        public ItemRuleBase[] RuleList;
    }
}