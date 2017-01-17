using UnityEngine;
using System.Collections;
using DG.Tweening;

public class ObstacleView : View
{
	public bool 			isVisible				{ get { return _isVisible; } 		private set { _isVisible = value; } }
	private ObstacleModel	obstacleModel 			{ get { return game.model.obstacleFactoryModel.currentModelsDictionary[this]; } }
	private Vector3			obstacleSpriteSize		{ get { return obstacleModel.spriteSize; } }

	[SerializeField]
	private bool 			_isVisible 	= true;

	public void OnInit(Vector3 pathPosition, Quaternion rotation, bool isDownDirection)
	{
		PlaceObstacle(pathPosition, rotation, isDownDirection);
	}

	private void PlaceObstacle(Vector3 pathPosition, Quaternion rotation, bool isDownDirection)
	{
		transform.rotation = rotation;

		Vector3 lookDirection = isDownDirection ? -transform.up : transform.up;

		lookDirection.z = pathPosition.z = 5f;

		Vector3 correctPosition = pathPosition + Vector3.ClampMagnitude (lookDirection - pathPosition, obstacleSpriteSize.y) * (isDownDirection ? -1 : 1);
		Vector3	obstacleOutsidePosition = pathPosition + Vector3.ClampMagnitude (pathPosition - lookDirection, obstacleSpriteSize.y * 2f) * (isDownDirection ? 1 : -1);

		if (game.model.currentRoadModel.width / 2f - obstacleSpriteSize.y < 0.0f)
		{
			Debug.LogError ("Obstacle [" + transform.name + "] sprite height is bigger than half of current road width.");
		}

		correctPosition.z = obstacleOutsidePosition.z = 5f;

		gameObject.SetActive(true);
		isVisible = true;

		transform.position = obstacleOutsidePosition;
		transform.DOMove(correctPosition, 0.2f).OnComplete(() => {
			
			StopAllCoroutines();

			StartCoroutine(CheckVisibility());
		});
	}
		
	private IEnumerator CheckVisibility () 
	{
		while (true)
		{
			Vector3 screenPoint = Camera.main.WorldToViewportPoint (transform.position);

			if (obstacleModel.spriteForVisible.isVisible)
			{
				if (!isVisible)
					isVisible = true;
			}
			else
			{
				if (isVisible)
				{
					isVisible = false;

					yield return new WaitForSeconds(2f);

					Notify (N.ObstacleInvisible, this);

					break;
				}
			}

			yield return null;	
		}
	}

}
