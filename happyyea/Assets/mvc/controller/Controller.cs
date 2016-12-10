using UnityEngine;
using System.Collections;

public abstract class Controller<T> : Controller where T : BaseApplication
{
	public T game { get { return (T)m_Game; } }
}

public abstract class Controller : Element
{
	virtual public void OnNotification( string alias, Object target, params object[] data ) { }
}
