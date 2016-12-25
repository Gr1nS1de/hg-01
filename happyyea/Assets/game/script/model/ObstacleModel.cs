using UnityEngine;
using System.Collections;

public enum ObstacleState
{
	HARD,
	DESTRUCTIBLE
}

public class ObstacleModel : Model<Game>
{
	public ObstacleState	state				{ get { return _state;}}

	[SerializeField]
	private ObstacleState	_state;

}
