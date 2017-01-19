using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CameraController : Controller
{
	public override void OnNotification( string alias, Object target, params object[] data )
	{
		switch ( alias )
		{
			case N.GameStart:
				{
					OnStart ();

					break;
				}

			case N.UIClickedStart:
				{
					game.view.cameraView.DOStart(() => {
						//Wait for camera zoom in
						Notify(N.GamePlay);
					});
					break;
				}

			case N.PlayerJumpFinish:
				{
					//ShakeCamera ();
					break;
				}
		}
	 }

	private void OnStart()
	{
		game.view.cameraView.OnStart ();
	}

	/*
	private void ShakeCamera()
	{
		if(DOTween.IsTweening(Camera.main))
			return;
		
		game.view.cameraView.DOShake ();
		//_gameManager.Add1Point();
		//FindObjectOfType<CameraManager>().DOShake();
	}
*/
}
