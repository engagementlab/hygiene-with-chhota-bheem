using UnityEngine;

namespace DefaultNamespace
{
    public class GameUI : MonoBehaviour
    {

        private Camera camera;
        // Use this for initialization
        void Awake ()
        {

            camera = Camera.main;

        }
        
        // Pause and unpause the game
        public void Pauser(bool pause)
        {
            if (pause)
                StartCoroutine(camera.GetComponent<GameManager>().Pause());
            else 
                StartCoroutine(camera.GetComponent<GameManager>().UnPause());
        }
        
        public void LoadLevel(string level) {

            if (level == "next")
            {
                var next = Application.loadedLevel + 1;
                UnityEngine.SceneManagement.SceneManager.LoadScene(next);
            } else if (!System.String.IsNullOrEmpty(level)) 
                UnityEngine.SceneManagement.SceneManager.LoadScene(level);
            else 
                UnityEngine.SceneManagement.SceneManager.LoadScene(Application.loadedLevel);
    		

        }
    }
}