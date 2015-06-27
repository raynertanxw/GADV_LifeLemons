using UnityEngine;
using System.Collections;

public class Player_Projectile : MonoBehaviour
{
	private int damage = 1;
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == Constants.tagEnemy)
		{
			other.gameObject.GetComponent<IDamagable>().TakeDamage(damage); // Damage the enemy.
			Destroy(gameObject); // Despawn self.
		}
	}
}
