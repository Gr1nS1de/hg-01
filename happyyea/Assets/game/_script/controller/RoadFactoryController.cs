using UnityEngine;
using System.Collections;

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
		}
	}

	private void OnStart()
	{
		InstantiateRoad ();

		Notify (N.GameRoadInstantiated);
	}

	public RoadModel InstantiateRoad()
	{
		RoadView roadObject = Instantiate(_roadFactoryModel.roadTemplate) as RoadView;
		RoadView roadView = roadObject.GetComponent<RoadView>();
		RoadModel roadModel = roadObject.GetComponent<RoadModel> ();
		RoadModel roadModelCopy = roadModel.GetCopyOf<RoadModel> (roadModel);

		Destroy (roadModel);

		game.model.currentRoadModel.GetCopyOf<RoadModel>(roadModelCopy);

		roadView.OnStart (roadModelCopy);

		return roadModelCopy;
	}
}
	