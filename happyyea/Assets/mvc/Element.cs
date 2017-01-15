﻿using UnityEngine;
using System.Collections.Generic;

public abstract class Element<T> : Element
{
	public GameApplication 	game 			{ get { return (GameApplication)m_Game; } }
	public UIApplication 	ui				{ get { return (UIApplication)m_UI; } }

}

public abstract class Element : MonoBehaviour
{
	public GameApplication				m_Game			{ get { return _game 	= !_game ? 	SearchGlobal<GameApplication>(	_game, 		"GameApplication") : 	_game; } }
	public UIApplication				m_UI			{ get { return _ui 		= !_ui ? 	SearchGlobal<UIApplication> (	_ui, 		"UIApplication") : 	_ui;}}
	public Dictionary<string, object>	m_Storage		{ get { return _storage == null ? _storage = new Dictionary<string, object>() : _storage; } }
	public GameObject					dynamic_objects	{ get { return _dynamic_objects = _dynamic_objects ? _dynamic_objects : GameObject.FindGameObjectWithTag ("dynamic_objects"); }}

	private GameApplication				_game;
	private UIApplication 				_ui;
	private Dictionary<string, object> 	_storage;
	private GameObject 					_dynamic_objects;

	public void Notify( string alias, params object[] data ) { m_Game.Notify( alias, this, data ); m_UI.Notify ( alias, this, data ); }
	public void NotifyNextFrame( string alias, params object[] data ) { m_Game.NotifyNextFrame( alias, this, data ); m_UI.NotifyNextFrame ( alias, this, data ); }

	public T SearchGlobal<T> (T obj, string storeKey = "", bool update = false ) where T : Object
	{
		/*
		if (obj)
			Debug.Log ("Start search: " + obj.name + " SK = " + storeKey);
		else
			Debug.Log ("Store key = " + storeKey);
		*/
		
		if (m_Storage.ContainsKey (storeKey) && storeKey != "" && !update)
			if ((T)m_Storage [storeKey] != null)
			{
				return (T)m_Storage [storeKey];
			}
			else
			{
				m_Storage.Remove (storeKey);
			}

		var searchFor = GameObject.FindObjectOfType<T>();

		if ( searchFor && storeKey != "" )
		{
			if ( update && m_Storage.ContainsKey( storeKey ) )
				m_Storage.Remove(storeKey);

			m_Storage.Add( storeKey, searchFor );
		}

		//Debug.Log ("Return " + searchFor);

		return searchFor;
	}

	public T[] SearchGlobal<T>( T[] obj, string storeKey = "", bool update = false ) where T : Object
	{
		if (m_Storage.ContainsKey (storeKey) && storeKey != "" && !update)
			if ((T)m_Storage [storeKey] != null)
			{
				return (T[])m_Storage [storeKey];
			}
			else
			{
				m_Storage.Remove (storeKey);
			}

		var searchFor = GameObject.FindObjectsOfType<T>();

		if ( searchFor.Length > 0 && storeKey != "" )
		{
			if ( update && m_Storage.ContainsKey( storeKey ) )
				m_Storage.Remove( storeKey );

			m_Storage.Add( storeKey, searchFor );
		}

		return searchFor;
	}

	public T SearchLocal<T>( T obj, string storeKey = "", bool update = false ) where T : Object
	{
		if (m_Storage.ContainsKey (storeKey) && storeKey != "" && !update)
			if ((T)m_Storage [storeKey] != null)
			{
				return (T)m_Storage [storeKey];
			}
			else
			{
				m_Storage.Remove (storeKey);
			}

		var searchFor = transform.GetComponent<T>() ? transform.GetComponent<T>() : transform.GetComponentInChildren<T>();

		if ( searchFor && storeKey != "" )
		{
			if ( update && m_Storage.ContainsKey( storeKey ) )
				m_Storage.Remove( storeKey );

			m_Storage.Add( storeKey, searchFor );
		}

		return searchFor;
	}

	public T[] SearchLocal<T>( T[] obj, string storeKey = "", bool update = false ) where T : Object
	{
		if (m_Storage.ContainsKey (storeKey) && storeKey != "" && !update)
			if ((T)m_Storage [storeKey] != null)
			{
				return (T[])m_Storage [storeKey];
			}
			else
			{
				m_Storage.Remove (storeKey);
			}

		var searchFor = transform.GetComponents<T>().Length > 0 ? transform.GetComponents<T>() : transform.GetComponentsInChildren<T>();

		if ( searchFor.Length > 0 && storeKey != "" )
		{
			if ( update && m_Storage.ContainsKey( storeKey ) )
				m_Storage.Remove( storeKey );

			m_Storage.Add( storeKey, searchFor );
		}

		return searchFor;
	}

	public void Traverse( System.Predicate<Transform> callback )
	{
		OnTraverseStep( transform, callback );
	}

	private void OnTraverseStep( Transform target, System.Predicate<Transform> callback )
	{
		if ( target )
			if ( !callback( target ) )
				return;

		for ( int i = 0; i < target.childCount; i++ )
			OnTraverseStep( target.GetChild( i ), callback );
	}

	public T Cast<T>() { return (T)(object)this; }
}
