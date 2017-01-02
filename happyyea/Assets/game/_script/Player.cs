﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

/// <summary>
/// Class attached to the Player GameObject in the hierarchy. In charge to handle the Player the player controls, detect touch and collision.
/// </summary>
public class Player : MonoBehaviour
{ 
    public enum PositionState
    {
        ON_CIRCLE,
        OUT_CIRCLE
    }

    public PositionState m_PositionState
    {
        get
        {
            return _positionState;
        }

        set
        {
            _positionState = value;
        }
    }

    public Gradient         m_LightGradient;
	public Transform        m_PlayerTransformContainer;
    public Transform        m_PlayerTransform;
	public Sprite           m_PlayerSprite;
    public SFLight          m_CircleSFLight;
    public float            m_CircleSFLightGradientCycleDuration;
    public float            m_DeathDuration;

    [HideInInspector]
	public Vector3          m_PlayerDefaultPosition = Vector3.zero;

    private GM				_gameManager;
    private PositionState   _positionState;

    public float jumpWidth
	{
		get
		{
			return FindObjectOfType<RoadCircle>().GetWidth()/2f - m_PlayerSprite.bounds.size.x * 0.5f * m_PlayerTransform.localScale.x;		
		}
	}

	void Awake()
	{
		_gameManager = FindObjectOfType<GM>();
		//m_PlayerSprite.color = _gameManager.m_PlayerColor;
	}

	void Start()
	{
		DOStart();
	}

    void Update()
    {
        float t = Mathf.PingPong( Time.time / m_CircleSFLightGradientCycleDuration, 1f );
        m_CircleSFLight.color = m_LightGradient.Evaluate( t );
    }

	public float GetRotation()
	{
		return transform.eulerAngles.z;
	}

	public void DOStartPosition(float x, float y)
	{
		m_PlayerDefaultPosition = new Vector3(x,y,2f);
		m_PlayerTransformContainer.position = m_PlayerDefaultPosition;
		m_PlayerTransform.localPosition = new Vector3(-jumpWidth, 0, 0);

        m_PositionState = PositionState.ON_CIRCLE;
	}

	public void DOStart()
	{
		transform.DORotate(new Vector3(0,0,-360f), 10, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1,LoopType.Incremental);
	}

	public void ActivateTouchControl()
	{
		InputTouch.OnTouchedDown += OnTouchDown;
	}

	public void DesactivateTouchControl()
	{
		InputTouch.OnTouchedDown -= OnTouchDown;
	}

	void OnTouchDown (TouchDirection td)
	{
        switch(m_PositionState)
        {
            case PositionState.ON_CIRCLE:
                    DOPlayerJump(PositionState.OUT_CIRCLE);
                break;

            case PositionState.OUT_CIRCLE:
                    DOPlayerJump(PositionState.ON_CIRCLE);
                break;
        }

	}

    private void DOPlayerJump(PositionState positionState)
    { 
        switch(positionState)
        {
            case PositionState.ON_CIRCLE:
                {
                    var v = new Vector3(-jumpWidth, 0, 0);

                    m_PlayerTransform.DOLocalMove(v, 0.3f)
                        .OnComplete(OnCompleteJump);

                    m_PositionState = PositionState.ON_CIRCLE;
                }
                break;

            case PositionState.OUT_CIRCLE:
                {
                    var v = new Vector3(+jumpWidth, 0, 0);

                    m_PlayerTransform.DOLocalMove(v, 0.3f)
                        .OnComplete(OnCompleteJump);

                    m_PositionState = PositionState.OUT_CIRCLE;
                }
                break;
        }
    }

	void OnCompleteJump()
	{
		if(DOTween.IsTweening(Camera.main))
			return;
		
		//_gameManager.Add1Point();
		FindObjectOfType<CameraManager>().DOShake();
	}

	public void AnimPlayer(float targetPos)
	{
		StopAllCoroutines();

		if(_gameManager.m_GameStatus == GM.GameStatus.GAMEOVER)
			return;
		
		StartCoroutine(_AnimPlayer(targetPos));
	}

	public IEnumerator _AnimPlayer(float targetPos)
	{
		yield return 0;
	}

	public void DOOnImpactEnter2D(ObstacleEntity obstacleEntity, Vector2 collisionPoint)
	{
		FindObjectOfType<GM>().OnImpactObstacleByPlayer(obstacleEntity.gameObject, collisionPoint);
	}

}