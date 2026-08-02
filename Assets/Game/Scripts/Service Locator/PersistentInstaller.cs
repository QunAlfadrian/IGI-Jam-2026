using UnityEngine;

namespace IGIJam.OrderInDisorder.ServiceLocator {
    public class PersistentInstaller : PersistentInstallerBase {
        [SerializeField] private int _randomSeed = 2026;

        public override void RegisterServices() {
            UnityEngine.Random.InitState(_randomSeed);
        }
    }
}