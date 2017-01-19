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

			case N.GameRoadChangeStart__:
				{
					var prevRoadAlias = (Road)data [0];
					var newRoadAlias = (Road)data [1];

					ChangeCurrentRoad (newRoadAlias);
					
					break;
				}

			case N.GamePlayerPlacedOnRoad:
				{
					//game.view.currentRoadView.OnPlayerPlaced ();
					break;
				}
		}
	}

	private void OnStart()
	{
		InitRoads ();

		Notify (N.GameRoadsPlaced);
	}

	public RoadModel InitRoads()
	{
		RoadModel currentGameRoadModelCopy = null;

		foreach(RoadView roadView in _roadFactoryModel.roadTemplates)
		{
			RoadModel roadModel = roadView.GetComponent<RoadModel> ();

			if (roadModel.alias == game.model.currentRoad)
			{
				currentGameRoadModelCopy = roadModel.GetCopyOf<RoadModel> (roadModel);

				Destroy (roadModel);
			}

			var roadsContainerPosition = GM.instance.RoadContainer.transform.position;

			roadsContainerPosition.x = -(_roadFactoryModel.roadsGapLength * ( (int)game.model.currentRoad - 1));

			GM.instance.RoadContainer.transform.position = roadsContainerPosition;

		}

		game.model.currentRoadModel.GetCopyOf<RoadModel>(currentGameRoadModelCopy);

		return currentGameRoadModelCopy;
	}

	private void ChangeCurrentRoad(Road roadAlias)
	{
		RoadModel currentGameRoadModelCopy = null;
		RoadView roadView = System.Array.Find (_roadFactoryModel.roadTemplates, rView => rView.GetComponent<RoadModel> ().alias == roadAlias);
		RoadModel roadModel = roadView.GetComponent<RoadModel> ();

		currentGameRoadModelCopy = roadModel.GetCopyOf<RoadModel> (roadModel);

		Destroy (roadModel);
		   
		game.model.currentRoadModel.GetCopyOf<RoadModel>(currentGameRoadModelCopy);

		/*
		GM.instance.RoadContainer.transform.DOMoveX (-(_roadFactoryModel.roadsGapLength * ( (int)game.model.currentRoad - 1)), 0.5f)
			.OnComplete(() => 
			{
				Notify(N.GameRoadChanged);
			});
			*/
	}
}
	