using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameEndUI : MonoBehaviour
{
    public AudioClip StarPopSound;
    public AudioClip VictoryMusic;

    private GameObject _headerContainer;
    private GameObject _boardContainer;
    private GameObject _scoreContainer;
    private GameObject _buttonsContainer;
    private GameObject _finalContainer;
    private Text _scoreText;
    private Text _villagers;
    private Text _endVillagers;

    private GameObject _gameOverImg;
    private GameObject _superImg;
    private GameObject _objToFadeIn;
    private Transform[] _lowerButtons;

    private Image[] _stars;
    
    private GameObject _spellStepsParent;
    private Transform[] _stepGroups;
    private Transform[] _stepsBgImages;
    private Image[] _stepsGroup1;
    private Image[] _stepsGroup2;

    private GameObject[] _bubbles;
    private GameObject[] _bubbleStars;
    
    private float _score;
    private float _totalScore;
    private int _villagerCount;
    private int _totalVillagers;
    private int _goToStar;
    private int _currentStar = 1;
    private float _starAudioPitch = 1;
    private float _animationTime;
    private bool _animateScore;
    private bool _finalLevel;

    private string _sfxFile;

    private void OnEnable()
    {
        
        _sfxFile = "level-";
        _boardContainer = transform.Find("Wrapper/Board").gameObject;
        _headerContainer = transform.Find("Wrapper/Header").gameObject;
        _buttonsContainer = transform.Find("Wrapper/Buttons").gameObject;
        
        _lowerButtons = _buttonsContainer.transform.GetComponentsInChildren<Transform>(true).Skip(1).ToArray();

        foreach (Transform button in _lowerButtons)
        {
            button.localScale = new Vector3(1, 1, 1);
            
            if (!button.gameObject.activeSelf)
                button.gameObject.SetActive(true);
        }

        _finalLevel = GameConfig.CurrentChapter == 2 && GameConfig.CurrentLevel == 1;
        _sfxFile += GameConfig.GameWon ? "won" : "lost";
        
        // "Final" screen objects
        if(GameConfig.GameWon && _finalLevel)
        {
            _finalContainer = transform.Find("Wrapper/Final").gameObject;
            _finalContainer.SetActive(true);
            _buttonsContainer.GetComponent<CanvasGroup>().alpha = 0;
            
        }
        if(_finalLevel)
        {
            _sfxFile += "-end";
            Events.instance.Raise(SoundEvent.WithClip(VictoryMusic, SoundEvent.SoundType.Music, true));
        }

        Events.instance.Raise(new SoundEvent(_sfxFile, SoundEvent.SoundType.SFX));
        
        _bubbles = GameObject.FindGameObjectsWithTag("GUIBubble");
        _bubbleStars = GameObject.FindGameObjectsWithTag("GUIStar");
        
        _gameOverImg = _headerContainer.transform.Find("GameOver").gameObject;
        _superImg = _headerContainer.transform.Find("Super!").gameObject;

        _stars = transform.Find("Wrapper/Board/Wrapper").GetComponentsInChildren<Image>().Skip(0).ToArray();
        
        StartCoroutine(AnimateBubbles());
        LanguageSetup();
        
        iTween.ScaleFrom(_headerContainer, iTween.Hash("scale", Vector3.zero, "time", 1, "easetype", iTween.EaseType.easeOutElastic, "delay", .1f));
        iTween.ScaleFrom(_boardContainer, iTween.Hash("scale", Vector3.zero, "time", 1, "easetype", iTween.EaseType.easeOutElastic, "oncomplete", "ScoreMultiplier", "oncompletetarget", gameObject, "delay", .8f));
        iTween.PunchRotation(_headerContainer, iTween.Hash("z", -90, "time", 2));

        iTween.ScaleFrom(_lowerButtons[0].gameObject, iTween.Hash("scale", Vector3.zero, "time", .6f, "easetype", iTween.EaseType.easeOutElastic, "delay", 1));
        iTween.ScaleFrom(_lowerButtons[1].gameObject, iTween.Hash("scale", Vector3.zero, "time", .6f, "easetype", iTween.EaseType.easeOutElastic, "delay", 1.6f));

        if(GameConfig.CurrentChapter == 2 && GameConfig.CurrentLevel == 1)
            _lowerButtons[2].gameObject.SetActive(false);
        else
            iTween.ScaleFrom(_lowerButtons[2].gameObject, iTween.Hash("scale", Vector3.zero, "time", .6f, "easetype", iTween.EaseType.easeOutElastic, "delay", 2.2f));

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
            
            _scoreText.text = (GameConfig.CurrentLanguage == 0 ? "Score: " : "") + (int)_score;
        }
    }

    public void LoadLevel()
    {
        GameConfig.LoadLevel();
    }

    public void Restart()
    {

        GameConfig.Score = 0;
        GameConfig.Reset();

    }

    public void ScoreCount()
    {

        StartCoroutine(ScoreCounter(_score, 0, _scoreText));

    }

    public void VillagerCount()
    {

        _villagers.gameObject.SetActive(true);
        _objToFadeIn = _villagers.transform.parent.gameObject;

        iTween.MoveFrom(_objToFadeIn, iTween.Hash("position", new Vector3(0, -200, 0), "time", 1, "islocal", true));
        iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", 1, "time", .7f, "onupdate", "FadeTextIn"));

        StarsCount();
        InvokeRepeating("SetVillagersText", 1, .04f);
    }

    public void ScoreMultiplier()
    {
        _scoreText.gameObject.SetActive(true);

        if(GameConfig.GameWon)
            _totalScore = GameConfig.Score * (GameConfig.StarCount() + 1);
        else
            _totalScore = GameConfig.Score;

        _objToFadeIn = _scoreText.transform.parent.gameObject;
        iTween.MoveFrom(_objToFadeIn, iTween.Hash("position", new Vector3(0, -100, 0), "time", 1, "islocal", true));
        iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", 1, "time", .7f, "onupdate", "FadeTextIn"));

        _animateScore = true;
    }

    public void StarsCount()
    {

        _goToStar = GameConfig.StarCount() * 2;
        if(_goToStar == 0) return;

        iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", 1, "time", .7f, "onupdate", "FillInStar", "oncomplete", "OnStarFilled"));

    }

    private void LanguageSetup()
    {
        var isTamil = GameConfig.CurrentLanguage == 1;
        if(isTamil)
        {
            _scoreText = _boardContainer.transform.Find("ScoreWrap/Tamil").GetComponent<Text>();
            _villagers = _boardContainer.transform.Find("VillagersMultiplier/Tamil").GetComponent<Text>();
        } 
        else
        {
            _scoreText = _boardContainer.transform.Find("ScoreWrap/English").GetComponent<Text>();
            _villagers = _boardContainer.transform.Find("VillagersMultiplier/English").GetComponent<Text>();

        }
        
    }
    private IEnumerator AnimateBubbles()
    {
        System.Random rnd = new System.Random();
        _bubbles = _bubbles.OrderBy(x => rnd.Next()).ToArray();
        _bubbleStars = _bubbleStars.OrderBy(x => rnd.Next()).ToArray();

        foreach(GameObject t in _bubbles)
            t.transform.localScale = Vector3.zero;
        foreach(GameObject t in _bubbleStars)
            t.transform.localScale = Vector3.zero;

        if(GameConfig.GameWon)
            yield return new WaitForSeconds(1.3f);
        else
            yield break;

        // Animate Bheem on last level
        if(_finalLevel)
        {
            GameObject chhotaBheem = _finalContainer.transform.Find("ChhotaBheem").gameObject;
            iTween.MoveTo(chhotaBheem, iTween.Hash("position", new Vector3(0, -260.4f, 0), "time", 1.5f, "delay", 2, "islocal", true, "easetype", iTween.EaseType.easeOutExpo));
            iTween.MoveTo(chhotaBheem, iTween.Hash("position", new Vector3(0, -768.42f, 0), "time", 1, "delay", 4, "islocal", true, "easetype", iTween.EaseType.easeInBack));
            iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", 1, "time", .7f, "delay", 6, "onupdate", "FadeButtons"));
        }

        for(var b = 0; b < _bubbles.Length; b++)
            iTween.ScaleTo(_bubbles[b], iTween.Hash("scale", Vector3.one, "time", 1, "easetype", iTween.EaseType.easeOutElastic, "delay", Random.Range(.3f, .5f) * b*.5f));

        yield return new WaitForSeconds(1.3f);
        for(var s = 0; s < _bubbleStars.Length; s++)
        {
            var delay = Random.Range(.3f, .7f) * s;
            iTween.ScaleTo(_bubbleStars[s], iTween.Hash("scale", Vector3.one, "time", 1, "easetype", iTween.EaseType.easeOutElastic, "delay", delay));
            iTween.PunchRotation(_bubbleStars[0], iTween.Hash("z", 90, "time", 1.2f, "delay", delay));
        }

    }

    private void FadeButtons(float alpha)
    {
        _buttonsContainer.GetComponent<CanvasGroup>().alpha = alpha;
    }
    
    private IEnumerator ScoreCounter(float score, int start, Text text)
    {
        DefaultNamespace.Counter.Reset((int) score, start, text);
        DefaultNamespace.Counter.Update();

        yield return new WaitForSeconds(3);
    }

    private void FadeTextIn(float alpha)
    {
        _objToFadeIn.GetComponent<CanvasGroup>().alpha = alpha;
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
        _villagers.text = (GameConfig.CurrentLanguage == 0 ? "Villagers Saved: " : "") + 0 + "/" + GameConfig.Multiplier;

        if (win)
        {
            _gameOverImg.SetActive(false);
        }
        else
        {
            _superImg.SetActive(false);
            _lowerButtons[2].gameObject.SetActive(false);
        }

    }

    private void SetVillagersText()
    {
        if(_villagerCount == _totalVillagers)
            CancelInvoke("SetVillagersText");
        else
            _villagerCount++;

        _villagers.text = (GameConfig.CurrentLanguage == 0 ? "Villagers Saved: " : "") + _villagerCount + "/" + GameConfig.Multiplier;
    }
}