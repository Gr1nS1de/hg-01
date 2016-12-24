using UnityEngine;
using System.Collections;

public class RoadModel : Model<Game> 
{
	public int			id					{ get { return _id; } }
	public RoadView		roadView			{ get { return _roadView; } }
	public float		radius				{ get { return _radius; } }
	public float		width				{ get { return _width;}}
	public int			segments			{ get { return _segments;}}

	[SerializeField]
	private int			_id;
	[SerializeField]
	private RoadView	_roadView;
	[SerializeField]
	private float       _radius;
	[SerializeField]
	private float       _width;
	[SerializeField]
	private int			_segments;

}
