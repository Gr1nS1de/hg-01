using UnityEngine;
using System.Collections;

public enum GameState
{
	READY,
	PLAYING,
	GAMEOVER
}

public class GameModel : Model<Game>
{

	#region Game model
	public GameState					gameState				{ get { return _gameState; } 		set { 			_gameState = value; }}
	public int							currentScore			{ get { return _currentScore; } 	set { 		_currentScore = value; }}
	public RoadModel					currentRoadModel		{ get { return _currentRoadModel;} 	set { 	Notify(N.GameRoadChanged, _currentRoadModel = value); }}


	[SerializeField]
	private GameState					_gameState 				= GameState.READY;
	[SerializeField]
	private int 						_currentScore;
	[SerializeField]
	private RoadModel 					_currentRoadModel;

	#endregion

	#region Declare models reference
	public CameraModel					cameraModel				{ get { return _cameraModel 				= SearchLocal<CameraModel>(					_cameraModel,				"CameraModel" ); } }
//	public RoadModel					roadModel				{ get { return _roadModel 					= SearchLocal<RoadModel>(					_roadModel,					"RoadModel" ); } }
	public RoadFactoryModel				roadFactoryModel		{ get { return _roadFactoryModel			= SearchLocal<RoadFactoryModel>(			_roadFactoryModel,			"RoadFactoryModel" ); } }
	public ObstacleModel				obstacleModel			{ get { return _obstacleModel 				= SearchLocal<ObstacleModel>(				_obstacleModel,				"ObstacleModel" ); } }
	public ObstacleFactoryModel			obstacleFactoryModel	{ get { return _obstacleFactoryModel 		= SearchLocal<ObstacleFactoryModel>(		_obstacleFactoryModel,		"ObstacleFactoryModel" ); } }
	public DestructibleModel			destructibleModel		{ get { return _destructibleModel 			= SearchLocal<DestructibleModel>( 			_destructibleModel, 		"DestructibleModel" ); } }
	public PlayerModel					playerModel				{ get { return _playerModel 				= SearchLocal<PlayerModel>(					_playerModel,				"PlayerModel" ); } }
	public PlayerInputModel				playerInputModel		{ get { return _playerInputModel 			= SearchLocal<PlayerInputModel>(			_playerInputModel,			"PlayerInputModel" ); } }
	public SoundModel					soundModel				{ get { return _soundModel 					= SearchLocal<SoundModel>(					_soundModel,				"SoundModel" ); } }
	public RCModel						RCModel					{ get { return _RCModel 					= SearchLocal<RCModel>(						_RCModel,					"RCModel" ); } }

	private CameraModel					_cameraModel;
//	private RoadModel					_roadModel;
	private RoadFactoryModel			_roadFactoryModel;
	private ObstacleModel				_obstacleModel;
	private DestructibleModel   		_destructibleModel;
	private PlayerModel					_playerModel;
	private PlayerInputModel			_playerInputModel;
	private SoundModel					_soundModel;
	private RCModel						_RCModel;
	private ObstacleFactoryModel		_obstacleFactoryModel;
	#endregion
}
	