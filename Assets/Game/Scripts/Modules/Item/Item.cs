using DG.Tweening;
using IGIJam.OrderInDisorder.GridSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IGIJam.OrderInDisorder.ItemSystem {
    public class Item : MonoBehaviour, IInitializable, IDisposable, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler {
        public event Action ItemStateChangedEvent;

        private Camera _cam;
        private Plane _dragPlane;
        private Vector3 _offset;
        private Vector3 _startingPos;
        private Collider _collider;
        private Tween _moveTween;
        private bool _wasDropped;
        private List<ItemRuleBase> _ruleList;

        [SerializeField] private ItemData _data;
        [SerializeField] private ItemState _state;

        public ItemData Data => _data;
        public Archetype Archetype => _data.Archetype;
        public Attribute[] AttributeArray => _data.AttributeArray;
        public ItemRuleBase[] ItemRuleArray => _data.ItemRuleArray;
        public ItemState State => _state;
        public Cell Cell { get; private set; }

        private void Awake() {
            _cam = Camera.main;
            _collider = GetComponent<Collider>();
            _data = Data.GetInstance();
        }

        public void Initialize() {
            GameContext.SceneEvents.Subscribe<ToggleItemColliderEvent>(OnToggleItemCollider);
        }

        public void Dispose() {
            GameContext.SceneEvents.Unsubscribe<ToggleItemColliderEvent>(OnToggleItemCollider);
        }

        public void Evaluate(out List<Cell> affectedCells) {
            if (_ruleList == null) {
                CacheRules();
            }

            affectedCells = new List<Cell>();

            List<Cell> affectedCellsByRule = new List<Cell>();
            bool result = true;

            for (int ruleIdx = 0; ruleIdx < _ruleList.Count; ruleIdx++) {
                affectedCellsByRule = _ruleList[ruleIdx].GetAffectedCells(Cell, Cell.Grid);
                for (int i = 0; i < affectedCellsByRule.Count; i++) {
                    if (affectedCells.Contains(affectedCellsByRule[i])) {
                        continue;
                    }

                    affectedCells.Add(affectedCellsByRule[i]);
                }

                if (!result) {
                    continue;
                }

                result = _ruleList[ruleIdx].Evaluate(Cell, Cell.Grid, affectedCellsByRule);
            }

            _state = result ? ItemState.Happy : ItemState.Sad;
            ItemStateChangedEvent?.Invoke();
            Debug.Log($"{Data.Name} is {State.ToString()}");
        }

        public void Evaluate() {
            if (_ruleList == null) {
                CacheRules();
            }

            List<Cell> affectedCellsByRule = new List<Cell>();
            bool result = true;

            for (int ruleIdx = 0; ruleIdx < _ruleList.Count; ruleIdx++) {
                affectedCellsByRule = _ruleList[ruleIdx].GetAffectedCells(Cell, Cell.Grid);

                result = _ruleList[ruleIdx].Evaluate(Cell, Cell.Grid, affectedCellsByRule);
                if (!result) {
                    break;
                }
            }

            _state = result ? ItemState.Happy : ItemState.Sad;
            ItemStateChangedEvent?.Invoke();
            Debug.Log($"{Data.Name} is {State.ToString()}");
        }

        private void CacheRules() {
            _ruleList = new List<ItemRuleBase>();

            for (int i = 0; i < Archetype.RuleArray.Length; i++) {
                if (_ruleList.Contains(Archetype.RuleArray[i])) {
                    continue;
                }

                _ruleList.Add(Archetype.RuleArray[i]);
            }

            for (int i = 0; i < AttributeArray.Length; i++) {
                ItemRuleBase[] attributeRules = AttributeArray[i].RuleArray;
                for (int j = 0; j < attributeRules.Length; j++) {
                    if (_ruleList.Contains(attributeRules[j])) {
                        continue;
                    }

                    _ruleList.Add(attributeRules[j]);
                }
            }

            for (int i = 0; i < ItemRuleArray.Length; i++) {
                if (_ruleList.Contains(ItemRuleArray[i])) {
                    continue;
                }

                _ruleList.Add(ItemRuleArray[i]);
            }
        }

        public void SetData(ItemData data) {
            _data = data;
        }

        public void SetCell(Cell cell) {
            Cell = cell;
        }

        public void ConfirmDrop() {
            _wasDropped = true;
        }

        public void Move(Vector3 destination) {
            _moveTween.Kill();
            _moveTween = transform.DOMove(destination, 0.5f).SetEase(Ease.OutCubic);
        }

        public void Return() {
            Move(_startingPos);

            if (Cell != null) {
                Cell.SetItem(this);
                Cell.EvaluateItem();
            }
        }

        #region Pointer Event Handlers
        public void OnPointerEnter(PointerEventData eventData) {
            transform.DOScale(Vector3.one * 1.15f, 0.25f);
            GameContext.SceneEvents.Publish(new ShowItemInfoEvent(this));
        }

        public void OnPointerExit(PointerEventData eventData) {
            transform.DOScale(Vector3.one * 1f, 0.25f);
            GameContext.SceneEvents.Publish(new HideItemInfoEvent());
        }

        public void OnBeginDrag(PointerEventData eventData) {
            _startingPos = transform.position;
            _dragPlane = new Plane(Vector3.forward, transform.position);
            _offset = transform.position - GetPointOnPlane(eventData);

            _wasDropped = false;

            if (Cell != null) {
                _state = ItemState.Happy;
                ItemStateChangedEvent?.Invoke();
                Cell.SetItem(null);
                Cell.EvaluateAffectedCells();
            }

            GameContext.SceneEvents.Publish(new ToggleCellColliderEvent(true));
            GameContext.SceneEvents.Publish(new ToggleItemColliderEvent(false));
            GameContext.SceneEvents.Publish(new HideItemInfoEvent());
        }

        public void OnDrag(PointerEventData eventData) {
            transform.position = GetPointOnPlane(eventData) + _offset;

            if (Cell != null && eventData.pointerEnter.TryGetComponent<Cell>(out var cell)) {
                if (cell.Occupied) {
                    Cell.SetItem(this);
                } else {
                    Cell.SetItem(null);
                }
            }
        }

        public void OnEndDrag(PointerEventData eventData) {

            if (!_wasDropped) {
                Return();
            }

            GameContext.SceneEvents.Publish(new ToggleCellColliderEvent(false));
            GameContext.SceneEvents.Publish(new ToggleItemColliderEvent(true));
        }

        private Vector3 GetPointOnPlane(PointerEventData eventData) {
            Ray ray = _cam.ScreenPointToRay(eventData.position);
            if (_dragPlane.Raycast(ray, out float distance)) {
                return ray.GetPoint(distance);
            }
            return transform.position;
        }
        #endregion

        #region Event Handlers
        private void OnToggleItemCollider(ToggleItemColliderEvent evt) {
            _collider.enabled = evt.Enabled;
        }
        #endregion

#if UNITY_EDITOR
        private void OnDrawGizmos() {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.yellow;
            style.alignment = TextAnchor.MiddleCenter;
            Handles.Label(transform.position + Vector3.up * 0.25f, State.ToString(), style);
        }
#endif
    }
}