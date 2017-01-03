using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Destructible2D;

#if UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif

/// <summary>
/// Class in charge of the logic of the game. This class will restart the level at game over, handle and save the point, and call the Ads if you import the VERY SIMPLE ADS asset available here: http://u3d.as/oWD
/// </summary>
public class GM : MonoBehaviour
{
	public static GM instance;

	public Gradient		backgroundGradient 		{ get { return _backgroundGradient; } }
	public float		backgroundDuration		{ get { return _backgroundDuration; } }
	public Color		currentBackgroundColor 	{ get { return _currentBackgroundColor; }	set { _currentBackgroundColor = value; } } 

	[SerializeField]
	private Gradient 	_backgroundGradient;
	[SerializeField]
	private Color		_currentBackgroundColor;
	[SerializeField]
	private float		_backgroundDuration;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad (this);
		}
		else
		{
			if (instance != this)
				Destroy (this.gameObject);
		}
	}

	void Update()
	{
		//cam.backgroundColor = gameManager.m_ThemeDynamicColor;
		float t = Mathf.PingPong( Time.time / _backgroundDuration, 1f );
		currentBackgroundColor = _backgroundGradient.Evaluate( t );
	}
}