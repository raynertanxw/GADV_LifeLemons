using UnityEngine;
using System.Collections;

public class Enemy_MeleeHench_Combat : MonoBehaviour, IDamagable
{
	public int health;
	public float rateOfFire;
	public float maxFiringRange;
	public int numQuarters;
	public GameObject quarter;

	private Animator anim;
	private SpriteRenderer spriteRen;
	private Transform player; // Reference to the player.
	private Vector3 distanceFromPlayer;
	private bool punchLeft = true;
	private float quarterSpawnPosOffset = 1.0f;
	private Color colorTint;
	
	void Awake()
	{
		anim = gameObject.GetComponent<Animator>();
		player = GameObject.FindGameObjectWithTag(Constants.tagPlayer).transform;
		spriteRen = transform.FindChild("MeleeHench_Body").GetComponent<SpriteRenderer>();

		int orderNum = ++GameManager.instance.totalNumEnemies; // Cache the totalNumEnemies AFTER incrementing.
		// Set all child sprites to orderNum to avoid sprite odering issues from instances of same prefab.
		for (int i = 0; i < transform.childCount; i++)
		{
			transform.GetChild(i).GetComponent<SpriteRenderer>().sortingOrder = orderNum;
		}
	}
	
	void Start()
	{
		StartCoroutine(Constants.punchAtPlayer);

		// Determine the color tint.
		EnemyMovementStates movementType = gameObject.GetComponent<Enemy_Movement>().movementState;

		// Put here because movement state is only confirmed after Awake (due to how it is set in Enemy_Spawner).
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
	
	private IEnumerator punchAtPlayer()
	{
		yield return new WaitForSeconds(rateOfFire);
		while (GameManager.instance.GameOver == false)
		{
			distanceFromPlayer = player.position - transform.position;
			if (distanceFromPlayer.magnitude < maxFiringRange)
			{
				Punch();
				yield return new WaitForSeconds(rateOfFire);
			}
			else
			{
				yield return null;
			}
		}
	}
	
	private void Punch()
	{
		// Alternate between left and right punches.
		if (punchLeft == true)
		{
			anim.SetTrigger(Constants.PunchLeft);
			punchLeft = false;
		}
		else
		{
			anim.SetTrigger(Constants.PunchRight);
			punchLeft = true;
		}
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
			StopCoroutine(Constants.punchAtPlayer);

			// Update the total enemy count in GameManager.
			GameManager.instance.UpdateEnemyCount();

			// Disable all attached scripts and collider.
			gameObject.GetComponent<Enemy_Movement>().enabled = false;
			gameObject.GetComponent<Collider2D>().enabled = false;
			transform.FindChild("MeleeHench_Knuckle_Left").GetComponent<Collider2D>().enabled = false;
			transform.FindChild("MeleeHench_Knuckle_Right").GetComponent<Collider2D>().enabled = false;

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

	// For death animation clip to call.
	public void DestroySelf()
	{
		// Destroy the enemy object.
		Destroy(gameObject);
	}

	// For death animation clip to call.
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
