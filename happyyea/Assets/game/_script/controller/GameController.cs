using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Destructible2D;

public class GameController : Controller
{
	#region Declare controllers reference
	public CameraController					cameraController				{ get { return _cameraController = SearchLocal<CameraController>(								_cameraController,					"CameraController" ); } }
	public RoadController					roadController					{ get { return _roadController = SearchLocal<RoadController>(									_roadController,					"RoadController" ); } }
	public RoadFactoryController			roadFactoryController			{ get { return _roadFactoryController = SearchLocal<RoadFactoryController>(						_roadFactoryController,				"RoadFactoryController" ); } }
	public ObstacleController				obstacleController				{ get { return _obstacleController = SearchLocal<ObstacleController>(							_obstacleController,				"ObstacleController" ); } }
	public ObstacleFactoryController		obstacleFactoryController		{ get { return _obstacleFactoryController = SearchLocal<ObstacleFactoryController>(				_obstacleFactoryController,			"ObstacleFactoryController" ); } }
	public DestructibleController			destructibleController			{ get { return _destructibleController = SearchLocal<DestructibleController>(					_destructibleController,			"DestructibleController" ); } }
	public PlayerController					playerController				{ get { return _playerController = SearchLocal<PlayerController>(								_playerController,					"PlayerController" ); } }
	public SoundController					soundController					{ get { return _soundController = SearchLocal<SoundController>(									_soundController,					"SoundController" ); } }
	public ResourcesController				resourcesController				{ get { return _resourcesController = SearchLocal<ResourcesController>(							_resourcesController,				"ResourcesController" ); } }

	private CameraController				_cameraController;
	private RoadController					_roadController;
	private RoadFactoryController 			_roadFactoryController;
	private ObstacleController				_obstacleController;
	private ObstacleFactoryController 		_obstacleFactoryController;
	private DestructibleController			_destructibleController;
	private PlayerController				_playerController;
	private SoundController					_soundController;
	private ResourcesController				_resourcesController;
	#endregion

	private PlayerModel 					_playerModel	{ get { return game.model.playerModel;}}

	public override void OnNotification( string alias, Object target, params object[] data )
	{
		switch ( alias )
		{
			case N.GameStart:
				{
					OnStart();
					break;
				}

			case N.GamePlay:
				{
					game.model.gameState = GameState.PLAYING;
					break;
				}


			case N.GamePlayerImpactObstacle:
				{
					var obstacleView = (ObstacleView)data [0];
					var collisionPoint = (Vector2)data [1];

					//Debug.Break ();

					OnImpactObstacleByPlayer (obstacleView, collisionPoint);
					break;
				}

			case N.GameChangeRoad:
				{
					break;
				}

			case N.GameOver:
				{
					var collisionPoint = (Vector2)data [0];

					GameOver (collisionPoint);
					break;
				}
		}
	}

	private void OnStart()
	{
		SetNewGame ();
	}

	void SetNewGame()
	{
		game.model.currentScore = 0;

		//m_PointText.text = _pointScore.ToString();

	}

	public void Add1Point()
	{
		game.model.currentScore++;

		//m_PointText.text = _pointScore.ToString();

		//_soundManager.PlayTouch();

		//FindObjectOfType<Circle>().DOParticle();
	}

	public void OnImpactObstacleByPlayer(ObstacleView obstacleView, Vector2 collisionPoint)
	{
		var obstacleModel = game.model.obstacleFactoryModel.obstacleModelsDictionary[obstacleView];

		if (!obstacleModel)
		{
			Debug.LogError ("Cant find model");
			return;
		}
			
		switch (obstacleModel.state)
		{
			case ObstacleState.HARD:
				{
					//obstacleRenderObject.GetComponent<Rigidbody2D> ().isKinematic = true;

					GameOver (collisionPoint);

					break;
				}

			case ObstacleState.DESTRUCTIBLE:
				{
					var obstacleDestructible = obstacleView.GetComponent<D2dDestructible> ();

					Add1Point ();

					Notify (N.DestructibleBreakEntity, obstacleDestructible, game.model.destructibleModel.destructibleObstacleFractureCount, collisionPoint);

					break;
				}
			default:
				break;
		}
	}

	public void GameOver( Vector2 collisionPoint )
	{
		if (game.model.gameState == GameState.GAMEOVER)
			return;

		//ReportScoreToLeaderboard(point);

		game.model.gameState = GameState.GAMEOVER;

		//_player.DesactivateTouchControl();

		DOTween.KillAll();
		StopAllCoroutines();

		Utils.SetLastScore(game.model.currentScore);

		//ShowAds();

		//_soundManager.PlayFail();

		Notify(N.DestructibleBreakEntity, _playerModel.playerDestructible, game.model.destructibleModel.playerFtactureCount, collisionPoint);

		ui.controller.OnGameOver(() =>
		{
			ReloadScene();
		});
	}

	private void ReloadScene()
	{

		#if UNITY_5_3_OR_NEWER
		SceneManager.LoadSceneAsync( 0, LoadSceneMode.Single );
		#else
		Application.LoadLevel(Application.loadedLevel);
		#endif
	}



}
