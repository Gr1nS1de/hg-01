﻿using UnityEngine;
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
					var roadId = (int)data [0];

					OnStartLoad (roadId);

					break;
				}
		}
	}

	private void OnStartLoad(int roadId)
	{
		LoadPlayerSprites();
		LoadRoad(roadId);
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
		

	public void LoadRoad(int id)
	{
		string roadsPrefabPath = _RCModel.roadsPrefabPath;
		string[] roadsPrefabDirs = System.IO.Directory.GetDirectories( roadsPrefabPath );
		string roadPrefabDir = GetDirFromPath(roadsPrefabDirs[id-1]);
		RoadView[] roadTemplate = Resources.LoadAll<RoadView>(GetDirFromPath (roadsPrefabPath) + "/" + roadPrefabDir);

		ObstacleView[] obstacleTemplates = LoadRoadObstacles (id);
			
		UpdateRoadFactoryModel ( roadTemplate[0] );
		UpdateObstacleFactoryModel ( obstacleTemplates );
		//_obstacleFactoryModel.obstacleTemplates = obstaclesViews;
	}

	private ObstacleView[] LoadRoadObstacles(int roadId)
	{
		List<ObstacleView> roadObstacleTemplates = new List<ObstacleView>();

		foreach (string obstacleStateName in System.Enum.GetNames(typeof(ObstacleState)))
		{
			ObstacleState obstacleState = (ObstacleState)GetObstacleStateValueByName (obstacleStateName);

			ObstacleView obstaclePrefab = GetObstaclePrefab (roadId, obstacleState);
			Sprite[] obstaclesSprites = GetObstacleSprites (roadId, obstacleState);

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

				obstacle.GetComponent<ObstacleModel> ().obstacleView = obstacle;
				obstacle.gameObject.SetActive (false);

				roadObstacleTemplates.Add (obstacle);
			}
		}

		return roadObstacleTemplates.ToArray ();
	}

	private ObstacleView GetObstaclePrefab(int roadId, ObstacleState obstacleState)
	{
		string roadsPrefabPath = _RCModel.roadsPrefabPath;
		string[] roadsPrefabDirs = System.IO.Directory.GetDirectories( roadsPrefabPath );
		string roadPrefabDir = GetDirFromPath(roadsPrefabDirs[roadId-1]);
		string[] roadPrefabDirFolders = System.IO.Directory.GetDirectories( roadsPrefabPath + "/" + roadPrefabDir );

		ObstacleView[] obstaclesPrefabs = Resources.LoadAll<ObstacleView> (GetDirFromPath (roadsPrefabPath) + "/" + roadPrefabDir + "/obstacles/" + System.Enum.GetName(typeof(ObstacleState), obstacleState).ToLower());

		return obstaclesPrefabs[0];
	}

	private Sprite[] GetObstacleSprites(int roadId, ObstacleState obstacleState)
	{
		string roadsSpritePath = _RCModel.roadsSpritePath;
		string[] roadsSpriteDirs = System.IO.Directory.GetDirectories( roadsSpritePath );
		string roadSpritebDir = GetDirFromPath(roadsSpriteDirs[roadId-1]);
		string[] roadSpriteDirFolders = System.IO.Directory.GetDirectories( roadsSpritePath + "/" + roadSpritebDir );

		Sprite[] obstaclesSprites = Resources.LoadAll<Sprite> (GetDirFromPath (roadsSpritePath) + "/" + roadSpritebDir + "/obstacles/" + System.Enum.GetName(typeof(ObstacleState), obstacleState).ToLower());

		return obstaclesSprites;
	}

	private int GetObstacleStateValueByName(string name)
	{
		foreach (int value in System.Enum.GetValues (typeof(ObstacleState)))
		{
			if (System.Enum.GetName (typeof(ObstacleState), value) == name)
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
