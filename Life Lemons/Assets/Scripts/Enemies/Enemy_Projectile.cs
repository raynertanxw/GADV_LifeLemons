using UnityEngine;
using System.Collections;

public class Enemy_Projectile : MonoBehaviour
{
	public bool destroySelfOnHit;
	private int damage = 1;
	private float ammoVolume = 10.0f;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == Constants.tagPlayer)
		{
			other.gameObject.GetComponent<Player_Combat>().TakeDamage(damage); // Damage the player.
			if (destroySelfOnHit == true)
			{
				Destroy(gameObject); // Despawn self.
			}
		}
		else if (other.name == "Lemonator_Funnel" && destroySelfOnHit == true)
		{
			if (other.gameObject.GetComponentInParent<Player_Combat>().playerState == PlayerStates.collect)
			{
				other.gameObject.GetComponentInParent<Player_Combat>().CollectAmmo(ammoVolume); // Add ammo to the player.
				Destroy(gameObject); // Despawn self.
			}
		}
	}
}
