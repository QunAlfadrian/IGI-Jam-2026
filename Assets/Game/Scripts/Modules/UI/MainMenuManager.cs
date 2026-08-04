using UnityEngine;
using UnityEngine.SceneManagement;

namespace IGIJam.OrderInDisorder.UI {
    public class MainMenuManager : MonoBehaviour {
        [SerializeField] private string _levelSceneName;

        public void Play() {
            SceneManager.LoadScene(_levelSceneName);
        }

        public void Exit() {
            Application.Quit();
        }
    }
}