using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class PlayerController : Controller
{
	private PlayerModel 	playerModel 		{ get { return game.model.playerModel; } }
	private PlayerView		playerView			{ get { return game.view.playerView; } } 

	public override void OnNotification( string alias, Object target, params object[] data )
	{
		switch ( alias )
		{
			case N.GameStart:
				{
					OnStart();

					break;
				}

			case N.GameRoadsPlaced:
				{
					game.view.playerSpriteContainerView.transform.position = game.model.currentRoadModel.roadTweenPath.transform.position;

					InitPlayerOnCurrentRoad ();

					break;
				}

			case N.GameRoadChangeStart__:
				{
					var prevRoadAlias = (Road)data [0];
					var newRoadAlias = (Road)data [1];
					Vector3 currentRoadPathPoint = (Vector2)System.Array.Find (game.model.roadFactoryModel.roadTemplates, roadView => roadView.GetComponent<RoadModel> ().alias == newRoadAlias).GetComponent<RoadModel> ().roadTweenPath.transform.position;

					currentRoadPathPoint.z = game.view.playerSpriteContainerView.transform.position.z;

					DOTween.Kill (Tween.PLAYER_CONTAINER_MOVE);

					game.view.playerSpriteContainerView.transform.DOMove ( 
						currentRoadPathPoint
						, 0.5f)
						.OnComplete(() => 
						{
							Notify(N.GameRoadChangeEnd);
						});
							
					SetParticleTraceActive(false);

					break;
				}

			case N.GameRoadChangeEnd:
				{
					//Debug.Log ("Road changed to " + game.model.currentRoad.ToString());
					InitPlayerOnCurrentRoad ();
					
					DOTween.Play (Tween.PLAYER_CONTAINER_MOVE);

					SetParticleTraceActive(true);
					break;
				}

			case N.InputOnTouchDown:
				{
					PlayerJump ();

					break;
				}

			case N.GameOver_:
				{
					Vector2 collisionPoint = (Vector2)data [0];

					OnGameOver (collisionPoint);

					break;
				}
		}
	}

	private void OnStart()
	{
		InitPlayer ();
	}

	private void InitPlayer()
	{
		playerModel.currentSprite = playerModel.sprites [0];
	}

	private void InitPlayerOnCurrentRoad()
	{
		//Sequence sequence = DOTween.Sequence();
		Vector3[] roadWaypoints = game.model.currentRoadWaypoints;

		game.view.playerSpriteView.transform.localPosition = new Vector3(0, +playerModel.jumpWidth, 0);

		if(playerModel.playerPath != null && playerModel.playerPath.IsActive ())
			playerModel.playerPath.Kill ();

		//game.view.playerSpriteContainerView.transform.position = game.model.currentRoadModel.roadTweenPath.transform.position;

		Tweener playerPath = game.view.playerSpriteContainerView.transform.DOPath (roadWaypoints, playerModel.pathDuration, PathType.Linear, PathMode.TopDown2D, 10, Color.green)
			.SetOptions (true)
			.SetLookAt (0.01f)
			.SetId(Tween.PLAYER_CONTAINER_MOVE);
		
		playerPath.SetLoops (-1);
		playerPath.SetEase (Ease.Linear);
		playerPath.ForceInit ();

		//playerPath.Goto (0.5f);

		playerPath.OnWaypointChange ((waypointIndex ) =>
		{
			OnPathWaypointChanged(waypointIndex);
		});

		playerModel.playerPath = playerPath;
		playerModel.positionState = PlayerPositionState.OUT_CIRCLE;

		//playerModel.playerPath = game.view.playerSpriteContainerView.transform.DOPath (roadWaypoints, playerModel.speed, PathType.CatmullRom, PathMode.TopDown2D, 10, Color.green);
		//playerView.transform.DORotate(new Vector3(0,0,-360f), playerModel.speed, RotateMode.FastBeyond360).SetId(Tween.PLAYER_CORE_ROTATION).SetEase(Ease.Linear).SetLoops(-1,LoopType.Incremental);
		Debug.Log("Player placed on road");
		Notify(N.GamePlayerPlacedOnRoad);

		//sequence.Append(playerPath  );

	}

	private void PlayerJump()
	{ 
		if (game.model.gameState == GameState.GAMEOVER)
			return;
		
		Notify (N.PlayerJumpStart);

		switch(playerModel.positionState)
		{
			case PlayerPositionState.ON_CIRCLE:
				{
					var v = new Vector3(0, -playerModel.jumpWidth, 0);

					game.view.playerSpriteView.transform.DOLocalMove(v, playerModel.jumpDuration)
						.OnComplete(OnCompleteJump);

					playerModel.positionState = PlayerPositionState.OUT_CIRCLE;

					break;
				}

			case PlayerPositionState.OUT_CIRCLE:
				{
					var v = new Vector3(0, +playerModel.jumpWidth, 0);

					game.view.playerSpriteView.transform.DOLocalMove(v, playerModel.jumpDuration)
						.OnComplete(OnCompleteJump);

					playerModel.positionState = PlayerPositionState.ON_CIRCLE;

					break;
				}
		}
	}

	private void OnCompleteJump()
	{
		Notify (N.PlayerJumpFinish);

	}

	private void OnPathWaypointChanged(int waypointIndex)
	{
		playerModel.playerPathWPIndex = waypointIndex; 
	}

	private void OnGameOver(Vector2 collisionPoint)
	{
		if(game.view.playerSpriteView.GetComponent<Rigidbody2D> ())
			game.view.playerSpriteView.GetComponent<Rigidbody2D> ().isKinematic = false;

		playerModel.playerPath.Kill ();		
		playerModel.particleTrace.transform.SetParent (game.view.playerSpriteContainerView.transform);

		Notify(N.DestructibleBreakEntity___, playerModel.playerDestructible, game.model.destructibleModel.playerFtactureCount, collisionPoint);

	}

	private void SetParticleTraceActive(bool isActive)
	{
		if (isActive)
		{
			playerModel.particleTrace.transform.SetParent (game.view.playerSpriteView.transform);
			playerModel.particleTrace.transform.localPosition = new Vector3(0f, 0f, 15f);
			playerModel.particleTrace.Clear ();
			playerModel.particleTrace.Play ();
			playerModel.particleTrace.simulationSpace = ParticleSystemSimulationSpace.World;
		}
		else
		{
			playerModel.particleTrace.transform.SetParent (GM.instance.RoadContainer.transform.GetChild((int)game.model.prevRoad - 1));
			playerModel.particleTrace.transform.localPosition = new Vector3(0f, 0f, 15f);
			playerModel.particleTrace.Pause ();
			playerModel.particleTrace.simulationSpace = ParticleSystemSimulationSpace.Local;
		}
	}
		
}