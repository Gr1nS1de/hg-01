using UnityEngine;
using System.Collections;

public class RoadView : View<Game>
{
	private LineRenderer 	_line;
	private RoadModel 		_roadModel;

	public void OnStart (RoadModel roadModel)
	{
		_roadModel = roadModel;

		_line = GetComponent<LineRenderer>();
		_line.SetVertexCount (_roadModel.segments + 2);
		_line.useWorldSpace = false;

		CreatePoints ();

		//_line.material.color = _gameManager.m_CircleColor;
		_line.SetWidth(_roadModel.width,_roadModel.width);
	}

	public void CreatePoints ()
	{
		float angle = 20f;
		float z = 0f;
		float x;
		float y;

		for (int i = 0; i < (_roadModel.segments + 2); i++)
		{
			x = Mathf.Sin (Mathf.Deg2Rad * angle) * _roadModel.radius;
			y = Mathf.Cos (Mathf.Deg2Rad * angle) * _roadModel.radius;

			_line.SetPosition (i,new Vector3(x,y,z) );

			angle += (360f / _roadModel.segments);
		}
	}
}
