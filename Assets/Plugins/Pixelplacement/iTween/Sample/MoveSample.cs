using UnityEngine;
using System.Collections;

public class MoveSample : MonoBehaviour
{	
	// void Start(){
	// 	iTween.MoveBy(gameObject, iTween.Hash("x", 2, "easeType", "easeInOutExpo", "loopType", "pingPong", "delay", .1));
	// }

	public Transform[] waypointArray;
	public GameObject target;
	float percentsPerSecond = 0.5f; // %2 of the path moved per second
	float currentPathPercent = 0.0f; //min 0, max 1
	   
	void Update () 
	{
	   currentPathPercent += percentsPerSecond * Time.deltaTime;

	   if(currentPathPercent < 1.0f)
		   iTween.PutOnPath(gameObject, waypointArray, currentPathPercent);
	}

	void OnDrawGizmos()
	{
	   //Visual. Not used in movement
	   iTween.DrawPath(waypointArray);
	}

}

