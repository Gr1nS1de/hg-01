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

			case N.GamePlay:
				{
					OnGamePlay ();

					break;
				}
		}
	}

	private void OnStart()
	{
		_obstacleFactoryModel.obstaclesDynamicContainer.name = "ObstaclesContainer";
		_obstacleFactoryModel.obstaclesDynamicContainer.transform.SetParent (dynamic_objects.transform);
	}

	private void OnGamePlay()
	{
		StartCoroutine( ObstacleInstantiator() );
	}

	IEnumerator ObstacleInstantiator()
	{
		while ( true )
		{
			var allObstacles = FindObjectsOfType<ObstacleView>();
			bool doInstantiateObstacle = false;

			if ( allObstacles != null && allObstacles.Length > 1 )
			{
				var allVisibleObtacles = System.Array.FindAll( allObstacles, o => o.isVisible == true );

				if ( allVisibleObtacles != null && allVisibleObtacles.Length < game.model.currentScore + 3 )
					doInstantiateObstacle = true;
			}
			else
				doInstantiateObstacle = true;

			if ( doInstantiateObstacle )
				DOInstantiateObstacle();

			yield return new WaitForSeconds( Utils.GetRandomNumber( 0.20f, 0.5f ) );
		}
	}

	public void DODeleteObstacleInstance( GameObject obstacleInstance )
	{

		DestroyObject( obstacleInstance );
	}

	//Instantiate random obstacle
	private void DOInstantiateObstacle()
	{
		ObstacleView instantiatedObstacle = null;
		var obstacleTemplatesDictionary = game.model.obstacleFactoryModel.obstacleTemplatesDictionary;
		ObstacleState randomObstacleState = (ObstacleState)UnityEngine.Random.Range( 0, System.Enum.GetNames(typeof(ObstacleState)).Length );
		int randomInstanceIndex = UnityEngine.Random.Range( 0, obstacleTemplatesDictionary[randomObstacleState].Length );

		ObstacleView obstacleRandomTemplate = obstacleTemplatesDictionary[randomObstacleState][randomInstanceIndex];

		instantiatedObstacle = Instantiate( obstacleRandomTemplate ) as ObstacleView;
		//instantiatedObstacle.gameObject.SetActive( true );

		ObstacleModel obstacleModel = instantiatedObstacle.GetComponent<ObstacleModel> ();

		ObstacleModel obstacleModelCopy = _obstacleFactoryModel.gameObject.AddComponent<ObstacleModel>();
		obstacleModelCopy.GetCopyOf<ObstacleModel> (obstacleModel);

		_obstacleFactoryModel.obstacleModelsDictionary.Add (instantiatedObstacle, obstacleModelCopy);

		Destroy (obstacleModel);

		GameObject wrapObject = CreateWrapperForObstacle ();

		instantiatedObstacle.transform.SetParent (wrapObject.transform);

		GameObject spriteForVisible = CreateSpriteForVisibility ();

		spriteForVisible.transform.SetParent (wrapObject.transform);
		obstacleModelCopy.spriteForVisible = spriteForVisible.GetComponent<SpriteRenderer>();

		wrapObject.transform.SetParent (_obstacleFactoryModel.obstaclesDynamicContainer.transform);

		instantiatedObstacle.OnInit (game.view.playerSpriteView.transform.eulerAngles.z - 30, Utils.GetRandomNumber( 0f, 100f ) < 50  );
	}

	private GameObject CreateWrapperForObstacle()
	{
		GameObject wrapObject = new GameObject ();

		wrapObject.name = "obstacleWrapper";

		return wrapObject;
	}

	private GameObject CreateSpriteForVisibility()
	{
		GameObject spriteForVisible = new GameObject ();

		spriteForVisible.AddComponent<SpriteRenderer> ();

		return spriteForVisible;
	}
		
}
