using UnityEngine;
using System.Collections.Generic;

public class ObstacleFactoryModel : Model
{
	public ObstacleView[]								obstacleTemplates				{ get { return _obstacleTemplates; } set { _obstacleTemplates = value; }}
	public Dictionary<ObstacleView, ObstacleModel>		obstacleModelsDictionary		{ get { return _obstacleModelsDictionary;}}		
	public ObstacleView[]								hardObstacleTemplates			{ get { return System.Array.FindAll(obstacleTemplates, o => o.GetComponent<ObstacleModel>().state == ObstacleState.HARD);}}
	public ObstacleView[]								destructibleObstacleTemplates	{ get { return System.Array.FindAll(obstacleTemplates, o => o.GetComponent<ObstacleModel>().state == ObstacleState.DESTRUCTIBLE);}}
	public Dictionary<ObstacleState, ObstacleView[]>  	obstacleTemplatesDictionary 	{ get { _obstacleTemplatesDictionary.Add (ObstacleState.HARD, hardObstacleTemplates); _obstacleTemplatesDictionary.Add (ObstacleState.DESTRUCTIBLE, destructibleObstacleTemplates);  return _obstacleTemplatesDictionary ; } }

	[SerializeField]
	private ObstacleView[]								_obstacleTemplates;
	private Dictionary<ObstacleView, ObstacleModel> 	_obstacleModelsDictionary 		= new Dictionary<ObstacleView, ObstacleModel>();
	private Dictionary<ObstacleState, ObstacleView[]>	_obstacleTemplatesDictionary	= new Dictionary<ObstacleState, ObstacleView[]>();

}	
