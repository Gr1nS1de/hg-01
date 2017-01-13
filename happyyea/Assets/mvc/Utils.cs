using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Utility class.
/// </summary>
public static class Utils
{
	private const string LastScoreKey = "LAST_SCORE";
	private const string BestScoreKey = "BEST_SCORE";

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
