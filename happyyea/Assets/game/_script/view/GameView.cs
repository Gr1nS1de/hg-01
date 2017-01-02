﻿using UnityEngine;
using System.Collections;

public class GameView : View
{
	public CameraView					cameraView					{ get { return _cameralView = SearchLocal<CameraView>(								_cameralView,				"CameraView" ); } }
	public RoadView						roadView					{ get { return _roadView = SearchLocal<RoadView>(									_roadView,					"RoadView" ); } }
	public ObstacleView					obstacleView				{ get { return _obstacleView = SearchLocal<ObstacleView>(							_obstacleView,				"ObstacleView" ); } }
	public PlayerView					playerView					{ get { return _playerView = SearchLocal<PlayerView>(								_playerView,				"PlayerView" ); } }
	public PlayerSpriteContainerView	playerSpriteContainerView	{ get { return _playerSpriteContainerView = SearchLocal<PlayerSpriteContainerView>(	_playerSpriteContainerView,	"PlayerSpriteContainerView" ); } }
	public PlayerSpriteView				playerSpriteView			{ get { return _playerSpriteView = SearchLocal<PlayerSpriteView>(					_playerSpriteView,			"PlayerSprite"); } }
	public RotatableComponent			rotatableComponent			{ get { return _rotatableComponent = SearchLocal<RotatableComponent>(				_rotatableComponent,		"RotatableComponent"); } }

	private CameraView					_cameralView;
	private RoadView					_roadView;
	private ObstacleView				_obstacleView;
	private PlayerView					_playerView;
	private PlayerSpriteContainerView	_playerSpriteContainerView;
	private PlayerSpriteView			_playerSpriteView;
	private RotatableComponent          _rotatableComponent;

}