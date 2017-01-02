﻿using UnityEngine;
using System.Collections;

public class N : MonoBehaviour
{
	#region Player notifications
	public const string PlayerJump					= "player.jump";
	public const string PlayerDie					= "player.die";
	#endregion

	#region Obstacle notifications
	public const string ObstacleInvisible			= "obstacle.invisible";
	#endregion

	#region Game notifications
	public const string GameStart					= "game.start";
	public const string GamePlay					= "game.play";
	public const string GamePause					= "game.pause";
	public const string GameOver					= "game.over";

	public const string GamePlayerImpactObstacle	= "game.player.impact.obstacle";

	public const string GameChangeRoad				= "game.road.change";
	public const string GameRoadInstantiated		= "game.road.instantiated";
	#endregion

	#region Destructible notifications
	public const string DestructibleBreakEntity 	= "destructible.break.entity";
	#endregion

	#region Input controller notification
	public const string InputOnTouchDown			= "input.ontouch.down";
	public const string InputOnTouchUp				= "input.ontouch.up";
	#endregion

	#region ResourcesController notifications
	public const string RCStartLoad					= "rc.start.load";
	#endregion
}