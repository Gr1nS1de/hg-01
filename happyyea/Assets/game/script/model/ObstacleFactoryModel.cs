using UnityEngine;
using System.Collections.Generic;

public class ObstacleFactoryModel : Model<Game>
{
	public ObstacleView[]							obstacleTemplates			{ get { return _obstacleTemplates; } }
	public Dictionary<ObstacleView, ObstacleModel>	obstacleModelsDictionary	{ get { return _obstacleModelsDictionary;}}		

	[SerializeField]
	private ObstacleView[]							_obstacleTemplates;
	private Dictionary<ObstacleView, ObstacleModel> _obstacleModelsDictionary = new Dictionary<ObstacleView, ObstacleModel>();
}	
