using UnityEngine;
using System.Collections;

public class RoadModel : Model
{
	public Road			alias				{ get { return _alias; } }
	public float		radius				{ get { return _radius; } }
	public float		width				{ get { return _width;}}
	public int			segments			{ get { return _segments;}}

	[SerializeField]
	private Road		_alias;
	[SerializeField]
	private float       _radius;
	[SerializeField]
	private float       _width;
	[SerializeField]
	private int			_segments;

}
