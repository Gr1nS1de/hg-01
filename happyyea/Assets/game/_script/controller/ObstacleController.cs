using UnityEngine;
using System.Collections;

public class ObstacleController : Controller
{
	private ObstacleFactoryModel _obstacleFactoryModel	{ get { return game.model.obstacleFactoryModel; } }
	
	public override void OnNotification( string alias, Object target, params object[] data )
	{
		switch ( alias )
		{
			case N.GameStart:
				{
					OnStart ();

					break;
				}

			case N.ObstacleInvisible:
				{
					ObstacleView obstacle = (ObstacleView)data [0];

					CheckRecycleObstacle (obstacle);

					break;
				}
		}
	}

	private void OnStart()
	{

	}

	private void CheckRecycleObstacle(ObstacleView obstacleView)
	{
		ObstacleModel obstacleModel = _obstacleFactoryModel.currentModelsDictionary [obstacleView];

		switch (obstacleModel.recyclableState)
		{
			case ObstacleRecyclableState.RECYCLABLE:
				{
					StoreObstacleForRecycle (obstacleView);
					break;
				}

			case ObstacleRecyclableState.NON_RECYCLABLE:
				{
					DeleteObstacle (obstacleView);
					break;
				}
		}
	}

	private void StoreObstacleForRecycle(ObstacleView obstacleView)
	{
		ObstacleModel obstacleModel = _obstacleFactoryModel.currentModelsDictionary [obstacleView];
		var recyclableDictionary = _obstacleFactoryModel.recyclableObstaclesDictionary;

		recyclableDictionary[obstacleModel.state].Add (obstacleView);

		obstacleView.gameObject.SetActive (false);
	}

	private void DeleteObstacle(ObstacleView obstacleView)
	{
		//Destroy model copy component from factory
		Destroy( _obstacleFactoryModel.currentModelsDictionary[obstacleView] );

		//Delete view from dictionary
		_obstacleFactoryModel.currentModelsDictionary.Remove (obstacleView);

		//Destroy obstacle wrapper
		Destroy(obstacleView.transform.parent.gameObject);
	}
}
