using UnityEngine;
using UnityEngine.UI;


#if UNITY_EDITOR
#endif

namespace IGIJam.OrderInDisorder.ItemSystem {
    public class ItemEmote : MonoBehaviour {
        [SerializeField] private Item _item;
        [SerializeField] private Image _happyIcon;
        [SerializeField] private Image _sadIcon;

        private void OnEnable() {
            _item.ItemStateChangedEvent += OnItemStateChanged;
        }

        private void OnDestroy() {
            _item.ItemStateChangedEvent -= OnItemStateChanged;
        }

        private void Start() {
            UpdateIcon();
        }

        private void UpdateIcon() {
            switch (_item.State) {
                case ItemState.Happy:
                    _happyIcon.gameObject.SetActive(true);
                    _sadIcon.gameObject.SetActive(false);
                    break;
                case ItemState.Sad:
                    _happyIcon.gameObject.SetActive(false);
                    _sadIcon.gameObject.SetActive(true);
                    break;
                default:
                    _happyIcon.gameObject.SetActive(false);
                    _sadIcon.gameObject.SetActive(false);
                    break;
            }
        }

        private void OnItemStateChanged() {
            UpdateIcon();
        }
    }
}