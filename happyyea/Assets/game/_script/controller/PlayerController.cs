using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class PlayerController : Controller
{
	private PlayerModel 	playerModel 		{ get { return game.model.playerModel; } }
	private PlayerView		playerView			{ get { return game.view.playerView; } } 
	private PlayerTraceView	playerTraceView		{ get { return game.view.playerTraceView; } }

	public override void OnNotification( string alias, Object target, params object[] data )
	{
		switch ( alias )
		{
			case N.GameStart:
				{
					OnStart();

					break;
				}

			case N.GameRoadInited:
				{
					PlacePlayerCurrentRoad ();

					break;
				}

			case N.GameChangeRoad:
				{
					var prevRoadAlias = (Road)data [0];

					DOTween.Pause (Tween.PLAYER_CORE_ROTATION);

					playerModel.particleTrace.transform.SetParent (GM.instance.RoadContainer.transform.GetChild((int)prevRoadAlias - 1));
					playerModel.particleTrace.transform.localPosition = new Vector3(0f, 0f, 15f);
					playerModel.particleTrace.Pause ();
					playerModel.particleTrace.simulationSpace = ParticleSystemSimulationSpace.Local;

					break;
				}

			case N.GameRoadChanged:
				{
					DOTween.Play (Tween.PLAYER_CORE_ROTATION);

					playerModel.particleTrace.transform.SetParent (game.view.playerSpriteView.transform);
					playerModel.particleTrace.transform.localPosition = new Vector3(0f, 0f, 15f);
					playerModel.particleTrace.Clear ();
					playerModel.particleTrace.Play ();
					playerModel.particleTrace.simulationSpace = ParticleSystemSimulationSpace.World;

					break;
				}

			case N.InputOnTouchDown:
				{
					PlayerJump ();

					break;
				}

			case N.GameOver:
				{
					OnGameOver ();

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

	private void PlacePlayerCurrentRoad()
	{
		//Sequence sequence = DOTween.Sequence();
		Vector3[] roadWaypoints = game.model.currentRoadWaypoints;
		Debug.Log (roadWaypoints.Length);
		//game.view.playerSpriteContainerView.transform.position = new Vector2(game.model.currentRoadModel.radius, 0f);
		game.view.playerSpriteView.transform.localPosition = new Vector3(0, +playerModel.jumpWidth, 0);

		game.view.playerSpriteContainerView.transform.position = game.model.currentRoadModel.roadTweenPath.transform.position;

		Tweener playerPath = game.view.playerSpriteContainerView.transform.DOPath (roadWaypoints, playerModel.pathDuration, PathType.Linear, PathMode.TopDown2D, 10, Color.green)
			.SetOptions(true)
			.SetLookAt(0.01f);
		playerPath.SetLoops (-1);
		playerPath.SetEase (Ease.Linear);


		playerPath.ForceInit ();

		playerPath.OnWaypointChange ((waypointIndex ) =>
		{
			OnPathWaypointChanged(waypointIndex);
		});

		playerModel.playerPath = playerPath;
		playerModel.positionState = PlayerPositionState.ON_CIRCLE;

		//playerModel.playerPath = game.view.playerSpriteContainerView.transform.DOPath (roadWaypoints, playerModel.speed, PathType.CatmullRom, PathMode.TopDown2D, 10, Color.green);
		//playerView.transform.DORotate(new Vector3(0,0,-360f), playerModel.speed, RotateMode.FastBeyond360).SetId(Tween.PLAYER_CORE_ROTATION).SetEase(Ease.Linear).SetLoops(-1,LoopType.Incremental);

		Notify(N.GamePlayerPlacedOnRoad);

		//sequence.Append(playerPath  );

	}

	private void PlayerJump()
	{ 
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

		if(DOTween.IsTweening(Camera.main))
			return;

		//_gameManager.Add1Point();
		//FindObjectOfType<CameraManager>().DOShake();
	}

	private void OnPathWaypointChanged(int waypointIndex)
	{
		playerModel.playerPathWPIndex = waypointIndex; 
	}

	private void OnGameOver()
	{
		if(game.view.playerSpriteView.GetComponent<Rigidbody2D> ())
			game.view.playerSpriteView.GetComponent<Rigidbody2D> ().isKinematic = false;

		playerModel.particleTrace.Stop ();
	}
		
}