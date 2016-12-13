using UnityEngine;
using System.Collections;

public class SoundController : Controller<Game>
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
