using System.Collections;
using System.Threading;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameEndUI : MonoBehaviour
    {
        public float Duration = 2f;
        private Animator _gameEndAnim;

        private int score;
        
        public void Awake()
        {
            _gameEndAnim = gameObject.GetComponent<Animator>();

        }

        public void ScoreCount()
        {
            
            StartCoroutine(ScoreTicker(0, GameConfig.Score));
            
            _gameEndAnim.SetTrigger("villagers");

        }

        public void VillagerCount()
        {    
            
            StartCoroutine(ScoreTicker(0, GameConfig.VillagersSaved));
            
            _gameEndAnim.SetTrigger("multiplier");
        }
        
        public void ScoreMultiplier()
        {
            if (GameConfig.VillagersSaved > 0)
                score = GameConfig.Score * GameConfig.VillagersSaved;
            else
                score = GameConfig.Score;
            
            StartCoroutine(ScoreTicker(GameConfig.Score, score));
            
            StarsCount();
        }

        private IEnumerator ScoreTicker(int start, int target)
        {
            int score = start;
            for (float timer = 0; timer < Duration; timer += Time.deltaTime)
            {
                float progress = timer / Duration;
                score = (int)Mathf.Lerp (start, target, progress);
                
            }
            yield return new WaitForSeconds(2);
            score = target;
        }

        public void StarsCount()
        {
            int _stars = GameConfig.StarCount();
		
            _gameEndAnim.SetInteger("stars", _stars);
        }
    }
}