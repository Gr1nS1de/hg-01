using UnityEngine;
using System.Collections;

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
		}
	 }

	private void OnStart()
	{
		game.view.cameraView.OnStart ();
	}
}
