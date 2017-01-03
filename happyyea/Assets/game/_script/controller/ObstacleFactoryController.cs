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
				InstantiateObstacle();

			yield return new WaitForSeconds( Utils.GetRandomNumber( 0.20f, 0.5f ) );
		}
	}

	//Instantiate random obstacle
	private void InstantiateObstacle()
	{
		if (game.model.gameState != GameState.PLAYING)
			return;
		
		ObstacleView instantiatedObstacle;
		ObstacleState randomObstacleState = (ObstacleState)UnityEngine.Random.Range( 0, System.Enum.GetNames(typeof(ObstacleState)).Length );

		instantiatedObstacle = TryGetObstacleFromRecycleDictionary (randomObstacleState);

		if (!instantiatedObstacle)
			instantiatedObstacle = CreateNewObstacle (randomObstacleState);

		//instantiatedObstacle.gameObject.SetActive( true );

		instantiatedObstacle.OnInit (game.view.playerSpriteView.transform.eulerAngles.z - 30, Utils.GetRandomNumber( 0f, 100f ) < 50  );
	}

	private ObstacleView TryGetObstacleFromRecycleDictionary(ObstacleState obstacleState)
	{
		ObstacleView recyclableObstacle = null;
		var recyclableDictionary = _obstacleFactoryModel.recyclableObstaclesDictionary;

		if (recyclableDictionary [obstacleState].Count >= 1)
		{
			recyclableObstacle = recyclableDictionary [obstacleState][0];

			recyclableDictionary[obstacleState].Remove(recyclableObstacle);
		}

		return recyclableObstacle;
	}

	private ObstacleView CreateNewObstacle(ObstacleState obstacleState)
	{
		var obstacleTemplatesDictionary = _obstacleFactoryModel.templatesByStateDictionary;
		int randomInstanceIndex = UnityEngine.Random.Range( 0, obstacleTemplatesDictionary[obstacleState].Length );
		ObstacleView obstacleRandomTemplate = obstacleTemplatesDictionary[obstacleState][randomInstanceIndex];

		ObstacleView instanceObstacle = Instantiate( obstacleRandomTemplate ) as ObstacleView;

		#region storing obstacle model copy to dictionary
		ObstacleModel obstacleModel = instanceObstacle.GetComponent<ObstacleModel> ();

		ObstacleModel obstacleModelCopy = _obstacleFactoryModel.gameObject.AddComponent<ObstacleModel>();
		obstacleModelCopy.GetCopyOf<ObstacleModel> (obstacleModel);

		_obstacleFactoryModel.currentModelsDictionary.Add (instanceObstacle, obstacleModelCopy);

		Destroy (obstacleModel);
		#endregion

		#region create wrap-base object for obstacle
		GameObject wrapObject = CreateWrapperForObstacle ();

		wrapObject.transform.SetParent (_obstacleFactoryModel.obstaclesDynamicContainer.transform);
		#endregion

		instanceObstacle.transform.SetParent (wrapObject.transform);

		#region create sprite renderer object for checking visibility of obstacle by camera.
		GameObject spriteForVisible = CreateSpriteForVisibility ();

		spriteForVisible.transform.SetParent (wrapObject.transform);
		obstacleModelCopy.spriteForVisible = spriteForVisible.GetComponent<SpriteRenderer>();
		#endregion

		return instanceObstacle;
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
