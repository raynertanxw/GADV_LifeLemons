using UnityEngine;
using System.Collections;

public class Tutorial_NormalHenchCombat : MonoBehaviour, IDamagable
{
	public bool canShoot = false;

	public int health;
	public GameObject enemyProjectile;
	public Transform projectileSpawn;
	public float rateOfFire;
	public float projectileSpeed;
	public int numQuarters;
	public GameObject quarter;
	
	private Animator anim;
	private float quarterSpawnPosOffset = 1.0f;
	
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
		while (canShoot == false)
		{
			yield return null;
		}

		while (true)
		{	
			if (canShoot == true)
			{
				Shoot();
			}
			yield return new WaitForSeconds(rateOfFire);
		}
	}
	
	public void Shoot()
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

	// For animation to call.
	public void DestroySelf()
	{
		// Destroy the enemy object.
		Destroy(gameObject);
	}

	// For animation to call.
	public void SpawnQuarters()
	{
		for (int i = 0; i < numQuarters; i++)
		{
			Vector3 offSet = new Vector3(Random.Range(-quarterSpawnPosOffset, quarterSpawnPosOffset), Random.Range(-quarterSpawnPosOffset, quarterSpawnPosOffset) ,0.0f);
			Instantiate(quarter, transform.position + offSet, Quaternion.identity);
		}
	}
	
	public void CheckGameOver()
	{
		if (health == 0)
		{
			// Stop shooting coroutine.
			StopCoroutine(Constants.shootAtPlayer);

			// Update the total enemy count in GameManager.
			Tutorial_GameManager.instance.EndTutorial();

			// Disable all attached scripts and collider.
			gameObject.GetComponent<Tutorial_EnemyMovement>().enabled = false;
			gameObject.GetComponent<Collider2D>().enabled = false;
			
			// Set all child sprites to DeadEnemy Soritng Layer so player will be rendered on top.
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
}
