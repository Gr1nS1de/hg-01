using UnityEngine;
using System.Collections;

public enum ObstacleState
{
	HARD,
	DESTRUCTIBLE
}

public class ObstacleModel : Model
{
	public ObstacleState	state				{ get { return _state;}}
	public ObstacleView		obstacleView		{ get { return _obstacleView;} set { _obstacleView = value;}}

	[SerializeField]
	private ObstacleState	_state;
	private ObstacleView	_obstacleView;

}
