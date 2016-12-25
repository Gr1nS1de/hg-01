using UnityEngine;
using System.Collections;

public class ResourcesController : Controller<Game> 
{
	private PlayerModel 			_playerModel;
	private ObstacleFactoryModel 	_obstacleFactoryModel;
	private RoadFactoryModel 		_roadFactoryModel;

	public override void OnNotification (string alias, Object target, params object[] data)
	{
		switch (alias)
		{
			case N.RCStartLoad:
				{
					OnStartLoad ();
					break;
				}
		}
	}

	private void OnStartLoad()
	{
		_playerModel = game.model.playerModel;
		_obstacleFactoryModel = game.model.obstacleFactoryModel;
		_roadFactoryModel = game.model.roadFactoryModel;

		LoadPlayerSprites();
		LoadObstacleSprites();
		LoadRoads();
	}

	public void LoadPlayerSprites()
	{
		Debug.Log("Add player sprites from resource");

		var obstacleSprites = Resources.LoadAll<Sprite>( _playerModel.spriteResourcesPath );

		_playerModel.sprites = new Sprite[obstacleSprites.Length];

		for ( int j = 0; j < obstacleSprites.Length; j++ )
		{
			_playerModel.sprites[j] = obstacleSprites[j];
		}
	}

	private void LoadObstacleSprites()
	{

	}

	private void LoadRoads()
	{
		var themesPath = Application.dataPath + "/game/sprite/Resources";
		Sprite[] themeSprites = null;
		string[] themeDirs = System.IO.Directory.GetDirectories( themesPath );

		for(int i = 0; i < themeDirs.Length; i++ )
		{
			var themeDir = themeDirs[0].Split(new char[] { '\\' } )[1];

			themeSprites = Resources.LoadAll<Sprite>( themeDir + "/theme" );

			var themeSprite = themeSprites[0];
		}
	}

}
