using UnityEngine;
using System.Collections;

public class Tutorial_Enemy_Projectile : MonoBehaviour
{
	public bool destroySelfOnHit;
	private int damage = 1;
	private float ammoVolume = 10.0f;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			other.gameObject.GetComponent<Tutorial_Player_Combat>().TakeDamage(damage); // Damage the player.
			if (destroySelfOnHit == true)
			{
				Destroy(gameObject); // Despawn self.
			}
		}
		else if (other.name == "Lemonator_Funnel" && destroySelfOnHit == true)
		{
			if (other.gameObject.GetComponentInParent<Tutorial_Player_Combat>().playerState == PlayerStates.collect)
			{
				other.gameObject.GetComponentInParent<Tutorial_Player_Combat>().CollectAmmo(ammoVolume); // Add ammo to the player.
				Destroy(gameObject); // Despawn self.
			}
		}
	}
}
