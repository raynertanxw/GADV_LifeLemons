using UnityEngine;
using System.Collections;

public class Enemy_NormalHench_Combat : MonoBehaviour, IDamagable
{
	public int health;
	public GameObject enemyProjectile;
	public Transform projectileSpawn;
	public float rateOfFire;
	public float projectileSpeed;

	private Animator anim;

	void Awake()
	{
		anim = gameObject.GetComponent<Animator>();
	}

	void Start()
	{
		StartCoroutine(shootAtPlayer());
	}

	private IEnumerator shootAtPlayer()
	{
		yield return new WaitForSeconds(rateOfFire);
		while (GameManager.instance.GameOver == false)
		{
			Shoot();
			yield return new WaitForSeconds(rateOfFire);
		}
	}

	private void Shoot()
	{
		// Play shooting animation.
		anim.SetTrigger(Constants.Shoot);

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
			// Update the total enemy count in GameManager.
			GameManager.instance.UpdateEnemyCount();
			// Destroy the enemy object.
			Destroy(gameObject);
		}
	}
}
