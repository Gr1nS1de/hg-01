using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIGameModel : Model 
{
	public CanvasGroup 	canvasGroupInGame		{ get { return _canvasGroupInGame; } }
	public Text			scoreText				{ get { return _scoreText; } }
	public Slider		scoreSlider				{ get { return _scoreSlider; } }
	public GameObject	itemSpotPrefab			{ get { return _itemSpotPrefab; } }

	[SerializeField]
	private Text		_scoreText;
	[SerializeField]
	private CanvasGroup	_canvasGroupInGame;
	[SerializeField]
	private Slider		_scoreSlider;
	[SerializeField]
	private GameObject	_itemSpotPrefab;
}
