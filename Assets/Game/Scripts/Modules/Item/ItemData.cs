using UnityEngine;

namespace IGIJam.OrderInDisorder.ItemSystem {
    [CreateAssetMenu(menuName = "IGIJam/Data/Item", order = 0)]
    public class ItemData : ScriptableObject {
        public string ID;
        public string Name;
        public string Description;
        public GameObject Prefab;
        public Archetype Archetype;
        public Attribute[] AttributeArray;
        public ItemRuleBase[] ItemRuleArray;

        public ItemData GetInstance() {
            return Instantiate(this);
        }

        public Item GetItem(Transform parent) {
            GameObject instantiated = Instantiate(Prefab, parent);
            Item item = instantiated.GetComponent<Item>();

            item.SetData(this);
            return item;
        }
    }
}