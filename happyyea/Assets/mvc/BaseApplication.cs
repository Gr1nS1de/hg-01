﻿using UnityEngine;
using System.Collections;
using DG.Tweening;

public abstract class BaseApplication<G, U> : BaseApplication
	where G : Element
	where U : Element
{
	public G game			{ get { return (G)(object)m_Game; } }
	public U ui				{ get { return (U)(object)m_UI; } }
}

public abstract class BaseApplication : Element
{

	private void Awake()
	{
		InitTweening ();
		Notify( N.RCStartLoad, Road.GINGER_MAN );
	}

	private void Start()
	{		
		
		Notify(N.GameStart);
	}
		
	public void Notify( string alias, Object target, params object[] data )
	{

		Traverse( delegate ( Transform it )
		{
			Controller[] list = it.GetComponents<Controller>();

			for ( int i = 0; i < list.Length; i++ )
			{
				list[i].OnNotification( alias, target, data );
			}

			return true;
		} );
	}

	private void InitTweening()
	{
		if (Time.realtimeSinceStartup < 1)
			DOTween.KillAll();		
	}
}