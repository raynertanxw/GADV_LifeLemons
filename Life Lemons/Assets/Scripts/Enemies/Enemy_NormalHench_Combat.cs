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

		int orderNum = ++GameManager.instance.totalNumEnemies; // Cache the totalNumEnemies AFTER incrementing.
		// Set all child sprites to orderNum to avoid sprite odering issues from instances of same prefab.
		for (int i = 0; i < transform.childCount; i++)
		{
			transform.GetChild(i).GetComponent<SpriteRenderer>().sortingOrder = orderNum;
		}
	}

	void Start()
	{
		StartCoroutine(Constants.shootAtPlayer);
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
			// Stop shooting coroutine.
			StopCoroutine(Constants.shootAtPlayer);

			// Update the total enemy count in GameManager.
			GameManager.instance.UpdateEnemyCount();

			// Disable all attached scripts and collider.
			gameObject.GetComponent<Enemy_Movement>().enabled = false;
			gameObject.GetComponent<Collider2D>().enabled = false;

			// Change sprite renderer sorting layer to that of DeadEnemy so player will be rendered on top of them.
			int orderNum = ++GameManager.instance.totalNumEnemies; // Cache the totalNumEnemies AFTER incrementing.
			// Set all child sprites to orderNum to avoid sprite odering issues from instances of same prefab.
			for (int i = 0; i < transform.childCount; i++)
			{
				transform.GetChild(i).GetComponent<SpriteRenderer>().sortingLayerName = Constants.DeadEnemy;
			}

			// Play Death Aniamtion.
			anim.SetTrigger(Constants.Die);

			// Disable self.
			this.enabled = false;
		}
	}

	// For animation to call.
	public void DestroySelf()
	{
		// Destroy the enemy object.
		Destroy(gameObject);
	}
}
