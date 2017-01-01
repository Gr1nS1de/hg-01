using UnityEngine;
using System.Collections;
using DG.Tweening;

public class ObstacleView : View<Game>
{
	public bool 			isVisible				{ get { return _isVisible; } 		private set { _isVisible = value; } }
	private ObstacleModel	obstacleModel 			{ get { return game.model.obstacleFactoryModel.obstacleModelsDictionary[this]; } }
	private Vector3			obstacleSpriteSize		{ get { return obstacleModel.spriteSize; } }

	[SerializeField]
	private bool 			_isVisible 	= true;

	public void OnInit(float rotationZ, bool isDown)
	{
		SetPosition(new Vector3(0,0,rotationZ), isDown, null);
		gameObject.SetActive(true);
		StartPlacingObstacle(rotationZ, isDown);

	}

	private void StartPlacingObstacle(float rotationZ, bool isDown)
	{

		int sign = +1;

		if(isDown)
			sign = -1;

		var currentObstaclePosition = transform.localPosition;

		isVisible = true;

		transform.localPosition = new Vector3(currentObstaclePosition.x + sign * 0.2f, currentObstaclePosition.y, currentObstaclePosition.z);
		transform.DOLocalMoveX(currentObstaclePosition.x, 0.2f).OnComplete(() => {
			
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
				if (!_isVisible)
					_isVisible = true;
			}
			else
			{
				if (_isVisible)
				{
					_isVisible = false;

					yield return new WaitForSeconds(2f);

					Notify (N.ObstacleInvisible, this);

					break;
				}
			}

			yield return null;	
		}
	}

	public void SetPosition(Vector3 rotation, bool isDown, System.Action callback)
	{
		float spriteHeightOffset = obstacleSpriteSize.y * transform.localScale.y*2f;
		Vector3 pPosition = new Vector3(game.model.currentRoadModel.radius, 0f, game.view.playerView.transform.position.z );

		Debug.Log ("Sprite Y offset = " + spriteHeightOffset + " " + obstacleSpriteSize.y);

		Vector3 correctPosition;

		if(isDown)
		{
			correctPosition = new Vector3(pPosition.x - spriteHeightOffset, pPosition.y, pPosition.z);

			transform.localPosition = correctPosition;

			correctPosition = transform.position;

			transform.position = correctPosition;
			transform.localEulerAngles = new Vector3(0,0,-90);
		}
		else
		{
			correctPosition = new Vector3(pPosition.x + spriteHeightOffset, pPosition.y, pPosition.z);

			transform.localPosition = correctPosition;

			correctPosition = transform.position;

			transform.position = correctPosition;
			transform.localEulerAngles = new Vector3(0,0,+90);
		}

		obstacleModel.spriteForVisible.transform.position = correctPosition;

		transform.parent.eulerAngles = rotation;

	}

}
