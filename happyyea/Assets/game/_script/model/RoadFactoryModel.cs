using UnityEngine;
using System.Collections.Generic;

public enum Road
{
	GINGER_MAN = 1
}

public class RoadFactoryModel : Model
{
	public RoadView						roadTemplate				{ get { return _roadTemplate; } set { _roadTemplate = value;} }
	//public Dictionary<int, RoadModel>	roadModelsDictionary		{ get { return _roadModelsDictionary;}}		

	[SerializeField]
	private RoadView					_roadTemplate;
	//private Dictionary<int, RoadModel> 	_roadModelsDictionary 		= new Dictionary<int, RoadModel>();
}
