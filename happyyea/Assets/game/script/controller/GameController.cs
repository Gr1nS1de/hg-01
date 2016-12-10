using UnityEngine;
using System.Collections;

public class GameController : Controller<Game>
{
	public override void OnNotification( string alias, Object target, params object[] data )
	{
		switch ( alias )
		{
			case N.GameStart:
				{
					Debug.Log("Game started");
					break;
				}
		}
	}
}
