using UnityEngine;
using System.Collections.Generic;

public class RoadController : Controller
{
	private RoadFactoryModel 			_roadFactoryModel		{ get { return game.model.roadFactoryModel; } }

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

	}
		

}
