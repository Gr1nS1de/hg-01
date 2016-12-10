using UnityEngine;
using System.Collections;

public abstract class Model<T> : Model where T : BaseApplication
{
	public T game { get { return (T)m_Game; } }
}

public abstract class Model : Element
{

}
