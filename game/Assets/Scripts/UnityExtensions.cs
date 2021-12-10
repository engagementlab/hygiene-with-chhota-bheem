﻿using UnityEngine;

public static class UnityExtensions
{
  public static Vector3 ScreenToWorldLength(this Camera camera, Vector3 position)
  {
    return camera.ScreenToWorldPoint(position) - camera.ScreenToWorldPoint(Vector3.zero);
  }
}