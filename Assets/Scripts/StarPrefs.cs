using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class StarPrefs : MonoBehaviour
{
    private string _baseName;
    
    public void SetStars(int stars)
    {
        PlayerPrefs.SetInt(_baseName, stars);
    }

    public void GetStars(string level)
    {
        
    }
}
