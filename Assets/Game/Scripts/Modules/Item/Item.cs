using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IGIJam.OrderInDisorder.ItemSystem {
    public class Item : MonoBehaviour {
        public void SetData(ItemData data) {
            // Set the item data here
        }

        public void Move(Vector3 destination) {
            transform.DOMove(destination, 0.5f).SetEase(Ease.OutCubic);
        }

        public void OnPointerEnter(PointerEventData eventData) {
            throw new System.NotImplementedException();
        }

        public void OnPointerDown(PointerEventData eventData) {
            throw new System.NotImplementedException();
        }

        public void OnBeginDrag(PointerEventData eventData) {
            throw new System.NotImplementedException();
        }

        public void OnDrag(PointerEventData eventData) {
            throw new System.NotImplementedException();
        }

        public void OnEndDrag(PointerEventData eventData) {
            throw new System.NotImplementedException();
        }
    }
}