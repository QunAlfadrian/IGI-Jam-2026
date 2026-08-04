using System;
using UnityEngine;
using UnityEngine.UI;

namespace IGIJam.OrderInDisorder.LevelSystem {
    public class EvaluateLevelButton : MonoBehaviour, IInitializable, IDisposable{
        [SerializeField] private LevelHandler _levelHandler;
        [SerializeField] private Button _button;

        public void Initialize() {
            GameContext.SceneEvents.Subscribe<LevelChangedEvent>(OnLevelChanged);
        }

        public void Dispose() {
            GameContext.SceneEvents.Unsubscribe<LevelChangedEvent>(OnLevelChanged);
        }

        private void Start() {
            GameContext.SceneServices.Register(this);
            _button.onClick.AddListener(OnClick);
        }

        private void OnClick() {
            _levelHandler.CheckCompletion();
        }

        private void OnLevelChanged(LevelChangedEvent evt) {
            Debug.Log("Level Changed");
            _levelHandler = evt.CurrentLevel;
        }
    }
}
