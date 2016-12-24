using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Destructible2D;
using DG.Tweening;

public class PlayerModel : Model <Game>
{
	public string                       spriteResourcesPath		{ get { return _spriteResourcesPath; } }
	public Gradient						sfLightGradient			{ get { return _lightGradient; } }	
	public PlayerView					playerView				{ get { return _playerView				= _playerView == null ?				SearchGlobal<PlayerView>(				_playerView,				"Player")					: _playerView; } }
	public PlayerSpriteContainerView	playerSpriteContainer	{ get { return _playerSpriteContainer	= _playerSpriteContainer == null ?	SearchGlobal<PlayerSpriteContainerView>(_playerSpriteContainer,		"PlayerSpriteContainer")	: _playerSpriteContainer; } }
	public PlayerSpriteView				playerSpriteView		{ get { return _playerSpriteView		= _playerSpriteView == null ?		SearchGlobal<PlayerSpriteView>(			_playerSpriteView,			"PlayerSprite" )			: _playerSpriteView; } }
	public D2dDestructible				playerDestructible		{ get { return playerSpriteView.GetComponent<D2dDestructible> ();}}
	public Sprite						currentSprite			{ get { return _currentSprite; } set { _currentSprite = value; playerDestructible.ReplaceWith( _currentSprite ); } }
	public Sprite[]						sprites					{ get { return _sprites; } set { _sprites = value;}}
	//	public SFLight						sfLight					{ get { return m_Light; } }
//	public float						sfLightDuration			{ get { return m_LightDuration; } }
	public float						deathDuration			{ get { return _deathDuration; } }
	public PlayerPositionState			positionState			{ get { return _positionState; } set { _positionState = value; } }
	public float 						jumpWidth				{ get { return _jumpWidth = game.model.currentRoadModel.width / 2f - currentSprite.bounds.size.x * 0.5f * playerSpriteView.transform.localScale.x; } }
	public float						jumpSpeed				{ get { return _jumpSpeed;} set { _jumpSpeed = value; }}
	public float						speed					{ get { return _speed; } set { _speed = value; } }
	public float						breakForce				{ get { return _breakForce;}}

	[SerializeField]
	private string                      _spriteResourcesPath;
	[SerializeField]
	private Gradient					_lightGradient;
	[SerializeField]
	private PlayerView 					_playerView;
	private PlayerSpriteContainerView   _playerSpriteContainer;
	private PlayerSpriteView			_playerSpriteView;
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
	private float						_jumpSpeed;
	[SerializeField]
	private float						_speed;
	[SerializeField]
	private float						_breakForce;
}

public enum PlayerPositionState
{
	ON_CIRCLE,
	OUT_CIRCLE
}

