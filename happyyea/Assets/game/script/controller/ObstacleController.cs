using UnityEngine;
using System.Collections;

public class ObstacleController : Controller<Game>
{
	public override void OnNotification( string alias, Object target, params object[] data )
	{
		switch ( alias )
		{
			case N.GameStart:
				{


					break;
				}
		}
	}

}
