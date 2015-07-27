using UnityEngine;
using System.Collections;

public class Enemy_NormalHench_Combat : MonoBehaviour, IDamagable
{
	public int health;
	public GameObject enemyProjectile;
	public Transform projectileSpawn;
	public float rateOfFire;
	public float projectileSpeed;
	public int numQuarters;
	public GameObject quarter;
	
	private Animator anim;
	private SpriteRenderer spriteRen;
	private float quarterSpawnPosOffset = 1.0f;
	private Color colorTint;

	void Awake()
	{
		anim = gameObject.GetComponent<Animator>();
		spriteRen = transform.FindChild("NormalHench_Body").GetComponent<SpriteRenderer>();

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

		// Determine the color tint.
		EnemyMovementStates movementType = gameObject.GetComponent<Enemy_Movement>().movementState;
		switch (movementType)
		{
		case EnemyMovementStates.follow:
			colorTint = Color.white;
			break;
		case EnemyMovementStates.strafing:
			colorTint = new Color(1f, 0.625f, 0f);
			break;
		case EnemyMovementStates.circleStrafing:
			colorTint = new Color(0.5f, 1f, 0f);
			break;
		}
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
		else
		{
			StartCoroutine(showHurt());
		}
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

	IEnumerator showHurt()
	{
		spriteRen.color = Color.red;
		yield return new WaitForSeconds(0.1f);
		spriteRen.color = colorTint;
	}
}
