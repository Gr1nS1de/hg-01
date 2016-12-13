using UnityEngine;
using System.Collections.Generic;

public class PlayerController : Controller<Game>
{
	public override void OnNotification( string alias, Object target, params object[] data )
	{
		switch ( alias )
		{
			case N.GameStart:
				{
					OnStart();

					break;
				}
		}
	}

	private void OnStart()
	{

	}

}