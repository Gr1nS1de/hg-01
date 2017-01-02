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
		}
	 }

	private void OnStart()
	{
		game.view.cameraView.OnStart ();
	}
}
