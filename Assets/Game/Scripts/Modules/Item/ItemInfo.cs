using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace IGIJam.OrderInDisorder.ItemSystem {
    public class ItemInfo : MonoBehaviour, IInitializable, IDisposable {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Vector2 _offset;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TextMeshProUGUI _name_tmp;
        [SerializeField] private TextMeshProUGUI _trait_tmp;
        [SerializeField] private TextMeshProUGUI _hint_tmp;
        private Item _item;
        private RectTransform _rectTransform;

        private void Awake() {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Start() {
            GameContext.SceneServices.Register(this);
        }

        public void Initialize() {
            GameContext.SceneEvents.Subscribe<ShowItemInfoEvent>(OnShowItemInfo);
            GameContext.SceneEvents.Subscribe<HideItemInfoEvent>(OnHideItemInfo);
        }

        public void Dispose() {
            GameContext.SceneEvents.Unsubscribe<ShowItemInfoEvent>(OnShowItemInfo);
            GameContext.SceneEvents.Unsubscribe<HideItemInfoEvent>(OnHideItemInfo);
        }

        private void Update() {
            Vector2 screenPosition = Mouse.current.position.ReadValue();

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvas.transform as RectTransform,
                screenPosition,
                _canvas.worldCamera,
                out Vector2 localPoint
            );

            _rectTransform.localPosition = localPoint + _offset;
        }

        private void UpdateInfo() {
            string traits = "";

            traits += _item.Data.Archetype.Name;
            for (int i = 0; i < _item.Data.AttributeArray.Length; i++) {
                traits += ", ";
                traits += _item.Data.AttributeArray[i].Name;
            }

            _name_tmp.text = _item.Data.Name;
            _trait_tmp.text = traits;
            _hint_tmp.text = _item.GetRandomHint();
        }


        private void OnHideItemInfo(HideItemInfoEvent evt) {
            _canvasGroup.alpha = 0;
        }

        private void OnShowItemInfo(ShowItemInfoEvent evt) {
            _item = evt.Item;

            UpdateInfo();
            _canvasGroup.alpha = 1;
        }
    }
}