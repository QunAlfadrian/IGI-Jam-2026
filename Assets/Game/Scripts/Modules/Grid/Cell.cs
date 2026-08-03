using IGIJam.OrderInDisorder.ItemSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IGIJam.OrderInDisorder.GridSystem {
    public class Cell : MonoBehaviour, IDropHandler {
        [Header("Cell Settings")]
        [SerializeField] private Transform _pivotPoint;
        [SerializeField] private bool _occupiable;
        public bool Occupied;
        [SerializeField] private Item _item;
        [SerializeField] private Collider _collider;
        private List<Cell> _affectedCellList;

        [Header("Grid")]
        [SerializeField] private Grid _grid;
        [SerializeField] private Vector2Int _gridPosition;
        [SerializeField] private int _gridIndex;

        public Grid Grid => _grid;
        public Vector2Int GridPosition => _gridPosition;
        public Vector3 PivotPosition => _pivotPoint.position;
        public int GridIndex => _gridIndex;
        public bool Occupiable => _occupiable;
        public Item Item => _item;

        private void Awake() {
            _collider = GetComponent<Collider>();
        }

        public void SetGrid(Grid grid, int index) {
            _grid = grid;
            _gridIndex = index;
            _gridPosition = Grid.ToRowCol(GridIndex);
        }

        public void SetItem(Item item) {
            _item = item;
            if (_item == null) {
                Occupied = false;
                EvaluateAffectedCells();
                return;
            }

            Debug.Log($"Item {item.name} moved to cell {GridPosition}");

            _item.Move(PivotPosition);
            _item.SetCell(this);
            Occupied = true;

            _item.Evaluate(out _affectedCellList);
            EvaluateAffectedCells();
        }

        public void EvaluateAffectedCells() {
            for (int i = 0; i < _affectedCellList.Count; i++) {
                if (!_affectedCellList[i].Occupiable || !_affectedCellList[i].Occupied) {
                    continue;
                }

                Debug.Log($"Evaluating {_affectedCellList[i].GridPosition}");
                _affectedCellList[i].Item.Evaluate();
            }
        }

        public void ToggleCollider(bool enabled) {
            _collider.enabled = enabled;
        }

        public void Swap(Cell other) {
            Item current = Item;

            SetItem(other.Item);
            other.SetItem(current);
        }

        public void OnDrop(PointerEventData eventData) {
            if (!eventData.pointerDrag.TryGetComponent<Item>(out Item droppedItem)) {
                return;
            }

            droppedItem.ConfirmDrop();
            if (!Occupiable) {
                droppedItem.Return();
                return;
            }

            if (Occupied) {
                Swap(droppedItem.Cell);
                return;
            }

            SetItem(droppedItem);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos() {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.white;
            style.alignment = TextAnchor.MiddleCenter;
            Handles.Label(transform.position, GridPosition.ToString(), style);

            if (_collider != null && _collider is BoxCollider) {
                BoxCollider collider = _collider as BoxCollider;
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(collider.transform.position + collider.center, collider.size);
            }
        }
#endif
    }
}