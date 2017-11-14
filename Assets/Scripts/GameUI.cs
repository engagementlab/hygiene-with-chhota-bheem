using UnityEngine;

public class GameUI : MonoBehaviour
{

    private Camera camera;
    private GameManager game;

    // Use this for initialization
    void Awake()
    {

        camera = Camera.main;
        game = camera.GetComponent<GameManager>();

    }

    // Pause and unpause the game
    public void PauseUnpause(bool pause)
    {
        if(pause)
            game.Pause();
        else
            StartCoroutine(game.UnPause());
    }

    public void HideSlowMo()
    {
        game.HideSlowMo();
    }

    public void LoadLevel(string level)
    {
        GameConfig.SlowMo = false;
        if(level == "next")
        {
            var next = Application.loadedLevel + 1;
            UnityEngine.SceneManagement.SceneManager.LoadScene(next);
        }
        else if(!System.String.IsNullOrEmpty(level))
            UnityEngine.SceneManagement.SceneManager.LoadScene(level);
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene(Application.loadedLevel);


    }
}