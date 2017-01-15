﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Destructible2D;
using DG.Tweening;

public enum PlayerPositionState
{
	ON_CIRCLE,
	OUT_CIRCLE
}
	
public class PlayerModel : Model
{
	public Gradient						sfLightGradient			{ get { return _lightGradient; } }	
	public D2dDestructible				playerDestructible		{ get { return game.view.playerSpriteView.GetComponent<D2dDestructible> ();}}
	public Sprite						currentSprite			{ get { return _currentSprite; } 		set { _currentSprite = value; playerDestructible.ReplaceWith( _currentSprite ); } }
	public Sprite[]						sprites					{ get { return _sprites; } 				set { _sprites = value;}}
	//public SFLight					sfLight					{ get { return m_Light; } }
	//public float						sfLightDuration			{ get { return m_LightDuration; } }
	public float						deathDuration			{ get { return _deathDuration; } }
	public PlayerPositionState			positionState			{ get { return _positionState; } 		set { _positionState = value; } }
	public float 						jumpWidth				{ get { return _jumpWidth 				= game.model.currentRoadModel.width / 2f - currentSprite.bounds.size.x * 0.5f * game.view.playerSpriteView.transform.localScale.x; } }
	public float						jumpDuration			{ get { return _jumpDuration;} 			set { _jumpDuration = value; }}
	public float						speed					{ get { return _speed; } 				set { _speed = value; } }
	public float						breakForce				{ get { return _breakForce; } }
	public ParticleSystem				particleTrace			{ get { return game.view.playerTraceView.GetComponent<ParticleSystem> ();}}
	public Tweener						playerPath				{ get { return _playerPath;} set { _playerPath = value;}}

	[SerializeField]
	private Gradient					_lightGradient;
	[SerializeField]
	private Sprite						_currentSprite;
	[SerializeField]
	private Sprite[]					_sprites;
//	private SFLight						m_Light;
//	private float						m_LightDuration;
	[SerializeField]
	private float						_deathDuration;
	[SerializeField]
	private PlayerPositionState 		_positionState;
	private float						_jumpWidth;
	[SerializeField]
	private float						_jumpDuration;
	[SerializeField]
	private float						_speed;
	[SerializeField]
	private float						_breakForce;
	private Tweener 					_playerPath;
}
	