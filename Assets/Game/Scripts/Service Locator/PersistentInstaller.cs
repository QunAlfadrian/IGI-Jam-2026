using IGIJam.OrderInDisorder.LevelSystem;
using UnityEngine;

namespace IGIJam.OrderInDisorder.ServiceLocator {
    public class PersistentInstaller : PersistentInstallerBase {
        [SerializeField] private int _randomSeed = 2026;
        [SerializeField] private LevelDatabase _levelDatabase;
        public override void RegisterServices() {
            UnityEngine.Random.InitState(_randomSeed);

            _levelDatabase.Populate();
            Services.Register(_levelDatabase);
        }
    }
}