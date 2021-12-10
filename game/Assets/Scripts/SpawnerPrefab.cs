using System;
using UnityEngine;

[Serializable]
public class SpawnerPrefab 
{

  public GameObject Prefab;
  public bool UseSpawnerParent;
  [Range(0, 20)]
  public float DelayBeforeNext;

}