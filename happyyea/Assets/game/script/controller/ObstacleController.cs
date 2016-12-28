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

					//obstacle.gameObject.SetActive (false);

					//Destroy model copy component from factory
					Destroy( _obstacleFactoryModel.obstacleModelsDictionary[obstacle] );

					//Delete view from dictionary
					_obstacleFactoryModel.obstacleModelsDictionary.Remove (obstacle);

					//Destroy obstacle wrapper
					Destroy(obstacle.transform.parent.gameObject);
					break;
				}
		}
	}

	private void OnStart()
	{

	}

}
