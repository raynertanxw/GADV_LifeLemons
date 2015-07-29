using UnityEngine;
using System.Collections;

public class Tutorial_Enemy_Projectile : MonoBehaviour
{
	public bool destroySelfOnHit;
	private int damage = 1;
	private float ammoVolume = 10.0f;

	void OnTriggerEnter2D(Collider2D other)
	{
		// If collide with player, damage player and destroy self if destroySelfOnHit is set to true.
		if (other.tag == Constants.tagPlayer)
		{
			other.gameObject.GetComponent<Tutorial_Player_Combat>().TakeDamage(damage); // Damage the player.
			if (destroySelfOnHit == true)
			{
				Destroy(gameObject); // Despawn self.
			}
		}
		// If collide with player funnel, add to player ammo levels and then destroy self.
		// Only destroySelfOnHit enemy projectiles can be converted to player ammo.
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
