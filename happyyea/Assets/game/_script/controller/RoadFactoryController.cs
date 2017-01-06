using UnityEngine;
using System.Collections;
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
		}
	}

	private void OnStart()
	{
		PlaceRoads ();

		Notify (N.GameRoadsPlaced);
	}

	public RoadModel PlaceRoads()
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

			roadView.OnStart (roadModel);
		}

		game.model.currentRoadModel.GetCopyOf<RoadModel>(currentGameRoadModelCopy);

		return currentGameRoadModelCopy;
	}

	private void ChangeRoad()
	{
		RoadModel currentGameRoadModelCopy = null;

		foreach (RoadView roadView in _roadFactoryModel.roadTemplates)
		{
			RoadModel roadModel = roadView.GetComponent<RoadModel> ();

			if(roadModel)
				if (roadModel.alias == game.model.currentRoad)
				{
					currentGameRoadModelCopy = roadModel.GetCopyOf<RoadModel> (roadModel);

					Destroy (roadModel);
				}
		}
		   
		game.model.currentRoadModel.GetCopyOf<RoadModel>(currentGameRoadModelCopy);
		
		GM.instance.RoadContainer.transform.DOMoveX (-(_roadFactoryModel.roadsGapLength * ( (int)game.model.currentRoad - 1)), 0.5f)
			.OnComplete(() => 
			{
				Notify(N.GameRoadChanged);
			});
	}
}
	