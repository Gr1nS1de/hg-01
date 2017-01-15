using UnityEngine;
using System.Collections.Generic;

public enum Road
{
	GINGERBREAD_MAN = 1,
	SNOWMAN			= 2
}

[System.Serializable]
public class RoadBundle
{
	public Road 			roadAlias;
	public Vector3[]		roadWaypoints;
}

public class RoadFactoryModel : Model
{
	public RoadView[]			roadTemplates		{ get { return _roadTemplates; } set { _roadTemplates = value;} }
	public float				roadsGapLength		{ get { return _roadsGapLength; } }
	public RoadBundle[]			roadBundles			{ get { return _roadBundles;}}

	[SerializeField]
	private RoadView[]			_roadTemplates 		= new RoadView[System.Enum.GetNames(typeof(Road)).Length];
	[SerializeField]
	private float				_roadsGapLength;
	[SerializeField]
	private RoadBundle[]		_roadBundles		= new RoadBundle[]{
		new RoadBundle{
		roadAlias = Road.GINGERBREAD_MAN,
		roadWaypoints = new[] { new Vector3(2.470269f,-1.790608f,10f), new Vector3(1.01779f,-2.800275f,10f), new Vector3(-0.7606676f,-2.957198f,10f), new Vector3(-2.295023f,-1.945919f,10f), new Vector3(-2.957585f,-0.6207935f,10f), new Vector3(-2.835534f,1.105356f,10f), new Vector3(-1.580153f,2.552534f,10f), new Vector3(-0.01092541f,3.005865f,10f), new Vector3(1.697787f,2.482789f,10f), new Vector3(2.883425f,0.9484344f,10f) }
		},
		new RoadBundle
		{
			roadAlias = Road.SNOWMAN,
			roadWaypoints = new[] {new Vector3(2.470269f,-1.790608f,10f)}
		}
	};

}
