using UnityEngine;
using System.Collections.Generic;

public class RoadFactoryModel : Model<Game> 
{
	public RoadView[]					roadTemplates				{ get { return _roadTemplates; } }
	public Dictionary<int, RoadModel>	roadModelsDictionary		{ get { return _roadModelsDictionary;}}		

	[SerializeField]
	private RoadView[]					_roadTemplates;
	private Dictionary<int, RoadModel> 	_roadModelsDictionary = new Dictionary<int, RoadModel>();
}
