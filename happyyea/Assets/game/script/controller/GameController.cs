using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Destructible2D;

public class GameController : Controller<Game>
{
	#region Declare controllers reference
	public CameraController					cameraController				{ get { return _cameraController = SearchLocal<CameraController>(								_cameraController,					"CameraController" ); } }
	public RoadController					roadController					{ get { return _roadController = SearchLocal<RoadController>(									_roadController,					"RoadController" ); } }
	public RoadFactoryController			roadFactoryController			{ get { return _roadFactoryController = SearchLocal<RoadFactoryController>(						_roadFactoryController,				"RoadFactoryController" ); } }
	public ObstacleController				obstacleController				{ get { return _obstacleController = SearchLocal<ObstacleController>(							_obstacleController,				"ObstacleController" ); } }
	public ObstacleFactoryController		obstacleFactoryController		{ get { return _obstacleFactoryController = SearchLocal<ObstacleFactoryController>(				_obstacleFactoryController,			"ObstacleFactoryController" ); } }
	public DestructibleObstacleController	destructibleObstacleController	{ get { return _destructibleObstacleController = SearchLocal<DestructibleObstacleController>(	_destructibleObstacleController,	"DestructibleObstacleController" ); } }
	public PlayerController					playerController				{ get { return _playerController = SearchLocal<PlayerController>(								_playerController,					"PlayerController" ); } }
	public PlayerInputController			playerInputController			{ get { return _playerInputController = SearchLocal<PlayerInputController>(						_playerInputController,				"PlayerInputController" ); } }
	public SoundController					soundController					{ get { return _soundController = SearchLocal<SoundController>(									_soundController,					"SoundController" ); } }
	public ResourcesController				resourcesController				{ get { return _resourcesController = SearchLocal<ResourcesController>(							_resourcesController,				"ResourcesController" ); } }

	private CameraController				_cameraController;
	private RoadController					_roadController;
	private RoadFactoryController 			_roadFactoryController;
	private ObstacleController				_obstacleController;
	private ObstacleFactoryController 		_obstacleFactoryController;
	private DestructibleObstacleController	_destructibleObstacleController;
	private PlayerController				_playerController;
	private PlayerInputController			_playerInputController;
	private SoundController					_soundController;
	private ResourcesController				_resourcesController;
	#endregion

	private PlayerModel 					_playerModel;
	private Vector3 						_entityBreakPoint;

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
					break;
				}

			case N.RoadChanged:
				{
					break;
				}

			case N.GameOver:
				{
					break;
				}
		}
	}

	private void OnStart()
	{
		_playerModel = game.model.playerModel;

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

	public void OnImpactObstacleByPlayer(GameObject obstacleEntityObj, Vector2 collisionPoint)
	{
		var obstacleEntity = obstacleEntityObj.GetComponent<ObstacleEntity>();
		var obstacleRenderObject = obstacleEntity.m_ObstacleObject;
		var obstacleDestructible = obstacleRenderObject.GetComponent<D2dDestructible>();

		switch (obstacleEntity.m_State)
		{
			case ObstacleEntity.State.NORMAL:
				obstacleRenderObject.GetComponent<Rigidbody2D>().isKinematic = true;
				GameOver( collisionPoint );
				break;

			case ObstacleEntity.State.DESTRUCTIBLE:
				Add1Point();
				BreakEntity( obstacleDestructible, game.model.destructibleObstacleFractureCount, collisionPoint);
				break;

			default:
				break;
		}
	}

	public void BreakEntity( D2dDestructible destructible, int fractureCount, Vector2 collisionPoint)
	{

		// Store explosion point (used in OnEndSplit)
		if (collisionPoint == Vector2.zero)
			_entityBreakPoint = destructible.transform.position;
		else
			_entityBreakPoint = collisionPoint;

		destructible.transform.tag = "Untagged";

		if( destructible.GetComponentInChildren<D2dCollider>() )
			destructible.GetComponentInChildren<D2dCollider>().m_SpriteChildCollider.tag = "Untagged";

		// Register split event
		destructible.OnEndSplit.AddListener(OnEndSplit);

		// Split via fracture
		D2dQuadFracturer.Fracture(destructible, fractureCount, 0.5f);

		// Unregister split event
		destructible.OnEndSplit.RemoveListener(OnEndSplit);
	}

	private void OnEndSplit(List<D2dDestructible> clones)
	{
		// Go through all clones in the clones list
		for (var i = clones.Count - 1; i >= 0; i--)
		{
			var clone = clones[i];
			var rigidbody = clone.GetComponent<Rigidbody2D>();

			// Does this clone have a Rigidbody2D?
			if (rigidbody != null)
			{
				// Get the local point of the explosion that called this split event
				var localPoint = (Vector2)clone.transform.InverseTransformPoint(_entityBreakPoint);

				// Get the vector between this point and the center of the destructible's current rect
				var vector = clone.AlphaRect.center - localPoint;

				var force = ( game.model.gameState == GameState.GAMEOVER ? _playerModel.breakForce : game.model.destructibleObstacleModel.breakForce );

				// Apply relative force
				rigidbody.AddRelativeForce(vector * force, ForceMode2D.Impulse);
			}
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

		Util.SetLastScore(game.model.currentScore);

		//ShowAds();

		//_soundManager.PlayFail();

		BreakEntity(_playerModel.playerDestructible, game.model.playerFtactureCount, collisionPoint);

		FindObjectOfType<CanvasController>().OnGameOver(() =>
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
