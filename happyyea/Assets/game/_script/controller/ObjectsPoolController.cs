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
		Queue<PoolingObject> poolingQueue = objectsPoolModel.poolingQueue;

		while ( true )
		{
			if (poolingQueue.Count <= 0)
			{
				yield return null;
				continue;
			}

			PoolingObject poolingObject = poolingQueue.Dequeue ();

			switch (poolingObject.poolingType)
			{
				case PoolingObjectType.OBSTACLE:
					{
						ObstacleView obstacleView = (ObstacleView)poolingObject.poolingObject;

						obstacleView.OnInit (game.view.playerSpriteContainerView.transform.eulerAngles.z - 30, Random.Range( 0f, 100f ) < 50  );

						yield return new WaitForSeconds( Random.Range( 0.20f, 0.5f ) );

						break;
					}
			}
					
		}
	}
}

