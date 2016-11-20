using UnityEngine;
using System.Collections;
using SplatterSystem.Platformer;
using SplatterSystem;

public class PlayerSplatter : MonoBehaviour
{
    public AbstractSplatterManager  m_Splatter;
    public bool                     m_UsePaint = true;
    public float                    m_PaintTimeout = 0.05f;
    public float                    m_PaintPositionOffset = 0;
    private float                   m_LastSplatterTime;


    private GroundState             _groundState;
    private Player                  _player;

    void Awake()
    {
        _groundState = new GroundState(GetComponent<Player>().m_PlayerSpriteRenderer.gameObject);
        _player = GetComponent<Player>();
        m_LastSplatterTime = Time.time;
    }

    void Update()
    {
        // Paint.
        if (m_UsePaint && (Time.time - m_LastSplatterTime > m_PaintTimeout) && _groundState.IsTouching() && _player.m_PositionState == Player.PositionState.ON_INNER_CIRCLE)
        {
            m_LastSplatterTime = Time.time;

            if (_groundState.raycastDown)
            {
                m_Splatter.Spawn(_groundState.raycastDown.point + Vector2.down * m_PaintPositionOffset, Vector3.down);
            }
            if (_groundState.raycastUp)
            {
                m_Splatter.Spawn(_groundState.raycastUp.point + Vector2.up * m_PaintPositionOffset, Vector3.up);
            }
            if (_groundState.raycastLeft)
            {
                m_Splatter.Spawn(_groundState.raycastLeft.point + Vector2.left * m_PaintPositionOffset, Vector3.left);
            }
            if (_groundState.raycastRight)
            {
                m_Splatter.Spawn(_groundState.raycastRight.point + Vector2.right * m_PaintPositionOffset, Vector3.right);
            }

        }
    }
}
