using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class PlayerController : Controller<Game>
{
	
	public override void OnNotification( string alias, Object target, params object[] data )
	{
		switch ( alias )
		{
			case N.GameStart:
				{
					OnStart();

					break;
				}
		}
	}

	private void OnStart()
	{

	}

	public void InitPlayer()
	{
		transform.DORotate(new Vector3(0,0,-360f), 10, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1,LoopType.Incremental);
	}
		
}