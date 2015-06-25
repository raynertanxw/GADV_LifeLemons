using UnityEngine;
using System.Collections;

public class Enemy_MeleeHench_Combat : MonoBehaviour, IDamagable
{
	public int health;
	public float rateOfFire;
	public float maxFiringRange;

	private Animator anim;
	private Transform player; // Reference to the player.
	private Vector3 distanceFromPlayer;
	private bool punchLeft = true;
	
	void Awake()
	{
		anim = gameObject.GetComponent<Animator>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	void Start()
	{
		StartCoroutine(punchAtPlayer());
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
		if (punchLeft == true)
		{
			anim.SetTrigger("PunchLeft");
			punchLeft = false;
		}
		else
		{
			anim.SetTrigger("PunchRight");
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
			// Update the total enemy count in GameManager.
			GameManager.instance.UpdateEnemyCount();
			// Destroy the enemy object.
			Destroy(gameObject);
		}
	}
}
