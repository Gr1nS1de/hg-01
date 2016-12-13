using UnityEngine;
using System.Collections;

public abstract class BaseApplication<M, V, C> : BaseApplication
	where M : Element
	where V : Element
	where C : Element
{
	public M model			{ get { return (M)(object)m_Model; } }
	public V view			{ get { return (V)(object)m_View; } }
	public C controller		{ get { return (C)(object)m_Controller; } }
}

public abstract class BaseApplication : Element
{
	public Model		m_Model			{ get { return _model; } }
	public View			m_View			{ get { return _view; } }
	public Controller	m_Controller	{ get { return _controller; } }

	private Model		_model;
	private View        _view;
	private Controller  _controller;

	private void Awake()
	{
		Notify( N.GameLoadResources );
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
				list[i].OnNotification( alias, target, data );

			return true;
		} );
	}
}
