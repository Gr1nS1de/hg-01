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
	private float 			_spriteHeightOffset;	//Offset by sprite bounds for proper position on road

	public void OnInit(float rotationZ, bool isDown)
	{
		SetPosition(new Vector3(0,0,rotationZ), isDown, null);
		gameObject.SetActive(true);
		StartPlacingObstacle(rotationZ, isDown);

	}

	private void StartPlacingObstacle(float rotationZ, bool isDown)
	{
		_spriteHeightOffset = obstacleSpriteSize.y * transform.localScale.y/1.5f;

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

			if (screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1)
			{
				if (!_isVisible)
					_isVisible = true;
			}
			else
			{
				if (_isVisible)
				{
					_isVisible = false;

					yield return new WaitForSeconds(1f);

					Notify (N.ObstacleInvisible, this);

					break;
				}
			}

			yield return null;	
		}
	}

	public void SetPosition(Vector3 rotation, bool isDown, System.Action callback)
	{

		Vector3 pPosition = new Vector3(game.model.roadModel.radius, 0f, game.view.playerView.transform.position.z );

		if(isDown)
		{
			var mPos = new Vector3(pPosition.x - _spriteHeightOffset, pPosition.y, pPosition.z);

			transform.localPosition = mPos;

			mPos = transform.position;

			transform.position = mPos;
			transform.localEulerAngles = new Vector3(0,0,-90);
		}
		else
		{
			var mPos = new Vector3(pPosition.x + _spriteHeightOffset, pPosition.y, pPosition.z);

			transform.localPosition = mPos;

			mPos = transform.position;

			transform.position = mPos;
			transform.localEulerAngles = new Vector3(0,0,+90);
		}

		transform.parent.eulerAngles = rotation;

	}

}
