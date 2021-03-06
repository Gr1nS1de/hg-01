﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Class attached to the sprite child of the Player GameOBject, in charge to listen if the player collide with an obstacle
/// </summary>
public class CollisionDetection : MonoBehaviour
{
	/// <summary>
	/// Listen the collision. If collision: all the Player method DOOnTriggerEnter2D
	/// </summary>
	public void OnTriggerEnter2D(Collider2D other)
	{
        if (other.tag == "Obstacle")
        {
            Debug.Log("Triggered");
            //FindObjectOfType<Player>().DOOnImpactEnter2D(other.GetComponentInParent<ObstacleEntity>(), Vector2.zero);
        }
	}

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Obstacle")
        {
            Debug.Log("Collised");
            //Debug.Break();

        }
        if (other.transform.tag == "Obstacle")
            FindObjectOfType<Player>().DOOnImpactEnter2D(other.transform.GetComponentInParent<ObstacleEntity>(), other.contacts[0].point);
    }
}
