using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class RoadFactoryController : Controller
{
	private RoadFactoryModel 	_roadFactoryModel	{ get { return game.model.roadFactoryModel; } }

	public override void OnNotification (string alias, Object target, params object[] data)
	{
		switch (alias)
		{
			case N.GameStart:
				{
					OnStart ();

					break;
				}

			case N.GameChangeRoad:
				{
					ChangeRoad ();
					
					break;
				}

			case N.GamePlayerPlacedOnRoad:
				{
					game.view.roadView.OnPlayerPlaced ();
					break;
				}
		}
	}

	private void OnStart()
	{
		InitRoads ();

		Notify (N.GameRoadInited);
	}

	public RoadModel InitRoads()
	{
		RoadModel currentGameRoadModelCopy = null;

		foreach(RoadView roadView in _roadFactoryModel.roadTemplates)
		{
			RoadModel roadModel = roadView.GetComponent<RoadModel> ();

			CommitStaticSprites (roadView);

			if (roadModel.alias == game.model.currentRoad)
			{
				currentGameRoadModelCopy = roadModel.GetCopyOf<RoadModel> (roadModel);

				Destroy (roadModel);
			}

			var roadsContainerPosition = GM.instance.RoadContainer.transform.position;

			roadsContainerPosition.x = -(_roadFactoryModel.roadsGapLength * ( (int)game.model.currentRoad - 1));

			GM.instance.RoadContainer.transform.position = roadsContainerPosition;

			roadView.OnStart (roadModel);
		}

		game.model.currentRoadModel.GetCopyOf<RoadModel>(currentGameRoadModelCopy);

		return currentGameRoadModelCopy;
	}

	private void CommitStaticSprites(RoadView road)
	{
		tk2dStaticSpriteBatcher staticBatcher = road.GetComponent<tk2dStaticSpriteBatcher> ();
		tk2dSprite[] roadSprites = road.GetComponentsInChildren<tk2dSprite> ();

		staticBatcher.batchedSprites = new tk2dBatchedSprite[roadSprites.Length];

		int counter = 0;

		foreach(tk2dSprite roadSprite in roadSprites)
		{
			tk2dBatchedSprite batchedSprite = new tk2dBatchedSprite ();

			batchedSprite.name = roadSprite.CurrentSprite.name;
			batchedSprite.spriteCollection = roadSprite.Collection;
			batchedSprite.spriteId = roadSprite.spriteId;

			Vector3 batchedSpritePosition = roadSprite.transform.position;
			Vector3 batchedSpriteScale = roadSprite.scale;
			Quaternion batchedSpriteRotation = roadSprite.transform.rotation;

			// Assign the relative matrix. Use this in place of bs.position
			//batchedSprite.relativeMatrix.SetTRS(batchedSpritePosition, batchedSpriteRotation, batchedSpriteScale);
			batchedSprite.position = batchedSpritePosition;
			batchedSprite.rotation = batchedSpriteRotation;
			batchedSprite.baseScale = batchedSpriteScale;

			staticBatcher.batchedSprites[counter] = batchedSprite;

			roadSprite.gameObject.SetActive (false);

			counter++;
		}

		// Don't create colliders when you don't need them. It is very expensive to
		// generate colliders at runtime.
		staticBatcher.SetFlag( tk2dStaticSpriteBatcher.Flags.GenerateCollider, false );

		staticBatcher.UpdateMatrices ();

		staticBatcher.Build();
	}

	private void ChangeRoad()
	{
		RoadModel currentGameRoadModelCopy = null;

		RoadModel roadModel = System.Array.Find (_roadFactoryModel.roadTemplates, roadView => roadView.GetComponent<RoadModel> ().alias == game.model.currentRoad).GetComponent<RoadModel> ();

		currentGameRoadModelCopy = roadModel.GetCopyOf<RoadModel> (roadModel);

		Destroy (roadModel);
		   
		game.model.currentRoadModel.GetCopyOf<RoadModel>(currentGameRoadModelCopy);
		
		GM.instance.RoadContainer.transform.DOMoveX (-(_roadFactoryModel.roadsGapLength * ( (int)game.model.currentRoad - 1)), 0.5f)
			.OnComplete(() => 
			{
				Notify(N.GameRoadChanged);
			});
	}
}
	