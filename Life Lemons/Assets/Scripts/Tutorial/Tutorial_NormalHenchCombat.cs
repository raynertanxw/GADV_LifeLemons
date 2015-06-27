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
	
	public void CheckGameOver()
	{
		if (health == 0)
		{
			// Update the total enemy count in GameManager.
			Tutorial_GameManager.instance.EndTutorial();
			// Destroy the enemy object.
			Destroy(gameObject);
		}
	}
}
