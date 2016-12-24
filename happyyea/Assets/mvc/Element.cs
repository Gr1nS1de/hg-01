using UnityEngine;
using System.Collections.Generic;

public abstract class Element<T> : Element where T : BaseApplication
{
	public T game { get { return (T)m_Game;}}
}

public abstract class Element : MonoBehaviour
{
	public BaseApplication				m_Game			{ get { return _game = !_game ? SearchGlobal<BaseApplication>(_game) : _game; } }
	public Dictionary<string, object>	m_Storage		{ get { return _storage == null ? _storage = new Dictionary<string, object>() : _storage; } }

	private BaseApplication				_game;
	private Dictionary<string, object> _storage;

	public void Notify( string alias, params object[] data ) { m_Game.Notify( alias, this, data ); }

	public T SearchGlobal<T> (T obj, string storeKey = "", bool update = false ) where T : Object
	{
		if (obj)
			Debug.Log ("Start search: " + obj.name + " SK = " + storeKey);
		else
			Debug.Log ("Store key = " + storeKey);
		if ( m_Storage.ContainsKey( storeKey ) && storeKey != "" && !update )
			return (T)m_Storage[storeKey];

		var searchFor = GameObject.FindObjectOfType<T>();

		if ( searchFor && storeKey != "" )
		{
			if ( update && m_Storage.ContainsKey( storeKey ) )
				m_Storage.Remove(storeKey);

			m_Storage.Add( storeKey, searchFor );
		}

		Debug.Log ("Return " + searchFor);

		return searchFor;
	}

	public T[] SearchGlobal<T>( T[] obj, string storeKey = "", bool update = false ) where T : Object
	{
		if ( m_Storage.ContainsKey( storeKey ) && storeKey != "" )
			return (T[])m_Storage[storeKey];

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
		if ( m_Storage.ContainsKey( storeKey ) && storeKey != "" && !update )
			return (T)m_Storage[storeKey];

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
		if ( m_Storage.ContainsKey( storeKey ) && storeKey != "" && !update )
			return (T[])m_Storage[storeKey];

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
