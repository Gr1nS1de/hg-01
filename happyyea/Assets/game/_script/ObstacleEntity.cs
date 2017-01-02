﻿using UnityEngine;
using System;
using System.Collections;
using DG.Tweening;
using Destructible2D;

/// <summary>
/// Attached to ObstaclePrefab prefab in the prefab folder. In charge to initialize the spike and anim them in, and destroy them if ot of screen.
/// </summary>
public class ObstacleEntity : MonoBehaviour 
{
    public enum State
    {
        NORMAL,
        DESTRUCTIBLE,
        _COUNTFLAG
    }

    public State m_State
    {
        get
        {
            return _currentState;
        }

        set
        {
            _currentState = value;
        }
    }

	public GameObject       m_ObstacleObject;
	public bool             m_IsVisible = false;
    
    [HideInInspector]
    public Sprite           m_ObstacleSprite;
    [HideInInspector]
    public SpriteRenderer   m_ObstacleVisibilitySpriteRenderer;

    private GM     _gameManager;
    private float           _decal = -1f;
    private State           _currentState;

	void Awake()
	{
		_gameManager = FindObjectOfType<GM>();
		m_ObstacleObject.SetActive(false);
        m_IsVisible = false;
    }


	public void Init(float rot, bool isDown)
	{
        StopAllCoroutines();
        StartCoroutine(DOStart(rot, isDown));
	}

	IEnumerator DOStart(float rot, bool isDown)
	{
		_decal = m_ObstacleSprite.bounds.size.y * m_ObstacleObject.transform.localScale.y/1.5f;

		float obstacleLastPosition = -1000000f;

		bool isAnimated = false;
      
        while (true)
		{
			
			if(obstacleLastPosition == m_ObstacleObject.transform.localPosition.x && !isAnimated)
			{
				m_ObstacleObject.SetActive(true);

                DOSetupForState();

                isAnimated = true;

				int sign = +1;

				if(isDown)
					sign = -1;

				var currentObstaclePosition = m_ObstacleObject.transform.localPosition;

                m_IsVisible = true;

                m_ObstacleObject.transform.localPosition = new Vector3(currentObstaclePosition.x + sign * 0.2f, currentObstaclePosition.y, currentObstaclePosition.z);
				m_ObstacleObject.transform.DOLocalMoveX(currentObstaclePosition.x, 0.2f).OnComplete(() => {

					StopAllCoroutines();
              
					StartCoroutine(CheckVisibility());
				});
			}

			if(!isAnimated)
			{
				obstacleLastPosition = m_ObstacleObject.transform.localPosition.x;

				DOPosition(new Vector3(0,0,rot), isDown, null);
			}

			yield return 0;
		}
	}

    private void DOSetupForState()
    {
        switch(m_State)
        {
            case State.NORMAL:
                    
                break;

            case State.DESTRUCTIBLE:
                m_ObstacleVisibilitySpriteRenderer.transform.position = m_ObstacleObject.transform.position;
                break;
        }
    }

    #region Templates setup for preload

    public void SetNormalTemplate()
    {
        m_State = State.NORMAL;

        var normSR = m_ObstacleObject.GetComponent<SpriteRenderer>().sprite;//GetComponentInChildren<SpriteRenderer>().sprite;

        normSR = m_ObstacleSprite;

        m_ObstacleVisibilitySpriteRenderer = m_ObstacleObject.GetComponent<SpriteRenderer>();

        m_ObstacleObject.GetComponent<Rigidbody2D>().freezeRotation = true;
    }

    public void SetDestructibleTemplate()
    {
        m_State = State.DESTRUCTIBLE;

        D2dDestructible destructible = null;

        destructible = m_ObstacleObject.AddComponent<D2dDestructible>();
        
        m_ObstacleObject.AddComponent<D2dRetainVelocity>();

        if (m_ObstacleObject.GetComponent<PolygonCollider2D>())
            Destroy(m_ObstacleObject.GetComponent<PolygonCollider2D>());

        D2dPolygonCollider D2d_polygonCollider = m_ObstacleObject.AddComponent<D2dPolygonCollider>();

        D2d_polygonCollider.Detail = 0.5f;
        D2d_polygonCollider.CellSize = 256;

        m_ObstacleVisibilitySpriteRenderer = AddSpriteHandler(gameObject);

        m_ObstacleObject.GetComponent<Rigidbody2D>().isKinematic = false;

        if (destructible.AlphaTex != m_ObstacleSprite)
            destructible.ReplaceWith(m_ObstacleSprite);
    }
    #endregion

    IEnumerator CheckVisibility()
	{
		while(true)
		{
            if (m_ObstacleVisibilitySpriteRenderer)
            {
                if (!m_ObstacleVisibilitySpriteRenderer.isVisible)
                {
                    yield return new WaitForSeconds(1f);

                    switch (m_State)
                    {
                        case State.NORMAL:
                                m_ObstacleObject.SetActive(false);
                                m_IsVisible = false;
                            break;

                        case State.DESTRUCTIBLE:
                                _gameManager.m_ObstacleManager.DODeleteObstacleInstance(gameObject);
                            break;
                    }
                    
                    break;
                }
            }

			yield return null;
		}
        
	}

	public void DOPosition(Vector3 rotation, bool isDown, Action callback)
	{
		var pPosition = FindObjectOfType<Player>().m_PlayerDefaultPosition;

		if(isDown)
		{
			var mPos = new Vector3(pPosition.x - _decal, pPosition.y, pPosition.z);

			m_ObstacleObject.transform.localPosition = mPos;

			mPos = m_ObstacleObject.transform.position;
			mPos.z = 2f;

			m_ObstacleObject.transform.position = mPos;
			m_ObstacleObject.transform.localEulerAngles = new Vector3(0,0,-90);
		}
		else
		{
			var mPos = new Vector3(pPosition.x + _decal, pPosition.y, pPosition.z);

			m_ObstacleObject.transform.localPosition = mPos;

			mPos = m_ObstacleObject.transform.position;
			mPos.z = 2f;

			m_ObstacleObject.transform.position = mPos;
			m_ObstacleObject.transform.localEulerAngles = new Vector3(0,0,+90);
		}

		transform.position = new Vector3(0,0,0);

		transform.eulerAngles = rotation;
        
	}
    
    private SpriteRenderer AddSpriteHandler(GameObject toObject)
    {
        GameObject spriteHandler = new GameObject();

        spriteHandler.transform.position = m_ObstacleObject.transform.position;
        spriteHandler.AddComponent<SpriteRenderer>();

        spriteHandler.name = "VisibilitySpriteHandler";

        spriteHandler.transform.SetParent(toObject.transform);

        return spriteHandler.GetComponent<SpriteRenderer>();
    }
}


//using UnityEngine;
//using System.Collections;
//using DG.Tweening;
//
//public class ObstacleLogic : MonoBehaviourHelper 
//{
//	Circle circle;
//
//	public GameObject m_ObstacleSprite;
//
//	SpriteRenderer sr;
//
//	public bool isVisible = false;
//
//	float decal = -1f;
//
//	void Awake()
//	{
//		circle = FindObjectOfType<Circle>();
//		m_ObstacleSprite.SetActive(false);
//		sr = m_ObstacleSprite.GetComponent<SpriteRenderer>();
//		isVisible = false;
//	}
//
//	public void Init(float rot, bool isDown)
//	{
//		StartCoroutine(DOStart(rot, isDown));
//	}
//
//	IEnumerator DOStart(float rot, bool isDown)
//	{
//		decal = m_ObstacleSprite.GetComponent<SpriteRenderer>().sprite.bounds.size.y * m_ObstacleSprite.transform.localScale.y/1.5f;
//
//		while(true)
//		{
//			DOPosition(new Vector3(0,0,rot), isDown);
//
//			if(!isVisible)
//			{
//				var d = Vector2.Distance(player.sr.transform.position,m_ObstacleSprite.transform.position);
//
//				if(d < 0.30)
//				{
//					isVisible = true;
//					yield return new WaitForSeconds(1f);
//				}
//			}
//			else
//			{
//				if(!sr.isVisible)
//				{
//					yield return new WaitForSeconds(1f);
//					Destroy(gameObject);
//				}
//			}
//			yield return new WaitForSeconds(0.3f);
//		}
//	}
//
//	public void DOPosition(Vector3 rotation, bool isDown)
//	{
//		m_ObstacleSprite.SetActive(true);
//
//		var pPosition = FindObjectOfType<Player>().defaultPosition;
//
//		if(isDown)
//		{
//			var mPos = new Vector3(pPosition.x - decal, pPosition.y, pPosition.z);
//
//			m_ObstacleSprite.transform.localPosition = mPos;
//
//			mPos = m_ObstacleSprite.transform.position;
//			mPos.z = 2f;
//
//			m_ObstacleSprite.transform.position = mPos;
//
//			m_ObstacleSprite.transform.localEulerAngles = new Vector3(0,0,-90);
//		}
//		else
//		{
//			var mPos = new Vector3(pPosition.x + decal, pPosition.y, pPosition.z);
//
//			m_ObstacleSprite.transform.localPosition = mPos;
//
//			mPos = m_ObstacleSprite.transform.position;
//			mPos.z = 2f;
//
//			m_ObstacleSprite.transform.position = mPos;
//
//			m_ObstacleSprite.transform.localEulerAngles = new Vector3(0,0,+90);
//		}
//
//		transform.position = new Vector3(0,0,0);
//
//		transform.eulerAngles = rotation;
//
//		m_ObstacleSprite.GetComponent<SpriteRenderer>().color = Camera.main.backgroundColor;
//	}
//}