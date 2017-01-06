using UnityEngine;
using System.Collections;

public enum GameState
{
	READY,
	PLAYING,
	GAMEOVER
}

public class GameModel : Model
{

	#region Game model
	public GameState					gameState				{ get { return _gameState; } 		set { _gameState 	= value; } }
	public int							currentScore			{ get { return _currentScore; } 	set { _currentScore = value; } }
	public Road							currentRoad				{ get { return _currentRoad;}		set { Notify(N.RCResetRoadModelTemplate, _currentRoad); _currentRoad = value; Notify (N.GameChangeRoad, _prevRoad); } }
	public RoadModel					currentRoadModel		{ get { return _currentRoadModel			= SearchLocal<RoadModel>(					_currentRoadModel,			"RoadModel" ); } }

	[SerializeField]
	private GameState					_gameState 				= GameState.READY;
	[SerializeField]
	private int 						_currentScore;
	[SerializeField]
	private Road						_currentRoad;
	private RoadModel 					_currentRoadModel;	
	private Road 						_prevRoad;
	#endregion

	#region Declare models reference
	public CameraModel					cameraModel				{ get { return _cameraModel 				= SearchLocal<CameraModel>(					_cameraModel,				"CameraModel" ); } }
	public RoadFactoryModel				roadFactoryModel		{ get { return _roadFactoryModel			= SearchLocal<RoadFactoryModel>(			_roadFactoryModel,			"RoadFactoryModel" ); } }
	public ObstacleFactoryModel			obstacleFactoryModel	{ get { return _obstacleFactoryModel 		= SearchLocal<ObstacleFactoryModel>(		_obstacleFactoryModel,		"ObstacleFactoryModel" ); } }
	public DestructibleModel			destructibleModel		{ get { return _destructibleModel 			= SearchLocal<DestructibleModel>( 			_destructibleModel, 		"DestructibleModel" ); } }
	public PlayerModel					playerModel				{ get { return _playerModel 				= SearchLocal<PlayerModel>(					_playerModel,				"PlayerModel" ); } }
	public GameSoundModel				soundModel				{ get { return _soundModel 					= SearchLocal<GameSoundModel>(				_soundModel,				"SoundModel" ); } }
	public RCModel						RCModel					{ get { return _RCModel 					= SearchLocal<RCModel>(						_RCModel,					"RCModel" ); } }

	private CameraModel					_cameraModel;
	private RoadFactoryModel			_roadFactoryModel;
	private DestructibleModel   		_destructibleModel;
	private PlayerModel					_playerModel;
	private GameSoundModel				_soundModel;
	private RCModel						_RCModel;
	private ObstacleFactoryModel		_obstacleFactoryModel;
	#endregion
}
	