using IGIJam.OrderInDisorder.ItemSystem;
using UnityEngine;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IGIJam.OrderInDisorder.GridSystem {
    public class Cell : MonoBehaviour {
        [SerializeField] private Grid _grid;
        [SerializeField] private Transform _cellWorldPoint;
        [SerializeField] private Vector2Int _gridPosition;
        [SerializeField] private int _gridIndex;
        [SerializeField] private bool _occupiable;
        [SerializeField] private Item _item;

        public bool Occupied;
        public Grid Grid => _grid;
        public Vector2Int GridPosition => _gridPosition;
        public int GridIndex => _gridIndex;
        public bool Occupiable => _occupiable;

        public void SetGrid(Grid grid, int index) {
            _grid = grid;
            _gridIndex = index;
            _gridPosition = Grid.ToRowCol(GridIndex);
        }

        public void SetItem(Item item) {
            _item = item;
            _item.Move(_cellWorldPoint.position);
            Occupied = true;
        }

        public void Swap(Cell other) {
            // swap the item in this cell with item in other
        }

        public void OnPointerEnter(PointerEventData eventData) {
            throw new System.NotImplementedException();
        }

        public void OnPointerExit(PointerEventData eventData) {
            throw new System.NotImplementedException();
        }

        public void OnDrop(PointerEventData eventData) {
            if (!Occupiable || Occupied) {
                return;
            }
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