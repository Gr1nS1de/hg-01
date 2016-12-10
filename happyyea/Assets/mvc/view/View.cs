using UnityEngine;
using System.Collections;

public abstract class View<T> : View where T : BaseApplication
{
	public T game { get { return (T)m_Game; } }
}

public abstract class View : Element
{

}



