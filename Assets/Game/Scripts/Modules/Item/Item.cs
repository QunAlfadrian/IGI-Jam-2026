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

        [SerializeField] private ItemData _data;
        [SerializeField] private ItemState _state;

        public ItemData Data => _data;
        public ItemState State => _state;
        public Cell Cell { get; private set; }

        private void Awake() {
            _cam = Camera.main;
            _collider = GetComponent<Collider>();
            _data = Data.GetInstance();
        }

        public void Evaluate() {
            bool result = false;

            _state = result ? ItemState.Happy : ItemState.Sad;

            Debug.Log($"{Data.Name} is {State.ToString()}");
        }

        public void SetData(ItemData data) {
            _data = data;
        }

        public void SetCell(Cell cell) {
            Cell = cell;
            Evaluate();
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
                Cell.Occupied = true;
            }
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