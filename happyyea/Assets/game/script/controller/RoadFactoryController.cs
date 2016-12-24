using UnityEngine;
using System.Collections;

public class RoadFactoryController : Controller<Game> 
{
	private RoadFactoryModel 	_roadFactoryModel;

	public override void OnNotification (string alias, Object target, params object[] data)
	{
		switch (alias)
		{
			case N.GameStart:
				{
					OnStart ();

					break;
				}
		}
	}

	private void OnStart()
	{
		_roadFactoryModel = game.model.roadFactoryModel;

		game.model.currentRoadModel = InstantiateRoad (0);
	}

	public RoadModel InstantiateRoad(int templateId)
	{
		var roadObject = Instantiate(_roadFactoryModel.roadTemplates[0]) as RoadView;
		var roadView = roadObject.GetComponent<RoadView>();
		var roadModel = roadObject.GetComponent<RoadModel> ();
		var roadModelCopy = roadModel.GetCopyOf<RoadModel> (roadModel);

		Destroy (roadModel);

		_roadFactoryModel.roadModelsDictionary.Add (roadModel.id, roadModelCopy);

		roadView.OnStart (roadModelCopy);

		return roadModelCopy;
	}
}
	