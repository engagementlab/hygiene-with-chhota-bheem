using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class GameEndUI : MonoBehaviour
    {
        public float Duration = 2f;

        public AudioClip StarPopSound;
//        private Animator _gameEndAnim;

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
        
        private int score;
        private int totalScore;
        private int villagerCount;
        private int totalVillagers;
        private int goToStar;
        private int currentStar = 1;
        private float starAudioPitch = 1;

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
            iTween.ScaleFrom(_lowerButtons[3].gameObject, iTween.Hash("scale", Vector3.zero, "time", .6f, "easetype", iTween.EaseType.easeOutElastic, "delay", 2.2f));
            
        }

        public void Restart()
        {
            
            GameConfig.Reset();
            
        }

        public void ScoreCount()
        {
            
            StartCoroutine(ScoreCounter(score, 0, _scoreText));

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

        IEnumerator ScoreCounter(int score, int start, Text text)
        {
            Counter.Reset(score, start, text);
            Counter.Update();
            
            yield return new WaitForSeconds(3);
        }
        
        public void ScoreMultiplier()
        {
            _scoreText.gameObject.SetActive(true);
            
            if (GameConfig.GameWon)
                totalScore = GameConfig.Score * (GameConfig.StarCount() + 1);
            else
                totalScore = GameConfig.Score;

            _objToFadeIn = _scoreText.gameObject;
            iTween.MoveFrom(_objToFadeIn, iTween.Hash("position", new Vector3(0, -23, 0), "time", 1, "islocal", true));
            iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", 1, "time", .7f, "onupdate", "FadeTextIn"));

            InvokeRepeating("SetScoreText", 1, .02f);
        }

        public void StarsCount()
        {
            
            goToStar = GameConfig.StarCount()*2;
            if(goToStar == 0) return;
            
            iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", 1, "time", .7f, "onupdate", "FillInStar", "oncomplete", "OnStarFilled"));
            
        }

        private void FadeTextIn(float alpha)
        {
            _objToFadeIn.GetComponent<CanvasRenderer>().SetAlpha(alpha);
        }

        private void FillInStar(float fillAmt)
        {
            _stars[currentStar].fillAmount = fillAmt;
        }

        private void OnStarFilled()
        {
            
            if(currentStar > 2) starAudioPitch += .5f;
            Events.instance.Raise(new SoundEvent(null, SoundEvent.SoundType.SFX, StarPopSound, 1, starAudioPitch));
            
            iTween.PunchScale(_stars[currentStar-1].gameObject, Vector3.one, 1);
            iTween.PunchRotation(_stars[currentStar-1].gameObject, iTween.Hash("z", -90, "time", 2));

            if(currentStar < goToStar-1)
            {            
                currentStar += 2;
                iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", 1, "time", .7f, "onupdate", "FillInStar", "oncomplete", "OnStarFilled"));
            } 
                
        }
        public void SetContent(bool win)
        {
            
            totalVillagers = GameConfig.VillagersSaved;
            _villagers.text = "Villagers Saved: " + 0 + "/" + GameConfig.Multiplier;

            if(win)
                _gameOverImg.SetActive(false);
            else
                _superImg.SetActive(false);
            
        }

        private void SetScoreText()
        {
            if(score == totalScore)
            {
                CancelInvoke("SetScoreText");
                VillagerCount();
            }
            else
                score++;

            _scoreText.text = "Score: " + score;
        }

        private void SetVillagersText()
        {
            if(villagerCount == totalVillagers)
                CancelInvoke("SetVillagersText");
            else
                villagerCount++;
            
            _villagers.text = "Villagers Saved: " + villagerCount + "/" + GameConfig.Multiplier;
        }
    }
}