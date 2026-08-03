using DG.Tweening;
using IGIJam.OrderInDisorder.GridSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IGIJam.OrderInDisorder.ItemSystem {
    public class Item : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler {
        private Camera _cam;
        private Plane _dragPlane;
        private Vector3 _offset;
        private Vector3 _startingPos;
        private Collider _collider;
        private Tween _moveTween;
        private bool _wasDropped;

        public Cell Cell { get; private set; }

        private void Awake() {
            _cam = Camera.main;
            _collider = GetComponent<Collider>();
        }

        public void SetData(ItemData data) {
            // Set the item data here
        }

        public void Move(Vector3 destination) {
            Debug.Log("Moving to: " + destination);
            _moveTween.Kill();
            _moveTween = transform.DOMove(destination, 0.5f).SetEase(Ease.OutCubic);
        }

        public void Return() {
            Move(_startingPos);

            if (Cell != null) {
                Cell.Occupied = true;
            }
        }

        public void SetCell(Cell cell) {
            Cell = cell;
        }

        public void ConfirmDrop() {
            _wasDropped = true;
        }

        #region Pointer Event Handlers
        public void OnPointerEnter(PointerEventData eventData) {
            transform.DOScale(Vector3.one * 1.15f, 0.25f);
        }

        public void OnPointerExit(PointerEventData eventData) {
            transform.DOScale(Vector3.one * 1f, 0.25f);
        }

        public void OnBeginDrag(PointerEventData eventData) {
            _startingPos = transform.position;
            _dragPlane = new Plane(Vector3.forward, transform.position);
            _offset = transform.position - GetPointOnPlane(eventData);

            _collider.enabled = false;
            _wasDropped = false;

            if (Cell != null) {
                Cell.Occupied = false;
            }

            GameContext.SceneEvents.Publish(new ToggleCellColliderEvent(true));
        }

        public void OnDrag(PointerEventData eventData) {
            transform.position = GetPointOnPlane(eventData) + _offset;
        }

        public void OnEndDrag(PointerEventData eventData) {
            _collider.enabled = true;

            if (!_wasDropped) {
                Return();
            }

            GameContext.SceneEvents.Publish(new ToggleCellColliderEvent(false));
        }

        private Vector3 GetPointOnPlane(PointerEventData eventData) {
            Ray ray = _cam.ScreenPointToRay(eventData.position);
            if (_dragPlane.Raycast(ray, out float distance)) {
                return ray.GetPoint(distance);
            }
            return transform.position;
        }
        #endregion
    }
}