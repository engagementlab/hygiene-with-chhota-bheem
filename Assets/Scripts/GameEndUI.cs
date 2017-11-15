using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class GameEndUI : MonoBehaviour
    {
        public float Duration = 2f;
        private Animator _gameEndAnim;
        
        private Text _score;
        private Text _villagers;
        private Text _endScore;
        private Text _endVillagers;
        
        private GameObject _gameOverImg;
        private GameObject _superImg;
        
        private int score;
        
        public void Awake()
        {
            _gameEndAnim = gameObject.GetComponent<Animator>();
            _score = gameObject.transform.Find("Wrapper/Board/ScoreWrap/Score").GetComponent<Text>();
            _villagers = gameObject.transform.Find("Wrapper/Board/VillagersMultiplier/Score").GetComponent<Text>();
            
            _endScore = gameObject.transform.Find("Wrapper/Board/ScoreWrap/Score").GetComponent<Text>();
            _endVillagers = gameObject.transform.Find("Wrapper/Board/VillagersMultiplier/Score").GetComponent<Text>();
            
            _gameOverImg = gameObject.transform.Find("Wrapper/GameOver").gameObject;
            _superImg = gameObject.transform.Find("Wrapper/Super!").gameObject;

        }

        public void Restart()
        {
            GameConfig.Reset();
        }

        public void ScoreCount()
        {
            
            StartCoroutine(ScoreCounter(score, 0, _score));
            
            _gameEndAnim.SetTrigger("villagers");

        }

        public void VillagerCount()
        {    
            
            StartCoroutine(ScoreCounter(score, 0, _score));
            
            _gameEndAnim.SetTrigger("multiplier");
        }

        IEnumerator ScoreCounter(int score, int start, Text text)
        {
            Counter.Reset(score, start, text);
            Counter.Update();
            
            yield return new WaitForSeconds(3);
        }
        
        public void ScoreMultiplier()
        {
            if (GameConfig.GameWon)
                score = GameConfig.Score * (GameConfig.StarCount() + 1);
            else
                score = GameConfig.Score;

            StartCoroutine(ScoreCounter(score, GameConfig.Score, _score));
            
            StarsCount();
        }

        public void StarsCount()
        {
            int stars = GameConfig.StarCount();
		
            _gameEndAnim.SetInteger("stars", stars);
        }

        public void SetContent(bool win)
        {
            _endScore.text = GameConfig.Score.ToString();
            _endVillagers.text = GameConfig.VillagersSaved.ToString();
            
            if(win)
                _gameOverImg.SetActive(false);
            else
                _superImg.SetActive(false);
            
            
        }
    }
}