﻿using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CameraView : View<Game>
{
	public CameraModel 		cameraModel		{ get { return game.model.cameraModel; } }
	public Camera 			cam				{ get { return GetComponent<Camera> (); } }
	public Transform		player			{ get { return game.view.playerSpriteContainerView.transform; } }
	public float			playZoomSize	{ get { return cameraModel.playZoomSize; } }

	public void OnStart()
	{
		StartCoroutine(DOFollow());
	}

	void Update()
	{
		cam.backgroundColor = GM.instance.currentBackgroundColor;
	}

	public void DOStart(System.Action callback)
	{
		cam.DOOrthoSize(playZoomSize, 0.7f)
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
			pos.x = player.position.x;
			pos.y = player.position.y;
			transform.position = pos;

			yield return 0;
		}
	}
	/*
	public void DOShake()
	{
		if(DOTween.IsTweening(cam))
			return;

		cam.DOOrthoSize(playZoomSize * 1.03f, 0.002f).OnComplete(() => {
			cam.DOOrthoSize(playZoomSize * 0.97f, 0.004f).OnComplete(() => {
				cam.DOOrthoSize(playZoomSize, 0.002f).OnComplete(() => {
					cam.orthographicSize = playZoomSize;
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
*/
}
