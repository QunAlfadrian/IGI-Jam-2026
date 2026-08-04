using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace IGIJam.OrderInDisorder.LevelSystem {
    public class LevelManager : MonoBehaviour, IInitializable, IDisposable {
        [SerializeField] private int _currentLevel = 1;
        [SerializeField] private List<LevelHandler> _levelHandlerList;
        [SerializeField] private LevelHandler _current;

        public void Initialize() {
            GameContext.SceneEvents.Subscribe<LevelUpEvent>(OnLevelUp);
        }

        public void Dispose() {
            GameContext.SceneEvents.Unsubscribe<LevelUpEvent>(OnLevelUp);
        }

        private void Start() {
            GameContext.SceneEvents.Publish(new LevelChangedEvent(_current));
            _current = _levelHandlerList[_currentLevel - 1];
        }

        private void OnLevelUp(LevelUpEvent evt) {
            _currentLevel++;

            if (_currentLevel > _levelHandlerList.Count) {
                GameContext.SceneEvents.Publish(new GameOverEvent());
                return;
            }

            _current = _levelHandlerList[_currentLevel - 1];
            Camera.main.transform.DOMoveX(_current.CameraPivot.position.x, 0.5f).SetEase(Ease.OutCubic);

            GameContext.SceneEvents.Publish(new LevelChangedEvent(_current));
        }

        [ContextMenu("Register Level Handler")]
        private void RegisterLevelHandler() {
            _levelHandlerList.Clear();
            LevelHandler[] levelHandlerArray = GameObject.FindObjectsByType<LevelHandler>(FindObjectsSortMode.None);
            foreach (var item in levelHandlerArray) {
                _levelHandlerList.Add(item);
            }
        }
    }
}
