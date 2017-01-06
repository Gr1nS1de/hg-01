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

					if (!RCModel.resourcesLoadedFlag)
						OnStartLoad ();
					else
						OnResetLoad ();

					break;
				}

			case N.RCResetRoadModelTemplate:
			case N.GameOver:
				{
					ResetCurrentRoadModelToTemplate ();
					break;
				}
		}
	}

	private void OnStartLoad()
	{
		LoadPlayerSprites();
		LoadRoads ();

		RCModel.resourcesLoadedFlag = true;
	}

	private void OnResetLoad()
	{
		UpdatePlayerModel (GM.instance.PlayerSprites);
		ResetRoads ();
	}

	private void UpdatePlayerModel(Sprite[] playerSprites)
	{
		_playerModel.sprites = playerSprites;
	}

	private void UpdateRoadFactoryModel(RoadView[] roadTemplates)
	{
		_roadFactoryModel.roadTemplates = roadTemplates;
	}

	private void UpdateObstacleFactoryModel(Road roadAlias, ObstacleView[] obstacleTemplates)
	{
		ObstacleBundle obstacleBundle = System.Array.Find (_obstacleFactoryModel.obstacleBundles, o => o.roadAlias == roadAlias);

		obstacleBundle.obstacleTemplates = obstacleTemplates;
	}

	private void LoadPlayerSprites()
	{
		var playerSprites = Resources.LoadAll<Sprite>( _RCModel.playerSpriteResourcePath );

		GM.instance.PlayerSprites = playerSprites;

		UpdatePlayerModel( playerSprites );
	}
		
	public void LoadRoads()
	{
		string roadsPrefabPath = _RCModel.roadsPrefabPath;

		List<RoadView> roadTemplates = new List<RoadView>();

		float lastRoadPositionX = 0;
		float positionGapDistance = game.model.roadFactoryModel.roadsGapLength;

		foreach (string roadName in System.Enum.GetNames(typeof(Road)))
		{
			#region Init and instantiate road
			RoadView[] roadTemplate = Resources.LoadAll<RoadView> (GetDirFromPath (roadsPrefabPath) + "/" + GetFolderByRoadAlias ( GetRoadAliasByName( roadName)));
			RoadView instantiatedRoad = Instantiate(roadTemplate[0]) as RoadView;

			var roadPosition = instantiatedRoad.transform.position;

			roadPosition.x = lastRoadPositionX;

			instantiatedRoad.transform.position = roadPosition;
			instantiatedRoad.transform.SetParent (GM.instance.RoadContainer.transform);
				
			roadTemplates.Add (instantiatedRoad);

			lastRoadPositionX += positionGapDistance;
			#endregion

			#region Init and instantiate obstacles for road
			ObstacleView[] obstacleTemplates = LoadRoadObstacles (GetRoadAliasByName( roadName ) );

			GameObject roadContainerForObstacles = new GameObject ();
			roadContainerForObstacles.name = roadName;
			roadContainerForObstacles.transform.SetParent (GM.instance.ObstaclesContainer.transform);

			foreach (ObstacleView obstacleTemplate in obstacleTemplates)
			{
				obstacleTemplate.transform.SetParent (roadContainerForObstacles.transform);
			}
			#endregion

			UpdateObstacleFactoryModel (GetRoadAliasByName(roadName), obstacleTemplates);
		}

		UpdateRoadFactoryModel (roadTemplates.ToArray());
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

				obstacle.gameObject.SetActive (false);

				roadObstacleTemplates.Add (obstacle);
			}
		}

		return roadObstacleTemplates.ToArray ();
	}

	private void ResetRoads()
	{
		List<RoadView> roadTemplates = new List<RoadView>();

		foreach (RoadView roadTemplate in GM.instance.RoadContainer.GetComponentsInChildren<RoadView>())
		{
			roadTemplates.Add (roadTemplate);

			List<ObstacleView> obstacleTemplates = new List<ObstacleView>();
			Transform obstaclesContainerForRoad = GM.instance.ObstaclesContainer.transform.FindChild (roadTemplate.GetComponent<RoadModel>().alias.ToString());

			for(int i = 0; i < obstaclesContainerForRoad.childCount; i++)
				obstacleTemplates.Add (obstaclesContainerForRoad.GetChild(i).GetComponent<ObstacleView>());

			UpdateObstacleFactoryModel (roadTemplate.GetComponent<RoadModel>().alias, obstacleTemplates.ToArray());
		}

		UpdateRoadFactoryModel (roadTemplates.ToArray());
	}

	private void ResetCurrentRoadModelToTemplate()
	{
		foreach (RoadView roadTemplate in GM.instance.RoadContainer.GetComponentsInChildren<RoadView>())
		{
			if (!roadTemplate.GetComponent<RoadModel> ())
			{
				RoadModel addedModel = roadTemplate.gameObject.AddComponent<RoadModel> ();

				addedModel.GetCopyOf<RoadModel> (game.model.currentRoadModel);

				break;
			}
		}
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

	private Road GetRoadAliasByName(string roadName)
	{
		foreach (int value in System.Enum.GetValues (typeof(Road)))
		{
			if (System.Enum.GetName (typeof(Road), value) == roadName)
				return (Road)System.Enum.Parse(typeof(Road), roadName);
		}

		Debug.LogError ("There is no road = " + roadName);

		return Road.GINGERBREAD_MAN;
	}

	private string GetDirFromPath(string path)
	{
		string[] splitedPath = path.Split (new char[] { '/' });

		return splitedPath[splitedPath.Length - 1];
	}
}
