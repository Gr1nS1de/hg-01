using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class PlayerController : Controller
{
	private PlayerModel 	_playerModel 	{ get { return game.model.playerModel;}}
	private PlayerView		_playerView		{ get { return game.view.playerView;}}


	public override void OnNotification( string alias, Object target, params object[] data )
	{
		switch ( alias )
		{
			case N.GameStart:
				{
					OnStart();

					break;
				}

			case N.GameRoadInstantiated:
				{
					InitPlayer ();
					
					break;
				}

			case N.InputOnTouchDown:
				{
					if(game.model.gameState == GameState.PLAYING)
						PlayerJump ();

					break;
				}
		}
	}

	private void OnStart()
	{}

	private void Update()
	{
		//float t = Mathf.PingPong( Time.time / m_CircleSFLightGradientCycleDuration, 1f );
		//m_CircleSFLight.color = m_LightGradient.Evaluate( t );
	}

	private void InitPlayer()
	{
		_playerModel.currentSprite = _playerModel.sprites [0];
		game.view.playerSpriteContainerView.transform.position = new Vector2(game.model.currentRoadModel.radius, 0f);
		game.view.playerSpriteView.transform.localPosition = new Vector3(-_playerModel.jumpWidth, 0, 0);

		_playerModel.positionState = PlayerPositionState.ON_CIRCLE;
		
		_playerView.transform.DORotate(new Vector3(0,0,-360f), _playerModel.speed, RotateMode.FastBeyond360).SetId(Tween.PLAYER_CORE_ROTATION).SetEase(Ease.Linear).SetLoops(-1,LoopType.Incremental);
	}

	private void PlayerJump()
	{ 
		switch(_playerModel.positionState)
		{
			case PlayerPositionState.ON_CIRCLE:
				{
					var v = new Vector3(-_playerModel.jumpWidth, 0, 0);

					game.view.playerSpriteView.transform.DOLocalMove(v, _playerModel.jumpDuration)
						.OnComplete(OnCompleteJump);

					_playerModel.positionState = PlayerPositionState.OUT_CIRCLE;

					break;
				}

			case PlayerPositionState.OUT_CIRCLE:
				{
					var v = new Vector3(+_playerModel.jumpWidth, 0, 0);

					game.view.playerSpriteView.transform.DOLocalMove(v, _playerModel.jumpDuration)
						.OnComplete(OnCompleteJump);

					_playerModel.positionState = PlayerPositionState.ON_CIRCLE;

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
		
}