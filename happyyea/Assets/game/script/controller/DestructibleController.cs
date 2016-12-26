using UnityEngine;
using System.Collections.Generic;
using Destructible2D;

public class DestructibleController : Controller
{
	private DestructibleModel _destructibleModel	{ get { return game.model.destructibleModel; } }

	public override void OnNotification (string alias, Object target, params object[] data)
	{
		switch (alias)
		{
			case N.GameStart:
				{
					OnStart ();

					break;
				}

			case N.GameBreakEntity:
				{
					var obstacleDestructible = (D2dDestructible)data [0];
					var fractureCount = (int)data [1];
					var collisionPoint = (Vector2)data [2];

					BreakEntity( obstacleDestructible, fractureCount, collisionPoint);

					break;
				}

		}
	}

	private void OnStart()
	{
	}

	public void BreakEntity( D2dDestructible destructible, int fractureCount, Vector2 collisionPoint)
	{

		// Store explosion point (used in OnEndSplit)
		if (collisionPoint == Vector2.zero)
			_destructibleModel.entityBreakPoint = destructible.transform.position;
		else
			_destructibleModel.entityBreakPoint = collisionPoint;

		destructible.transform.tag = "Untagged";

		if( destructible.GetComponentInChildren<D2dCollider>() )
			destructible.GetComponentInChildren<D2dCollider>().m_SpriteChildCollider.tag = "Untagged";

		// Register split event
		destructible.OnEndSplit.AddListener(OnEndSplit);

		// Split via fracture
		D2dQuadFracturer.Fracture(destructible, fractureCount, 0.5f);

		// Unregister split event
		destructible.OnEndSplit.RemoveListener(OnEndSplit);
	}

	private void OnEndSplit(List<D2dDestructible> clones)
	{
		// Go through all clones in the clones list
		for (var i = clones.Count - 1; i >= 0; i--)
		{
			var clone = clones[i];
			var rigidbody = clone.GetComponent<Rigidbody2D>();

			// Does this clone have a Rigidbody2D?
			if (rigidbody != null)
			{
				// Get the local point of the explosion that called this split event
				var localPoint = (Vector2)clone.transform.InverseTransformPoint(_destructibleModel.entityBreakPoint);

				// Get the vector between this point and the center of the destructible's current rect
				var vector = clone.AlphaRect.center - localPoint;

				var force = ( game.model.gameState == GameState.GAMEOVER ? game.model.playerModel.breakForce : game.model.destructibleModel.breakForce );

				// Apply relative force
				rigidbody.AddRelativeForce(vector * force, ForceMode2D.Impulse);
			}
		}
	}

}
