using UnityEngine;
using System.Collections;

public class GameView : View<Game>
{
	public CameraView					cameraView					{ get { return _cameralView = SearchLocal<CameraView>(								_cameralView,				"CameraView" ); } }
	public ThemeView					themeView					{ get { return _themeView = SearchLocal<ThemeView>(									_themeView,					"ThemeView" ); } }
	public ObstacleView					obstacleView				{ get { return _obstacleView = SearchLocal<ObstacleView>(							_obstacleView,				"ObstacleView" ); } }
	public DestructibleObstacleView		destructibleObstacleView	{ get { return _destructibleObstacleView = SearchLocal<DestructibleObstacleView>(	_destructibleObstacleView,	"DestructibleObstacleView" ); } }
	public PlayerView					playerView					{ get { return _playerView = SearchLocal<PlayerView>(								_playerView,				"PlayerView" ); } }
	public RotatableComponent			rotatableComponent			{ get { return _rotatableComponent = SearchLocal<RotatableComponent>(				_rotatableComponent,		"RotatableComponent"); } }

	private CameraView					_cameralView;
	private ThemeView					_themeView;
	private ObstacleView				_obstacleView;
	private DestructibleObstacleView	_destructibleObstacleView;
	private PlayerView					_playerView;
	private RotatableComponent          _rotatableComponent;

}
