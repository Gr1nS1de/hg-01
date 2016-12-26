using UnityEngine;
using System.Collections;

public class RoadModel : Model
{
	public int			id					{ get { return _id; } }
	public float		radius				{ get { return _radius; } }
	public float		width				{ get { return _width;}}
	public int			segments			{ get { return _segments;}}

	[SerializeField]
	private int			_id;
	[SerializeField]
	private float       _radius;
	[SerializeField]
	private float       _width;
	[SerializeField]
	private int			_segments;

}
