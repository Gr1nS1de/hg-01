using UnityEngine;
using System.Collections;

public class RoadController : Controller<Game>
{

	public override void OnNotification (string alias, Object target, params object[] data)
	{
		switch (alias) 
		{
			case N.GameStart:
				{
				
					break;
				}

		
			case N.GameOver:
				{
				
					break;
				}
		}
	}
}
