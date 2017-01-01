using UnityEngine;
using System.Collections.Generic;
using Destructible2D;

public class ResourcesController : Controller
{
	private RCModel 				_RCModel				{ get { return game.model.RCModel; } }
	private PlayerModel 			_playerModel			{ get { return game.model.playerModel; } }
	private ObstacleFactoryModel 	_obstacleFactoryModel 	{ get { return game.model.obstacleFactoryModel; } }
	private RoadFactoryModel 		_roadFactoryModel 		{ get { return game.model.roadFactoryModel; } }

	public override void OnNotification (string alias, Object target, params object[] data)
	{
		switch (alias)
		{
			case N.RCStartLoad:
				{
					var roadAlias = (Road)data [0];

					OnStartLoad (roadAlias);

					break;
				}
		}
	}

	private void OnStartLoad(Road roadAlias)
	{
		LoadPlayerSprites();
		LoadRoad(roadAlias);
	}

	private void UpdatePlayerModel(Sprite[] playerSprites)
	{
		_playerModel.sprites = playerSprites;
	}

	private void UpdateRoadFactoryModel(RoadView roadTemplate)
	{
		_roadFactoryModel.roadTemplate = roadTemplate;
	}

	private void UpdateObstacleFactoryModel(ObstacleView[] obstacleTemplates)
	{
		_obstacleFactoryModel.obstacleTemplates = obstacleTemplates;
	}

	private void LoadPlayerSprites()
	{
		Debug.Log("Add player sprites from resource");

		var playerSprites = Resources.LoadAll<Sprite>( _RCModel.playerSpriteResourcePath );

		UpdatePlayerModel( playerSprites );
	}
		
	public void LoadRoad(Road roadAlias)
	{
		string roadsPrefabPath = _RCModel.roadsPrefabPath;
		RoadView[] roadTemplate = Resources.LoadAll<RoadView>(GetDirFromPath (roadsPrefabPath) + "/" + GetFolderByRoadAlias(roadAlias));

		ObstacleView[] obstacleTemplates = LoadRoadObstacles (roadAlias);
			
		UpdateRoadFactoryModel ( roadTemplate[0] );
		UpdateObstacleFactoryModel ( obstacleTemplates );
		//_obstacleFactoryModel.obstacleTemplates = obstaclesViews;
	}

	private ObstacleView[] LoadRoadObstacles(Road roadAlias)
	{
		List<ObstacleView> roadObstacleTemplates = new List<ObstacleView>();

		foreach (string obstacleStateName in System.Enum.GetNames(typeof(ObstacleState)))
		{
			ObstacleState obstacleState = (ObstacleState)GetObstacleStateValueByName (obstacleStateName);

			ObstacleView obstaclePrefab = LoadObstaclePrefab (roadAlias, obstacleState);
			Sprite[] obstaclesSprites = LoadObstacleSprites (roadAlias, obstacleState);

			foreach (Sprite obstacleSprite in obstaclesSprites)
			{
				ObstacleView obstacle = Instantiate (obstaclePrefab) as ObstacleView;

				switch (obstacle.GetComponent<ObstacleModel>().state)
				{
					case ObstacleState.HARD:
						obstacle.GetComponent<SpriteRenderer> ().sprite = obstacleSprite;
					break;

					case ObstacleState.DESTRUCTIBLE:
						obstacle.GetComponent<D2dDestructible> ().ReplaceWith (obstacleSprite);
					break;
				}

				ObstacleModel obstacleModel = obstacle.GetComponent<ObstacleModel> ();

				obstacleModel.obstacleView = obstacle;
				obstacleModel.spriteSize = obstacleSprite.bounds.size;
				Debug.Log ("Sprite size = " + obstacleModel.spriteSize);
				obstacle.gameObject.SetActive (false);

				roadObstacleTemplates.Add (obstacle);
			}
		}

		return roadObstacleTemplates.ToArray ();
	}

	private ObstacleView LoadObstaclePrefab(Road roadAlias, ObstacleState obstacleState)
	{
		string roadsPrefabPath = _RCModel.roadsPrefabPath;

		ObstacleView[] obstaclesPrefabs = Resources.LoadAll<ObstacleView> (GetDirFromPath (roadsPrefabPath) + "/" + GetFolderByRoadAlias(roadAlias) + "/obstacles/" + System.Enum.GetName(typeof(ObstacleState), obstacleState).ToLower());

		return obstaclesPrefabs[0];
	}

	private Sprite[] LoadObstacleSprites(Road roadAlias, ObstacleState obstacleState)
	{
		string roadsSpritePath = _RCModel.roadsSpritePath;

		Sprite[] obstaclesSprites = Resources.LoadAll<Sprite> (GetDirFromPath (roadsSpritePath) + "/" + GetFolderByRoadAlias(roadAlias) + "/obstacles/" + System.Enum.GetName(typeof(ObstacleState), obstacleState).ToLower());

		return obstaclesSprites;
	}

	private string GetFolderByRoadAlias(Road roadAlias)
	{
		string roadDir;

		roadDir = (int)roadAlias + "_" + System.Enum.GetName (typeof(Road), roadAlias).ToLower();

		Debug.Log ("Return roadDir = " + roadDir);

		return roadDir;

	}

	private int GetObstacleStateValueByName(string stateName)
	{
		foreach (int value in System.Enum.GetValues (typeof(ObstacleState)))
		{
			if (System.Enum.GetName (typeof(ObstacleState), value) == stateName)
				return value;
		}

		return -1;
	}

	private string GetDirFromPath(string path)
	{
		string[] splitedPath = path.Split (new char[] { '/' });

		return splitedPath[splitedPath.Length - 1];
	}
}
