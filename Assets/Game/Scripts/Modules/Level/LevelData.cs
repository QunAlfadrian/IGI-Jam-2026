using IGIJam.OrderInDisorder.ItemSystem;
using UnityEngine;

namespace IGIJam.OrderInDisorder.LevelSystem {
    [CreateAssetMenu(menuName = "IGIJam/Data/Level")]
    public class LevelData : ScriptableObject {
        public string ID;
        public bool Unlocked;
        public ItemData[] ItemArray;
        public string SceneToLoadPath;
    }
}
