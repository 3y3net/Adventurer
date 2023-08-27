using UnityEngine;
using UnityEngine.SceneManagement;

namespace JohnStairs.RCC.Demo {
    public class DemoGUI : MonoBehaviour {
        private void Awake() {
        }

        public void Update() {
        }

        public void ClickPresetMMO() {
			UnityEngine.SceneManagement.SceneManager.LoadScene("MMO");
        }

        public void ClickPresetARPG() {
			UnityEngine.SceneManagement.SceneManager.LoadScene("ARPG");
        }

        public void ClickPresetIsometric() {
			UnityEngine.SceneManagement.SceneManager.LoadScene("Isometric");
        }

        public void ClickPresetPlayground() {
			UnityEngine.SceneManagement.SceneManager.LoadScene("Playground");
        }
    }
}