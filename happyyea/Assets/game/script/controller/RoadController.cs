using UnityEngine;
using System.Collections.Generic;

public class RoadController : Controller<Game>
{
	private RoadFactoryModel 			_roadFactoryModel;
	public Dictionary<int, RoadModel> 	_roadModelsDictionary;

	public override void OnNotification (string alias, Object target, params object[] data)
	{
		switch (alias) 
		{
			case N.GameStart:
				{
					OnStart ();

					break;
				}

		
			case N.GameOver:
				{
				
					break;
				}
		}
	}

	private void OnStart()
	{
		_roadFactoryModel = game.model.roadFactoryModel;
		_roadModelsDictionary = _roadFactoryModel.roadModelsDictionary;
	}
		

}
