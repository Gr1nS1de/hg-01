using UnityEngine;
using System.Collections;

public enum ObstacleState
{
	HARD,
	DESTRUCTIBLE
}

public class ObstacleModel : Model
{

	public ObstacleModel(){}

	public ObstacleState	state				{ get { return _state; } }
	public ObstacleView		obstacleView		{ get { return _obstacleView;} 	set { _obstacleView = value;}}
	public Vector3			spriteSize			{ get { return _spriteSize; } 	set { _spriteSize = value; } }

	[SerializeField]
	private ObstacleState	_state;
	private ObstacleView	_obstacleView;
	private Vector3			_spriteSize;

}
