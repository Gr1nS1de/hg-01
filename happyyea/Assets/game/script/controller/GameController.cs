using UnityEngine;
using System.Collections;

public class GameController : Controller<Game>
{
	public CameraController					cameraController				{ get { return _cameraController = SearchLocal<CameraController>(								_cameraController,					"CameraController" ); } }
	public ThemeController					themeController					{ get { return _themeController = SearchLocal<ThemeController>(									_themeController,					"ThemeController" ) ;  } }
	public ObstacleController				obstacleController				{ get { return _obstacleController = SearchLocal<ObstacleController>(							_obstacleController,				"ObstacleController" ); } }
	public DestructibleObstacleController	destructibleObstacleController	{ get { return _destructibleObstacleController = SearchLocal<DestructibleObstacleController>(	_destructibleObstacleController,	"DestructibleObstacleController" ); } }
	public PlayerController					playerController				{ get { return _playerController = SearchLocal<PlayerController>(								_playerController,					"PlayerController" ); } }
	public SoundController					soundController					{ get { return _soundController = SearchLocal<SoundController>(									_soundController,					"SoundController" ); } }
	public RoadController					roadController					{ get { return _roadController = SearchLocal<RoadController>(									_roadController,					"RoadController" ); } }
	public ResourcesController				resourcesController				{ get { return _resourcesController = SearchLocal<ResourcesController>(							_resourcesController,				"ResourcesController" ); } }


	private CameraController				_cameraController;
	private ThemeController					_themeController;
	private ObstacleController				_obstacleController;
	private DestructibleObstacleController	_destructibleObstacleController;
	private PlayerController				_playerController;
	private SoundController					_soundController;
	private RoadController					_roadController;
	private ResourcesController				_resourcesController;

	public override void OnNotification( string alias, Object target, params object[] data )
	{
		switch ( alias )
		{
			case N.GameStart:
				{
					OnStart();
					break;
				}
		}
	}

	private void OnStart()
	{
	}


}
