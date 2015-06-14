using UnityEngine;
using System.Collections;

public class Enemy_NormalHench_Combat : MonoBehaviour, IDamagable
{
	public int health;
	public GameObject enemyProjectile;
	public Transform projectileSpawn;
	public float rateOfFire;
	public float projectileSpeed;

	//private Animator anim;

	void Start()
	{
		StartCoroutine(shootAtPlayer());
	}

	private IEnumerator shootAtPlayer()
	{
		while (GameManager.instance.GameOver == false)
		{
			yield return new WaitForSeconds(rateOfFire);
			Shoot();
		}
	}

	private void Shoot()
	{
		GameObject projectile = (GameObject)Instantiate(enemyProjectile, projectileSpawn.position, transform.rotation);
		projectile.GetComponent<Rigidbody2D>().velocity = Utilities.RotationZ2DirectionVec(transform.rotation.eulerAngles.z) * projectileSpeed;
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
