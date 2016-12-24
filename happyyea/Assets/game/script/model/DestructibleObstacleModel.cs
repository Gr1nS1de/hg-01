using UnityEngine;
using System.Collections;

public class DestructibleObstacleModel : Model<Game>
{
	public float			breakForce	{ get { return _breakForce;}}

	[SerializeField]
	private float _breakForce;
}
