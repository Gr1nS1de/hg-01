using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

/// <summary>
/// Class in charge to the camera animation: zoom in at start, shake when player jumps, and follow the player during the game
/// </summary>
public class CameraManager : MonoBehaviour
{
	Camera cam;

	GM gameManager; 

	public Transform toFollow;

	float orthoSize = 2f;

	void Awake()
	{
		gameManager = FindObjectOfType<GM>();
		cam = GetComponent<Camera>();
	}

	IEnumerator Start()
	{
		StartCoroutine(DOFollow());

		yield return 0;
	}

    void Update()
    {
        cam.backgroundColor = gameManager.m_ThemeDynamicColor;
    }

	public void DOStart(Action callback)
	{
		cam.DOOrthoSize(orthoSize, 0.7f)
			.SetEase(Ease.OutBack)
			.OnComplete(() => {
				DOVirtual.DelayedCall(0.2f, () => {
					if(callback != null)
						callback();
				});
			});
	}

	IEnumerator DOFollow()
	{
		while(true)
		{
			Vector3 pos = transform.position;
			pos.x = toFollow.position.x;
			pos.y = toFollow.position.y;
			transform.position = pos;

			yield return 0;
		}
	}

	public void DOShake()
	{
		if(!gameManager.m_ActivateCameraShake)
			return; 
		
		if(DOTween.IsTweening(cam))
			return;
		
		cam.DOOrthoSize(orthoSize * 1.03f, 0.02f).OnComplete(() => {
			cam.DOOrthoSize(orthoSize * 0.97f, 0.04f).OnComplete(() => {
				cam.DOOrthoSize(orthoSize, 0.02f).OnComplete(() => {
					cam.orthographicSize = orthoSize;
				});
//				cam.DOOrthoSize(orthoSize * 1.03f, 0.04f).OnComplete(() => {
//					cam.DOOrthoSize(orthoSize, 0.02f).OnComplete(() => {
//						cam.orthographicSize = orthoSize;
//					});
////					cam.DOOrthoSize(orthoSize * 0.95f, 0.05f).OnComplete(() => {
////						
////					});
//				});

			});
		});

	}


}
