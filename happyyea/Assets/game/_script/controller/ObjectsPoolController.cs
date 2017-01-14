using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectsPoolController : Controller
{
	public ObjectsPoolModel objectsPoolModel	{ get { return game.model.objectsPoolModel; } } 

	public override void OnNotification (string alias, Object target, params object[] data)
	{
		switch (alias)
		{
			case N.GamePlay:
				{
					OnGamePlay ();

					break;
				}
		}
	}

	private void OnGamePlay()
	{
		StartCoroutine( ObjectsPoolingRoutine() );
	}

	private IEnumerator ObjectsPoolingRoutine()
	{
		List<GameObject> poolingList = objectsPoolModel.poolingList;

		while ( true )
		{
			if (poolingList.Count <= 0)
				continue;



			yield return new WaitForSeconds( Random.Range( 0.20f, 0.5f ) );
		}
	}
}

