﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Utility class.
/// </summary>
public static class Utils
{
	private const string LastScoreKey = "LAST_SCORE";
	private const string BestScoreKey = "BEST_SCORE";

	public static Vector3[] MakeSmoothCurve(Vector3[] arrayToCurve,float smoothness){
		List<Vector3> points;
		List<Vector3> curvedPoints;
		int pointsLength = 0;
		int curvedLength = 0;

		if(smoothness < 1.0f) smoothness = 1.0f;

		pointsLength = arrayToCurve.Length;

		curvedLength = (pointsLength*Mathf.RoundToInt(smoothness))-1;
		curvedPoints = new List<Vector3>(curvedLength);

		float t = 0.0f;
		for(int pointInTimeOnCurve = 0;pointInTimeOnCurve < curvedLength+1;pointInTimeOnCurve++){
			t = Mathf.InverseLerp(0,curvedLength,pointInTimeOnCurve);

			points = new List<Vector3>(arrayToCurve);

			for(int j = pointsLength-1; j > 0; j--){
				for (int i = 0; i < j; i++){
					points[i] = (1-t)*points[i] + t*points[i+1];
				}
			}

			curvedPoints.Add(points[0]);
		}

		return(curvedPoints.ToArray());
	}

	public static void ActivateTransformChildrens(Transform obj, bool isActivate)
	{
		if (!obj)
			Debug.LogError ("Try to activate null Transform");
		
		for(int i = 0; i < obj.transform.childCount; i++)
		{
			if(!obj.transform.GetChild (i).gameObject.activeInHierarchy)
				obj.transform.GetChild (i).gameObject.SetActive(isActivate);
		}
	}

	public static void AddRoadScore(Road road, int score)
	{
		int currentScore = GetRoadScore (road);

		PlayerPrefs.SetInt (road.ToString(), currentScore + score);

		PlayerPrefs.Save();
	}

	public static void SetLastScore(int score)
	{
		PlayerPrefs.SetInt(LastScoreKey,score);

		SetBestScore(score);

		PlayerPrefs.Save();
	}

	public static void SetBestScore(int score)
	{
		int b = GetBestScore();

		if(score > b)
			PlayerPrefs.SetInt(BestScoreKey,score);
	}

	public static int GetRoadScore(Road road)
	{
		return PlayerPrefs.GetInt (road.ToString(), 0);
	}

	public static int GetBestScore()
	{
		return PlayerPrefs.GetInt(BestScoreKey, 0);
	}

	public static int GetLastScore()
	{
		return PlayerPrefs.GetInt(LastScoreKey, 0);
	}
		

}
