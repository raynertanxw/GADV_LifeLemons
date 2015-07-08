using UnityEngine;
using System.Collections;

public class Enemy_MediumHench_Combat : MonoBehaviour, IDamagable
{
	public int health;
	public GameObject enemyProjectile;
	public Transform projectileLeftSpawn;
	public Transform projectileRightSpawn;
	public float rateOfFire;
	public float projectileSpeed;
	
	private Animator anim;
	private SpriteRenderer spriteRen;

	void Awake()
	{
		anim = gameObject.GetComponent<Animator>();
		spriteRen = transform.FindChild("MediumHenchBody").GetComponent<SpriteRenderer>();

		int orderNum = ++GameManager.instance.totalNumEnemies; // Cache the totalNumEnemies AFTER incrementing.
		// Set all child sprites to orderNum to avoid sprite odering issues from instances of same prefab.
		for (int i = 0; i < transform.childCount; i++)
		{
			transform.GetChild(i).GetComponent<SpriteRenderer>().sortingOrder = orderNum;
		}
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
		// Play the shooting animation.
		anim.SetTrigger(Constants.Shoot);

		ShootLeft();
		ShootRight();
	}

	private void ShootLeft()
	{
		GameObject projectile = (GameObject)Instantiate(enemyProjectile, projectileLeftSpawn.position, transform.rotation);
		projectile.GetComponent<Rigidbody2D>().velocity = Utilities.RotationZ2DirectionVec(transform.rotation.eulerAngles.z) * projectileSpeed;
	}

	private void ShootRight()
	{
		GameObject projectile = (GameObject)Instantiate(enemyProjectile, projectileRightSpawn.position, transform.rotation);
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
		else
		{
			StartCoroutine(showHurt());
		}
	}

	IEnumerator showHurt()
	{
		spriteRen.color = Color.red;
		yield return new WaitForSeconds(0.1f);
		spriteRen.color = Color.white;
	}
}
