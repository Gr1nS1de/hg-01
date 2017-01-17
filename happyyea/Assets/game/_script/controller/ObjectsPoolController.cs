using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class ObjectsPoolController : Controller
{
	public ObjectsPoolModel objectsPoolModel	{ get { return game.model.objectsPoolModel; } } 
	public ObjectsPoolView	objectsPoolView		{ get { return game.view.objectsPoolView;}}

	private Vector3			_lastObstaclePoolerViewPosition;

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
		//Start moving pooler object
		StartCoroutine (MovePoolerViewRoutine());
		//Start pooling
		StartCoroutine( ObjectsPoolingRoutine() );
	}

	private IEnumerator ObjectsPoolingRoutine()
	{
		Queue<PoolingObject> poolingQueue = objectsPoolModel.poolingQueue;

		yield return null;

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

						PoolObstacle (obstacleView);

						yield return new WaitForSeconds( Random.Range( 0.20f, 0.5f ) );

						break;
					}
			}
					
		}
	}

	private IEnumerator MovePoolerViewRoutine()
	{
		while (true)
		{
			//float spriteHeightOffset = obstacleSpriteSize.y * transform.localScale.y;//*2f;
			float playerPathElapsedPercentage = game.model.playerModel.playerPath.ElapsedPercentage(false);
			float forwarpPointPercentage = playerPathElapsedPercentage + objectsPoolModel.gapPercentage;

			if (forwarpPointPercentage > 1.0f)
				forwarpPointPercentage -= 1.0f;

			Vector3 forwardPosition = game.model.playerModel.playerPath.PathGetPoint(forwarpPointPercentage);

			objectsPoolView.transform.position = forwardPosition;

			objectsPoolModel.poolerPositionDelta = forwardPosition - _lastObstaclePoolerViewPosition;

			_lastObstaclePoolerViewPosition = forwardPosition;

			yield return null;
		}
	}

	private void PoolObstacle(ObstacleView obstacleView)
	{
		var directionPoint = objectsPoolModel.poolerPositionDelta;
		var angle = Mathf.Atan2(directionPoint.y, directionPoint.x) * Mathf.Rad2Deg;
		bool isDownDirection = false;

		if (Random.Range (0, 2) == 0)
		{
			isDownDirection = true;
			angle += 180;
		}

		Quaternion obstacleRotation = Quaternion.AngleAxis(angle, Vector3.forward);

		obstacleView.OnInit (objectsPoolView.transform.position, obstacleRotation, isDownDirection );

	}
}

