using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerModel : Model <Game>
{
	public string                       spriteResourcesPath		{ get { return m_SpriteResourcesPath; } }
	public Gradient						sfLightGradient			{ get { return m_LightGradient; } }
	public PlayerSpriteContainerView	playerContainer			{ get { return m_PlayerSpriteContainer	= m_PlayerSpriteContainer == null ?	SearchGlobal<PlayerSpriteContainerView>(m_PlayerSpriteContainer,	"PlayerSpriteContainer")	: m_PlayerSpriteContainer; } }
	public PlayerSpriteView				player					{ get { return m_PlayerSprite			= m_PlayerSprite == null ?			SearchGlobal<PlayerSpriteView>(			m_PlayerSprite,				"PlayerSprite" )			: m_PlayerSprite; } }
	public Sprite						sprite					{ get { return m_Sprite; } }
	public SFLight						sfLight					{ get { return m_Light; } }
	public float						sfLightDuration			{ get { return m_LightDuration; } }
	public float						deathDuration			{ get { return m_DeathDuration; } }

	public string                       m_SpriteResourcesPath;
	public Gradient						m_LightGradient;
	public PlayerSpriteContainerView    m_PlayerSpriteContainer;
	public PlayerSpriteView				m_PlayerSprite;
	public Sprite						m_Sprite;
	public SFLight						m_Light;
	public float						m_LightDuration;
	public float						m_DeathDuration;
}