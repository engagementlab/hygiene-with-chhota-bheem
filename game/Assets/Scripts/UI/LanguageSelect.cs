using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageSelect : MonoBehaviour
{

    public CanvasGroup mainMenu;
    
    // Start is called before the first frame update
    void Start()
    {
        
        #if !UNITY_EDITOR
            if(PlayerPrefs.HasKey("language"))
            {
                gameObject.SetActive(false);
    
                mainMenu.alpha = 1;
                mainMenu.interactable = true;
        
            mainMenu.GetComponent<MainMenu>().Animate();
            }
        #endif

    }
    
    public void ChooseLanguage(int language)
    {
        GameConfig.CurrentLanguage = language;
        PlayerPrefs.SetInt("language", language);
        
        Events.instance.Raise (new LanguageChangeEvent());
        
        gameObject.SetActive(false);

        mainMenu.alpha = 1;
        mainMenu.interactable = true;
        
        mainMenu.GetComponent<MainMenu>().Animate();
    }
}
