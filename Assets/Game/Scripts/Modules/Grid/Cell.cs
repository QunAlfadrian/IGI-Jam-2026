using IGIJam.OrderInDisorder.ItemSystem;
using UnityEngine;
using UnityEngine.EventSystems;
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
        private Collider _collider;

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
            Debug.Log($"Item {item.name} moved to cell {GridPosition}");
            _item = item;
            _item.Move(PivotPosition);
            _item.SetCell(this);
            Occupied = true;
        }

        public void ToggleCollider(bool enabled) {
            _collider.enabled = enabled;
        }

        public void Swap(Cell other) {
            // swap the item in this cell with item in other
            if (Item == null || other.Item == null) {
                return;
            }

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
        }
#endif
    }
}