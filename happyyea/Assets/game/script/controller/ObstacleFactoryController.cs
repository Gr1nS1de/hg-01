using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ObstacleFactoryController : Controller
{
	private ObstacleFactoryModel _obstacleFactoryModel	{ get { return game.model.obstacleFactoryModel; } }

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
		StartCoroutine( ObstacleInstantiator() );
	}

	IEnumerator ObstacleInstantiator()
	{
		while ( true )
		{
			var allObstacles = FindObjectsOfType<ObstacleEntity>();
			bool doInstantiateObstacle = false;

			if ( allObstacles != null && allObstacles.Length > 1 )
			{
				var allVisibleObtacles = System.Array.FindAll( allObstacles, o => o.m_IsVisible == true );

				if ( allVisibleObtacles != null && allVisibleObtacles.Length < game.model.currentScore + 3 )
					doInstantiateObstacle = true;
			}
			else
				doInstantiateObstacle = true;

			if ( doInstantiateObstacle )
				DOInstantiateObstacle();

			yield return new WaitForSeconds( Util.GetRandomNumber( 0.20f, 0.5f ) );
		}
	}

	public void DODeleteObstacleInstance( GameObject obstacleInstance )
	{

		DestroyObject( obstacleInstance );
	}

	private void DOInstantiateObstacle()
	{
		
		ObstacleView objTmp = null;
		var obstacleTemplatesDictionary = game.model.obstacleFactoryModel.obstacleTemplatesDictionary;
		ObstacleState randomObstacleState = (ObstacleState)UnityEngine.Random.Range( 0, System.Enum.GetNames(typeof(ObstacleState)).Length );
		int randomInstanceIndex = UnityEngine.Random.Range( 0, obstacleTemplatesDictionary[randomObstacleState].Length );

		var obstacleRandomTemplate = obstacleTemplatesDictionary[randomObstacleState][randomInstanceIndex];

		objTmp = Instantiate( obstacleRandomTemplate ) as ObstacleView;
		objTmp.gameObject.SetActive( true );

		//obstacleEntity.Init( game.view.playerSpriteView.transform.eulerAngles.z - 30, Util.GetRandomNumber( 0f, 100f ) < 50 );

	}
}
