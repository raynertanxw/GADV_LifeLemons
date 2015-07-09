﻿using UnityEngine;
using System.Collections;

public class Enemy_MeleeHench_Combat : MonoBehaviour, IDamagable
{
	public int health;
	public float rateOfFire;
	public float maxFiringRange;

	private Animator anim;
	private SpriteRenderer spriteRen;
	private Transform player; // Reference to the player.
	private Vector3 distanceFromPlayer;
	private bool punchLeft = true;
	
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

	// For animation to call.
	public void DestroySelf()
	{
		// Destroy the enemy object.
		Destroy(gameObject);
	}

	IEnumerator showHurt()
	{
		spriteRen.color = Color.red;
		yield return new WaitForSeconds(0.1f);
		spriteRen.color = Color.white;
	}
}
