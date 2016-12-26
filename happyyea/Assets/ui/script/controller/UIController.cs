using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class UIController : Controller
{
	private UIModel _UIModel	{ get { return ui.model; } }

	void Awake()
	{
		UpdateText();
	}

	void Start()
	{
		_UIModel.canvasGroupInGame.alpha = 0;
		_UIModel.canvasGroupStart.alpha = 0.8f;
	}

	void UpdateText()
	{
		_UIModel.bestScoreText.text = "BEST " + Util.GetBestScore().ToString();
		_UIModel.lastScoreText.text = "LAST " + Util.GetLastScore().ToString();
	}

	public void OnStartGame(System.Action complete)
	{
		UpdateText();

		_UIModel.canvasGroupInGame.DOFade(0,0.01f)
			.SetEase(Ease.Linear);

		_UIModel.canvasGroupStart.DOFade(0,0.5f)
			.SetDelay(0.2f)
			.SetEase(Ease.Linear)
			.OnComplete(() => {

				_UIModel.canvasGroupStart.alpha = 0;

				_UIModel.canvasGroupStart.gameObject.SetActive(false);

				_UIModel.canvasGroupInGame.DOFade(0.8f,1).SetEase(Ease.Linear);

				if(complete!=null)
					complete();
			});
	}

	public void OnGameOver(System.Action complete)
	{
		UpdateText();

		_UIModel.canvasGroupStart.gameObject.SetActive(true);

		_UIModel.canvasGroupInGame.DOFade(0,0.2f).SetEase(Ease.Linear);

		_UIModel.canvasGroupStart.DOFade(0.8f,1).SetEase(Ease.Linear).OnComplete(() => {

			_UIModel.canvasGroupStart.alpha = 1;

			DOTween.KillAll();

			if(complete!=null)
				complete();
		});
	}

	public void OnClickedStart()
	{
		FindObjectOfType<CameraManager>().DOStart(() => {
			Notify(N.GamePlay);
		});

		OnStartGame(null);

	}
}
