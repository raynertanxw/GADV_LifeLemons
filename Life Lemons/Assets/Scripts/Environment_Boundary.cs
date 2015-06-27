using UnityEngine;
using System.Collections;

public class Environment_Boundary : MonoBehaviour
{
	void OnTriggerEnter2D(Collider2D col)
	{
		// If the collided object is a projectile, destroy it.
		if (col.gameObject.tag == Constants.tagProjectile)
		{
			Destroy(col.gameObject);
		}
	}
}