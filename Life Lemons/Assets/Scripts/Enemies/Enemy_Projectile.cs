using UnityEngine;
using System.Collections;

public class Enemy_Projectile : MonoBehaviour
{
	private int damage = 1;
	private float ammoVolume = 10.0f;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			other.gameObject.GetComponent<Player_Combat>().TakeDamage(damage); // Damage the player.
			Destroy(gameObject); // Despawn self.
		}
		else if (other.name == "Lemonator_Funnel")
		{
			if (other.gameObject.GetComponentInParent<Player_Combat>().playerState == PlayerStates.collect)
			{
				other.gameObject.GetComponentInParent<Player_Combat>().CollectAmmo(ammoVolume); // Add ammo to the player.
				Destroy(gameObject); // Despawn self.
			}
		}
	}
}
