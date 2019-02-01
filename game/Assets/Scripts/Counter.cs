using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class Counter : MonoBehaviour
    {
        public static int score;
        public static int start;
        public static Text text;
        
        // Use this for initialization
        private void Awake () {
		
            DontDestroyOnLoad(this);
		
        }

        public static void Reset(int num, int startNum, Text scoreText)
        {
            score = num;
            start = startNum;
            text = scoreText;
        }
        
        public static void Update()
        {
            if(int.Parse(text.text) < score){
                text.text = start.ToString();
                start = start++;
            }
        }
    }
}