using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splash : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(LoadMenu());

    }

    IEnumerator LoadMenu()
    {
        yield return new WaitForSeconds(5);

        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");

    }

}
