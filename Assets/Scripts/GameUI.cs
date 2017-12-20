using UnityEngine;

public class GameUI : MonoBehaviour
{

    public GameEndUI GameEnd;
    public InterstitialUI Interstitials;
    
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

    public void HideSlowMo(bool gameOver)
    {
        if(gameOver)
            GameConfig.GameOver = true;
    
        game.HideSlowMo();
    }

    public void LoadLevel(string level)
    {
        GameConfig.SlowMo = false;
        if(level == "next")
        {
            if(GameConfig.CurrentLevel == 0)
            {
                
                GameConfig.LevelPlayCount = 1;
                GameConfig.CurrentLevel = 1;
                GameConfig.LoadLevel();
            }
            else
            {
                GameConfig.CurrentLevel = 0;
                if(GameConfig.CurrentChapter < 2)
                    GameConfig.CurrentChapter++;

                Interstitials.PreviousScreen = GameEnd.transform.Find("Wrapper").gameObject;
                Interstitials.OpenLevelInterstitial(GameConfig.CurrentLevel, true);
            }
        }
        else if(!System.String.IsNullOrEmpty(level))
            UnityEngine.SceneManagement.SceneManager.LoadScene(level);
        else
        {
            GameConfig.LevelPlayCount++;
            UnityEngine.SceneManagement.SceneManager.LoadScene(Application.loadedLevel);
        }


    }
}