using UnityEngine;
using System.Collections;
using DG.Tweening;

public class ObstacleView : View<Game>
{
	public bool 			isVisible				{ get { return _isVisible; } 		private set { _isVisible = value; } }
	private ObstacleModel	obstacleModel 			{ get { return game.model.obstacleFactoryModel.currentModelsDictionary[this]; } }
	private Vector3			obstacleSpriteSize		{ get { return obstacleModel.spriteSize; } }

	[SerializeField]
	private bool 			_isVisible 	= true;

	public void OnInit(float rotationZ, bool isDown)
	{
		StartPlacingObstacle(rotationZ, isDown);
	}

	private void StartPlacingObstacle(float rotationZ, bool isDown)
	{

		int sign = +1;

		if(isDown)
			sign = -1;

		Vector3 correctLocalPosition = SetCorrectLocalPosition(new Vector3(0,0,rotationZ), isDown);

		gameObject.SetActive(true);

		isVisible = true;

		transform.position = new Vector3(correctLocalPosition.x, correctLocalPosition.y + sign * 0.2f, correctLocalPosition.z);
		transform.DOMoveY(correctLocalPosition.y, 0.2f).OnComplete(() => {
			
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

	public Vector3 SetCorrectLocalPosition(Vector3 rotation, bool isDown)
	{
		float spriteHeightOffset = obstacleSpriteSize.y * transform.localScale.y;//*2f;
		float playerPathElapsedPercentage = game.model.playerModel.playerPath.ElapsedPercentage(false);
		float forwarpPointPercentage = playerPathElapsedPercentage + 0.1f;

		if (forwarpPointPercentage > 1.0f)
			forwarpPointPercentage -= 1.0f;

		return game.model.playerModel.playerPath.PathGetPoint(forwarpPointPercentage);
	}

}
