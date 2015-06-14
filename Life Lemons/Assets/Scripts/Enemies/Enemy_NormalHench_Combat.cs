using UnityEngine;
using System.Collections;

public class Enemy_NormalHench_Combat : MonoBehaviour, IDamagable
{
	public int health;
	public GameObject enemyProjectile;
	public float rateOfFire;

	//private Animator anim;
	private const float ammoSpriteMaxScale = 0.25f;
	private float projectileSpeed = 50.0f;

	void Start()
	{
		StartCoroutine(shootAtPlayer());
	}

	private IEnumerator shootAtPlayer()
	{
		while (true)
		{
			yield return new WaitForSeconds(rateOfFire);
			Shoot();
		}
	}

	private void Shoot()
	{
		Debug.Log("Shoot");
	}

	// For Player Projectiles to call.
	public void TakeDamage(int damage)
	{
		health -= damage;
		CheckGameOver();
	}
	
	public void CheckGameOver()
	{
		if (health == 0)
		{
			// Destroy the enemy object.
			Destroy(gameObject);
		}
	}
}
