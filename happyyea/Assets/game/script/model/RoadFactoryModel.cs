using UnityEngine;
using System.Collections.Generic;

public class RoadFactoryModel : Model
{
	public RoadView						roadTemplate				{ get { return _roadTemplate; } set { _roadTemplate = value;} }
	//public Dictionary<int, RoadModel>	roadModelsDictionary		{ get { return _roadModelsDictionary;}}		

	[SerializeField]
	private RoadView					_roadTemplate;
	//private Dictionary<int, RoadModel> 	_roadModelsDictionary 		= new Dictionary<int, RoadModel>();
}
