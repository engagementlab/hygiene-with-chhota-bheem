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

    private void OnEnable()
    {

        _bubbles = GameObject.FindGameObjectsWithTag("GUIBubble");
        _bubbleStars = GameObject.FindGameObjectsWithTag("GUIStar");

        _boardContainer = transform.Find("Wrapper/Board").gameObject;
        _headerContainer = transform.Find("Wrapper/Header").gameObject;
        _buttonsContainer = transform.Find("Wrapper/Buttons").gameObject;
        _lowerButtons = _buttonsContainer.transform.GetComponentsInChildren<Transform>().Skip(0).ToArray();
        
        // Spell step objects
        /*_spellStepsParent = transform.Find("Wrapper/SpellSteps/Chapter1").gameObject;
        _stepsBgImages = new []{ _spellStepsParent.transform.Find("BG1"), _spellStepsParent.transform.Find("BG2") };
        _stepGroups = new []{ _spellStepsParent.transform.Find("Steps/Group1"), _spellStepsParent.transform.Find("Steps/Group2") };
        _stepsGroup1 = _stepGroups[0].GetComponentsInChildren<Image>();
        _stepsGroup2 = _stepGroups[1].GetComponentsInChildren<Image>();*/

        _scoreText = _boardContainer.transform.Find("ScoreWrap/Text").GetComponent<Text>();
        _villagers = _boardContainer.transform.Find("VillagersMultiplier/Text").GetComponent<Text>();
        _scoreText.gameObject.SetActive(false);
        _villagers.gameObject.SetActive(false);

        _gameOverImg = _headerContainer.transform.Find("GameOver").gameObject;
        _superImg = _headerContainer.transform.Find("Super!").gameObject;

        _stars = transform.Find("Wrapper/Board/Wrapper").GetComponentsInChildren<Image>().Skip(0).ToArray();

//        StartCoroutine(AnimateSteps());
        StartCoroutine(AnimateBubbles());
        
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
        
        for(var b = 0; b < _bubbles.Length; b++)
            iTween.ScaleTo(_bubbles[b], iTween.Hash("scale", Vector3.one, "time", 1, "easetype", iTween.EaseType.easeOutElastic, "delay", Random.Range(.3f, .7f) * b));

        yield return new WaitForSeconds(1.3f);
        for(var s = 0; s < _bubbleStars.Length; s++)
        {
            var delay = Random.Range(.3f, .7f) * s;
            iTween.ScaleTo(_bubbleStars[s], iTween.Hash("scale", Vector3.one, "time", 1, "easetype", iTween.EaseType.easeOutElastic, "delay", delay));
            iTween.PunchRotation(_bubbleStars[0], iTween.Hash("z", 90, "time", 1.2f, "delay", delay));
        }

    }
    
    private IEnumerator AnimateSteps()
    {
        iTween.MoveTo(_stepsBgImages[0].gameObject, new Vector3(-700, -101, 0), .001f);
        iTween.MoveTo(_stepsBgImages[1].gameObject, new Vector3(700, -101, 0), .001f);

        for(var i1 = 0; i1 < _stepsGroup1.Length; i1++)
            iTween.ScaleTo(_stepsGroup1[i1].gameObject, Vector3.zero, .001f);
        for(var i2 = 0; i2 < _stepsGroup2.Length; i2++)
            iTween.ScaleTo(_stepsGroup2[i2].gameObject, Vector3.zero, .001f);
        
        yield return new WaitForSeconds(2);
        
        iTween.MoveTo(_stepsBgImages[0].gameObject, iTween.Hash("position", new Vector3(0, -101, 0), "time", 2.5f, "islocal", true, "easetype", iTween.EaseType.easeOutBack));
        iTween.MoveTo(_stepsBgImages[1].gameObject, iTween.Hash("position", new Vector3(0, -101, 0), "time", 2.5f, "islocal", true, "easetype", iTween.EaseType.easeOutBack));

        for(var i1 = 0; i1 < _stepsGroup1.Length; i1++)
            iTween.ScaleTo(_stepsGroup1[i1].gameObject, iTween.Hash("scale", Vector3.one, "time", 1, "easetype", iTween.EaseType.easeOutElastic, "delay", .8f * i1));
        
        yield return new WaitForSeconds(_stepsGroup1.Length);

        iTween.MoveTo(_stepGroups[0].gameObject, iTween.Hash("position", new Vector3(-12, 50, 0), "time", .7f, "islocal", true, "easetype", iTween.EaseType.easeInExpo));
        iTween.ValueTo(gameObject, iTween.Hash("from", 1, "to", 0, "time", .7f, "onupdate", "FadeSpellsOut"));
        
        for(var i2 = 0; i2 < _stepsGroup2.Length; i2++)
            iTween.ScaleTo(_stepsGroup2[i2].gameObject, iTween.Hash("scale", Vector3.one, "time", 1, "easetype", iTween.EaseType.easeOutElastic, "delay", .8f * i2));
        
    }

    private void FadeSpellsOut(float alpha)
    {
        _stepGroups[0].gameObject.GetComponent<CanvasGroup>().alpha = alpha;
    }
    
    private IEnumerator ScoreCounter(float score, int start, Text text)
    {
        DefaultNamespace.Counter.Reset((int) score, start, text);
        DefaultNamespace.Counter.Update();

        yield return new WaitForSeconds(3);
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