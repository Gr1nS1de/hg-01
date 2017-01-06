using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class UIController : Controller
{
	private UIMenuModel	UIMenuModel	{ get { return ui.model.UIMenuModel; } }
	private UIGameModel UIGameModel	{ get { return ui.model.UIGameModel; } }

	public override void OnNotification (string alias, Object target, params object[] data)
	{
		switch (alias)
		{
			case N.GameStart:
				{
					OnStart ();
					break;
				}

			case N.GameAddScore:
				{
					int score = (int) data[0];

					OnAddScore (score);

					break;
				}
		}
	}

	void OnStart()
	{
		UpdateText();
		UIGameModel.canvasGroupInGame.alpha = 0;
		UIMenuModel.canvasGroupStart.alpha = 1f;
	}

	private void OnAddScore(int score)
	{
		UIGameModel.scoreText.text = game.model.currentScore.ToString();
	}

	void UpdateText()
	{
		UIMenuModel.bestScoreText.text = "BEST " + Utils.GetBestScore().ToString();
		UIMenuModel.lastScoreText.text = "LAST " + Utils.GetLastScore().ToString();
	}

	public void OnStartGame(System.Action complete)
	{
		UpdateText();

		UIGameModel.canvasGroupInGame.DOFade(0,0.01f)
			.SetEase(Ease.Linear);

		UIMenuModel.canvasGroupStart.DOFade(0,0.5f)
			.SetDelay(0.2f)
			.SetEase(Ease.Linear)
			.OnComplete(() => {

				UIMenuModel.canvasGroupStart.alpha = 0;

				UIMenuModel.canvasGroupStart.gameObject.SetActive(false);

				UIGameModel.canvasGroupInGame.DOFade(1f,1).SetEase(Ease.Linear);

				if(complete!=null)
					complete();
			});
	}

	public void OnGameOver(System.Action complete)
	{
		UpdateText();

		UIMenuModel.canvasGroupStart.gameObject.SetActive(true);

		UIGameModel.canvasGroupInGame.DOFade(0,0.2f).SetEase(Ease.Linear);

		UIMenuModel.canvasGroupStart.DOFade(1f,1).SetEase(Ease.Linear).OnComplete(() => {

			UIMenuModel.canvasGroupStart.alpha = 1;

			DOTween.KillAll();

			if(complete!=null)
				complete();
		});
	}

	public void OnClickedStart()
	{

		game.view.cameraView.DOStart(() => {
			//Wait for camera zoom in
			Notify(N.GamePlay);
		});

		OnStartGame(null);

	}
}
