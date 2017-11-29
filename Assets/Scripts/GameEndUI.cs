using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameEndUI : MonoBehaviour
{
    public AudioClip StarPopSound;

    private GameObject _headerContainer;
    private GameObject _boardContainer;
    private GameObject _scoreContainer;
    private GameObject _buttonsContainer;
    private Text _scoreText;
    private Text _villagers;
    private Text _endVillagers;

    private GameObject _gameOverImg;
    private GameObject _superImg;
    private GameObject _objToFadeIn;
    private Transform[] _lowerButtons;

    private Image[] _stars;

    private float _score;
    private float _totalScore;
    private int _villagerCount;
    private int _totalVillagers;
    private int _goToStar;
    private int _currentStar = 1;
    private float _starAudioPitch = 1;
    private float _animationTime;
    private bool _animateScore;

    private void OnEnable()
    {

        _boardContainer = transform.Find("Wrapper/Board").gameObject;
        _headerContainer = transform.Find("Wrapper/Header").gameObject;
        _buttonsContainer = transform.Find("Wrapper/Buttons").gameObject;
        _lowerButtons = _buttonsContainer.transform.GetComponentsInChildren<Transform>().Skip(0).ToArray();

        _scoreText = _boardContainer.transform.Find("ScoreWrap/Text").GetComponent<Text>();
        _villagers = _boardContainer.transform.Find("VillagersMultiplier/Text").GetComponent<Text>();
        _scoreText.gameObject.SetActive(false);
        _villagers.gameObject.SetActive(false);

        _gameOverImg = _headerContainer.transform.Find("GameOver").gameObject;
        _superImg = _headerContainer.transform.Find("Super!").gameObject;

        _stars = transform.Find("Wrapper/Board/Wrapper").GetComponentsInChildren<Image>().Skip(0).ToArray();

        iTween.ScaleFrom(_headerContainer, iTween.Hash("scale", Vector3.zero, "time", 1, "easetype", iTween.EaseType.easeOutElastic, "delay", .1f));
        iTween.ScaleFrom(_boardContainer, iTween.Hash("scale", Vector3.zero, "time", 1, "easetype", iTween.EaseType.easeOutElastic, "oncomplete", "ScoreMultiplier", "oncompletetarget", gameObject, "delay", .8f));
        iTween.PunchRotation(_headerContainer, iTween.Hash("z", -90, "time", 2));

        iTween.ScaleFrom(_lowerButtons[1].gameObject, iTween.Hash("scale", Vector3.zero, "time", .6f, "easetype", iTween.EaseType.easeOutElastic, "delay", 1));
        iTween.ScaleFrom(_lowerButtons[2].gameObject, iTween.Hash("scale", Vector3.zero, "time", .6f, "easetype", iTween.EaseType.easeOutElastic, "delay", 1.6f));

        if(GameConfig.CurrentChapter == 2 && GameConfig.CurrentLevel == 1)
            _lowerButtons[3].gameObject.SetActive(false);
        else
            iTween.ScaleFrom(_lowerButtons[3].gameObject, iTween.Hash("scale", Vector3.zero, "time", .6f, "easetype", iTween.EaseType.easeOutElastic, "delay", 2.2f));

    }

    private void Update()
    {
        if(!_animateScore) return;

        if(_score == _totalScore)
        {
            _animateScore = false;
            GameConfig.Score = (int)_totalScore;
            
            CancelInvoke("SetScoreText");
            VillagerCount();
        }
        else
        {
            // Animate to total over 2s
            _animationTime += Time.deltaTime;
            _score = Mathf.Lerp(0, _totalScore, _animationTime/2);
            _scoreText.text = "Score: " + (int)_score;
        }
    }

    public void LoadLevel()
    {
        GameConfig.LoadLevel();
    }

    public void Restart()
    {

        GameConfig.Reset();

    }

    public void ScoreCount()
    {

        StartCoroutine(ScoreCounter(_score, 0, _scoreText));

    }

    public void VillagerCount()
    {

        _villagers.gameObject.SetActive(true);
        _objToFadeIn = _villagers.gameObject;

        iTween.MoveFrom(_objToFadeIn, iTween.Hash("position", new Vector3(0, -33, 0), "time", 1, "islocal", true));
        iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", 1, "time", .7f, "onupdate", "FadeTextIn"));

        StarsCount();
        InvokeRepeating("SetVillagersText", 1, .04f);
    }

    IEnumerator ScoreCounter(float score, int start, Text text)
    {
        DefaultNamespace.Counter.Reset((int) score, start, text);
        DefaultNamespace.Counter.Update();

        yield return new WaitForSeconds(3);
    }

    public void ScoreMultiplier()
    {
        _scoreText.gameObject.SetActive(true);

        if(GameConfig.GameWon)
            _totalScore = GameConfig.Score * (GameConfig.StarCount() + 1);
        else
            _totalScore = GameConfig.Score;

        _objToFadeIn = _scoreText.gameObject;
        iTween.MoveFrom(_objToFadeIn, iTween.Hash("position", new Vector3(0, -23, 0), "time", 1, "islocal", true));
        iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", 1, "time", .7f, "onupdate", "FadeTextIn"));

        _animateScore = true;
    }

    public void StarsCount()
    {

        _goToStar = GameConfig.StarCount() * 2;
        if(_goToStar == 0) return;

        iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", 1, "time", .7f, "onupdate", "FillInStar", "oncomplete", "OnStarFilled"));

    }

    private void FadeTextIn(float alpha)
    {
        _objToFadeIn.GetComponent<CanvasRenderer>().SetAlpha(alpha);
    }

    private void FillInStar(float fillAmt)
    {
        _stars[_currentStar].fillAmount = fillAmt;
    }

    private void OnStarFilled()
    {

        if(_currentStar > 2) _starAudioPitch += .5f;
        Events.instance.Raise(new SoundEvent(null, SoundEvent.SoundType.SFX, StarPopSound, 1, _starAudioPitch));

        iTween.PunchScale(_stars[_currentStar - 1].gameObject, Vector3.one, 1);
        iTween.PunchRotation(_stars[_currentStar - 1].gameObject, iTween.Hash("z", -90, "time", 2));

        if(_currentStar < _goToStar - 1)
        {
            _currentStar += 2;
            iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", 1, "time", .7f, "onupdate", "FillInStar", "oncomplete", "OnStarFilled"));
        }

    }

    public void SetContent(bool win)
    {

        _totalVillagers = GameConfig.VillagersSaved;
        _villagers.text = "Villagers Saved: " + 0 + "/" + GameConfig.Multiplier;

        if (win)
        {
            _gameOverImg.SetActive(false);
        }
        else
        {
            _superImg.SetActive(false);
            _lowerButtons[3].gameObject.SetActive(false);
        }

    }

    private void SetVillagersText()
    {
        if(_villagerCount == _totalVillagers)
            CancelInvoke("SetVillagersText");
        else
            _villagerCount++;

        _villagers.text = "Villagers Saved: " + _villagerCount + "/" + GameConfig.Multiplier;
    }
}