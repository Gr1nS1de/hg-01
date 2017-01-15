using UnityEngine;
using System.Collections;
using DG.Tweening;

public class RoadView : View<Game>
{
	private LineRenderer 	_line;
	private RoadModel 		_roadModel;

	public void OnStart (RoadModel roadModel)
	{
		_roadModel = roadModel;

		_line = GetComponent<LineRenderer>();
		_line.useWorldSpace = false;
	}

	public void OnPlayerPlaced()
	{
		Vector3[] drawPoints = game.model.playerModel.playerPath.PathGetDrawPoints(); 
		int pointsCount = drawPoints.Length;

		_line.SetWidth(_roadModel.width,_roadModel.width);
		_line.SetVertexCount(pointsCount);

		for (int i = 0; i < pointsCount; ++i)
			_line.SetPosition(i, drawPoints[i]);

	}
}
