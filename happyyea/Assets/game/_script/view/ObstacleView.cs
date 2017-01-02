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

		Vector3 correctLocalPosition = SetCorrectLocalPosition(new Vector3(0,0,rotationZ), isDown, null);

		gameObject.SetActive(true);

		isVisible = true;

		transform.localPosition = new Vector3(correctLocalPosition.x + sign * 0.2f, correctLocalPosition.y, correctLocalPosition.z);
		transform.DOLocalMoveX(correctLocalPosition.x, 0.2f).OnComplete(() => {
			
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

	public Vector3 SetCorrectLocalPosition(Vector3 rotation, bool isDown, System.Action callback)
	{
		float spriteHeightOffset = obstacleSpriteSize.y * transform.localScale.y;//*2f;
		Vector3 pPosition = new Vector3(game.model.currentRoadModel.radius, 0f, game.view.playerView.transform.position.z );

		Vector3 searchPosition;

		if(isDown)
		{
			searchPosition = new Vector3(pPosition.x - spriteHeightOffset, pPosition.y, pPosition.z);

			transform.localPosition = searchPosition;

			searchPosition = transform.position;

			transform.position = searchPosition;
			transform.localEulerAngles = new Vector3(0,0,-90);
		}
		else
		{
			searchPosition = new Vector3(pPosition.x + spriteHeightOffset, pPosition.y, pPosition.z);

			transform.localPosition = searchPosition;

			searchPosition = transform.position;

			transform.position = searchPosition;
			transform.localEulerAngles = new Vector3(0,0,+90);
		}

		obstacleModel.spriteForVisible.transform.position = searchPosition;

		transform.parent.eulerAngles = rotation;

		return transform.localPosition;
	}

}
