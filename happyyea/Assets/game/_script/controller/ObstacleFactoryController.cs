using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ObstacleFactoryController : Controller
{
	private ObstacleFactoryModel 	obstacleFactoryModel	{ get { return game.model.obstacleFactoryModel; } }
	private ObjectsPoolModel		objectsPoolModel		{ get { return game.model.objectsPoolModel;}}

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
		obstacleFactoryModel.obstaclesDynamicContainer.name = "ObstaclesContainer";
		obstacleFactoryModel.obstaclesDynamicContainer.transform.SetParent (dynamic_objects.transform);
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
				int visibleObtacles = System.Array.FindAll( allObstacles, o => o.isVisible == true ).Length;
				int pooledObstacles = System.Array.FindAll( objectsPoolModel.poolingQueue.ToArray (), o => o.poolingType == PoolingObjectType.OBSTACLE).Length;
				int allObstaclesCount = visibleObtacles + pooledObstacles;

				if ( allObstaclesCount < 10 )
					doInstantiateObstacle = true;
			}
			else
				doInstantiateObstacle = true;

			if ( doInstantiateObstacle )
				AddObstacleToPoolQueue();


			yield return null;
		}
	}

	//Add random obstacle to pool queue
	private void AddObstacleToPoolQueue()
	{
		if (game.model.gameState != GameState.PLAYING)
			return;
		
		ObstacleView instantiatedObstacle;
		ObstacleState randomObstacleState = (ObstacleState)UnityEngine.Random.Range( 0, System.Enum.GetNames(typeof(ObstacleState)).Length );

		instantiatedObstacle = TryGetObstacleFromRecycleDictionary (randomObstacleState);

		if (!instantiatedObstacle)
			instantiatedObstacle = CreateNewObstacle (randomObstacleState);
		
		objectsPoolModel.poolingQueue.Enqueue (new PoolingObject{
			poolingType = PoolingObjectType.OBSTACLE,
			poolingObject = instantiatedObstacle
		});
	}

	private ObstacleView TryGetObstacleFromRecycleDictionary(ObstacleState obstacleState)
	{
		ObstacleView recyclableObstacle = null;
		var recyclableDictionary = obstacleFactoryModel.recyclableObstaclesDictionary;

		if (recyclableDictionary [obstacleState].Count >= 1)
		{
			recyclableObstacle = recyclableDictionary [obstacleState][0];

			recyclableDictionary[obstacleState].Remove(recyclableObstacle);
		}

		return recyclableObstacle;
	}

	private ObstacleView CreateNewObstacle(ObstacleState obstacleState)
	{
		var obstacleTemplatesDictionary = obstacleFactoryModel.templatesByStateDictionary;
		int randomInstanceIndex = UnityEngine.Random.Range( 0, obstacleTemplatesDictionary[obstacleState].Length );
		ObstacleView obstacleRandomTemplate = obstacleTemplatesDictionary[obstacleState][randomInstanceIndex];

		ObstacleView instanceObstacle = Instantiate( obstacleRandomTemplate ) as ObstacleView;

		#region storing obstacle model copy to dictionary
		ObstacleModel obstacleModel = instanceObstacle.GetComponent<ObstacleModel> ();

		ObstacleModel obstacleModelCopy = obstacleFactoryModel.gameObject.AddComponent<ObstacleModel>();
		obstacleModelCopy.GetCopyOf<ObstacleModel> (obstacleModel);

		obstacleFactoryModel.currentModelsDictionary.Add (instanceObstacle, obstacleModelCopy);

		Destroy (obstacleModel);
		#endregion

		instanceObstacle.transform.SetParent (obstacleFactoryModel.obstaclesDynamicContainer.transform);

		#region create sprite renderer object for checking visibility of obstacle by camera.
		GameObject spriteForVisible = CreateSpriteForVisibility ();

		spriteForVisible.transform.SetParent (instanceObstacle.transform);
		spriteForVisible.transform.localPosition = Vector3.zero;

		obstacleModelCopy.spriteForVisible = spriteForVisible.GetComponent<SpriteRenderer>();
		#endregion

		return instanceObstacle;
	}


	private GameObject CreateSpriteForVisibility()
	{
		GameObject spriteForVisible = new GameObject ();

		spriteForVisible.AddComponent<SpriteRenderer> ();

		return spriteForVisible;
	}
		
}
