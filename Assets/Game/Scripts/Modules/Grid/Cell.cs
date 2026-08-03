using UnityEngine;
using UnityEngine.EventSystems;

namespace IGIJam.OrderInDisorder.GridSystem {
    public class Cell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler {
        [SerializeField] private Grid _grid;
        [SerializeField] private Vector2Int _gridPosition;
        [SerializeField] private int _gridIndex;
        [SerializeField] private bool _occupiable;

        public bool Occupied;
        public bool Occupiable => _occupiable;

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
            throw new System.NotImplementedException();
        }
    }
}