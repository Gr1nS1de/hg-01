using UnityEngine;
using System.Collections;

public class GameModel : Model<Game>
{
	public CameraModel					cameraModel					{ get { return _cameraModel = SearchLocal<CameraModel>(								_cameraModel,				"CameraModel" ); } }
	public ThemeModel					themeModel					{ get { return _themeModel = SearchLocal<ThemeModel>(								_themeModel,				"ThemeModel" ); } }
	public ObstacleModel				obstacleModel				{ get { return _obstacleModel = SearchLocal<ObstacleModel>(							_obstacleModel,				"ObstacleModel" ); } }
	public DestructibleObstacleModel	destructibleObstacleModel	{ get { return _destructibleObstacleModel = SearchLocal<DestructibleObstacleModel>( _destructibleObstacleModel, "DestructibleObstacleModel" ); } }
	public PlayerModel					playerModel					{ get { return _playerModel = SearchLocal<PlayerModel>(								_playerModel,				"PlayerModel" ); } }
	public SoundModel					soundModel					{ get { return _soundModel = SearchLocal<SoundModel>(								_soundModel,				"SoundModel" ); } }

	private CameraModel					_cameraModel;
	private ThemeModel                  _themeModel;
	private ObstacleModel				_obstacleModel;
	private DestructibleObstacleModel   _destructibleObstacleModel;
	private PlayerModel					_playerModel;
	private SoundModel					_soundModel;

}
