using UnityEngine;

namespace IGIJam.OrderInDisorder.ItemSystem {
    [CreateAssetMenu(menuName = "IGIJam/Data/Item", order = 0)]
    public class ItemData : ScriptableObject {
        public string ID;
        public string Name;
        public string Description;
        public GameObject Prefab;

        public Item GetInstance(Transform parent) {
            GameObject instantiated = Instantiate(Prefab, parent);
            Item item = instantiated.GetComponent<Item>();

            item.SetData(this);
            return item;
        }
    }
}