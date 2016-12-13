using UnityEngine;
using System.Collections;

public class RotatableComponent : View<Game>
{
	public float m_Speed;

	private int i = 0;

	void FixedUpdate ()
	{
		if ( i-- < 360 )
			transform.Rotate(0f, 0f, i);
	}
}
