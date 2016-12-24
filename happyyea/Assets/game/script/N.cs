using UnityEngine;
using System.Collections;

public class N : MonoBehaviour
{
	#region Player notifications
	public const string PlayerJump			= "player.jump";
	public const string PlayerDie			= "player.die";
	#endregion

	#region Obstacle notifications
	public const string ObstacleBreak		= "obstacle.break";
	#endregion

	#region Game notifications
	public const string RoadChanged			= "game.road.changed";

	//States
	public const string GameStart			= "game.start";
	public const string GamePlay			= "game.play";
	public const string GamePause			= "game.pause";
	public const string GameOver			= "game.over";
	#endregion

	#region ResourcesController notifications
	public const string RCStartLoad			= "rc.start.load";
	#endregion
}
