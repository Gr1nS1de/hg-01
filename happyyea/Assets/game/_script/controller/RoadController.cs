﻿using UnityEngine;
using System.Collections.Generic;

public class RoadController : Controller
{
	private RoadFactoryModel 	_roadFactoryModel		{ get { return game.model.roadFactoryModel; } }

	public override void OnNotification (string alias, Object target, params object[] data)
	{
		switch (alias) 
		{
			case N.GameStart:
				{
					OnStart ();

					break;
				}
			case N.GameRoadsPlaced:
				{
					InitRoads ();

					break;
				}

			case N.GameRoadChangeEnd:
				{
					break;
				}

		
			case N.GameOver_:
				{
				
					break;
				}
		}
	}

	private void OnStart()
	{
		CommitStaticSprites (game.view.currentRoadView);
	}

	private void InitRoads()
	{
		foreach (RoadView roadView in _roadFactoryModel.roadTemplates)
		{
			RoadModel roadModel = roadView.GetComponent<RoadModel> ();

			roadView.OnInit (roadModel);
		}
	}
		
	private void CommitStaticSprites(RoadView road)
	{
		Utils.ActivateTransformChildrens (road.transform, true);
		
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
}
