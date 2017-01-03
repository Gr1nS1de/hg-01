using UnityEngine;
using System.Collections;
using Destructible2D;
using DarkTonic.MasterAudio;

public class GameSoundController : Controller
{
	public override void OnNotification( string alias, Object target, params object[] data )
	{
		switch ( alias )
		{
			case N.GameStart:
				{
					break;
				}

			case N.DestructibleBreakEntity:
				{
					var obstacleDestructible = (D2dDestructible)data [0];
					//var fractureCount = (int)data [1];
					//var collisionPoint = (Vector2)data [2];

					MasterAudio.PlaySoundAndForget ("01_break");

					break;
				}
		}

	}

	private void PlayDestructibleSound()
	{
		
	}


}
