﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tutorial_Player_Combat : MonoBehaviour, IDamagable
{
	// Used for Tutorial control. Limits player's controls until certain point in tutorial.
	public bool canSwitch = false;
	public bool canShoot = false;
	public bool caughtLemonOnce = false;

	public PlayerStates playerState = PlayerStates.collect;
	public float ammoPercentage;
	public int maxHealth;
	public int health;
	public GameObject playerProjectile;
	public float ammoCostPerShot;
	public float projectileSpeed;

	public Sprite[] chassisDamageStates, glassDamageStates, blasterDamageStates, funnelDamageStates;

	private Animator anim;
	private Transform ammoLevelLemonjuice;
	private Transform projectileSpawn;
	private const float ammoSpriteMaxScale = 0.25f; // Maximum Scale value of sprite. Used for calculation of ammo percenatge UI scale values.
	private SpriteRenderer chassisSpriteRen, glassSpriteRen, blasterSpriteRen, funnelSpriteRen;
	private SpriteRenderer lemonJuiceSpriteRen;

	// UI elements
	private Text textPlayerAmmo;

	void Awake()
	{
		anim = gameObject.GetComponent<Animator>();
		ammoLevelLemonjuice = transform.FindChild("Ammo_Level_Indicator");
		projectileSpawn = transform.FindChild("Projectile_Spawn");
		chassisSpriteRen = transform.FindChild("Lemonator_Chassis").GetComponent<SpriteRenderer>();
		glassSpriteRen = transform.FindChild("Lemonator_Glass").GetComponent<SpriteRenderer>();
		blasterSpriteRen = transform.FindChild("Lemonator_Blaster").GetComponent<SpriteRenderer>();
		funnelSpriteRen = transform.FindChild("Lemonator_Funnel").GetComponent<SpriteRenderer>();
		lemonJuiceSpriteRen = transform.FindChild("Ammo_Level_Indicator").GetComponent<SpriteRenderer>();

		UpdateAmmoUI();

		health = maxHealth;
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(1) && canSwitch == true) // Right Click.
		{
			switch (playerState)
			{
			case PlayerStates.collect:
				playerState = PlayerStates.shoot;
				anim.SetTrigger(Constants.toggle_shoot);
				break;

			case PlayerStates.shoot:
				playerState = PlayerStates.collect;
				anim.SetTrigger(Constants.toggle_collect);
				break;

			default:
				break;
			}
		}

		if (Input.GetMouseButtonDown(0)) // Left Click.
		{
			if (playerState == PlayerStates.shoot && canShoot == true)
			{
				Shoot();
			}
		}
	}

	// Note: This only updates the ammo indicator on the player's body itself. Tutorial does not have main UI to update.
	void UpdateAmmoUI()
	{
		float newScale = ammoSpriteMaxScale * ammoPercentage / 100.0f;
		if (newScale < 0)
		{
			newScale = 0f;
		}
		ammoLevelLemonjuice.localScale = new Vector3(newScale, ammoSpriteMaxScale, 1f);
	}

	void Shoot()
	{
		// If enough ammo, shoot.
		if (ammoPercentage - ammoCostPerShot >= 0f)
		{
			ammoPercentage -= ammoCostPerShot;
			UpdateAmmoUI();

			// Calculate the rotation and vector to instantiate the object and set it's velocity.
			Vector3 playerRotation = transform.rotation.eulerAngles;
			float angle = (playerRotation.z % 360)*Mathf.Deg2Rad; //0 - 360
			float x = Mathf.Cos(angle); // cos normalized. Gets a 1 or -1 and stuff.
			float y = Mathf.Sin(angle); // sin normalized. Gets a 1 or -1 and stuff.
			Vector3 velocity = new Vector3(x, y, 0f) * projectileSpeed;

			playerRotation.z += 90f;
			GameObject projectile = (GameObject)Instantiate(playerProjectile, projectileSpawn.position, Quaternion.Euler(playerRotation));
			projectile.GetComponent<Rigidbody2D>().velocity = velocity;
		}
		// Else, since it's tutorial, let the player shoot to their heart's content! REFILL AMMO!
		else
		{
			Debug.Log("TUTORIAL_REFILLING_AMMO");

			ammoPercentage = 50.0f;
			UpdateAmmoUI();
			
			// Calculate the rotation and vector to instantiate the object and set it's velocity.
			Vector3 playerRotation = transform.rotation.eulerAngles;
			float angle = (playerRotation.z % 360)*Mathf.Deg2Rad; //0 - 360
			float x = Mathf.Cos(angle); // cos normalized. Gets a 1 or -1 and stuff.
			float y = Mathf.Sin(angle); // sin normalized. Gets a 1 or -1 and stuff.
			Vector3 velocity = new Vector3(x, y, 0f) * projectileSpeed;
			
			playerRotation.z += 90f;
			GameObject projectile = (GameObject)Instantiate(playerProjectile, transform.position, Quaternion.Euler(playerRotation));
			projectile.GetComponent<Rigidbody2D>().velocity = velocity;
		}
	}

	// For enemy projectiles to call.
	public void TakeDamage(int damage)
	{
		// Flash Red.
		StartCoroutine(showHurt());

		health -= damage;
		// Update UI elements.
		int healthState = health * 3 / maxHealth;
		// As Take damage is only called after the player takes damage, it will never be full health.
		// Hench, healthState will always be 0-2 as health will always be < maxHealth*3 and never resulting in >2 when divided by maxHealth.
		chassisSpriteRen.sprite = chassisDamageStates[healthState];
		glassSpriteRen.sprite = glassDamageStates[healthState];
		blasterSpriteRen.sprite = blasterDamageStates[healthState];
		funnelSpriteRen.sprite = funnelDamageStates[healthState];

		// Since it's tutorial, MAKE PLAYER IMMORTAL!!!
		if (health < 10)
		{
			health = 500;
		}

		CheckGameOver();
	}

	public void CheckGameOver()
	{
		if (health == 0)
		{
			// GameOver.
			Destroy(gameObject);
			GameManager.EndGame();
		}
	}

	IEnumerator showHurt()
	{
		chassisSpriteRen.color = Color.red;
		glassSpriteRen.color = Color.red;
		blasterSpriteRen.color = Color.red;
		funnelSpriteRen.color = Color.red;
		lemonJuiceSpriteRen.color = Color.red;
		yield return new WaitForSeconds(0.1f);
		chassisSpriteRen.color = Color.white;
		glassSpriteRen.color = Color.white;
		blasterSpriteRen.color = Color.white;
		funnelSpriteRen.color = Color.white;
		lemonJuiceSpriteRen.color = Color.white;
	}

	// For enemy projectiles to call.
	public void CollectAmmo(float ammoVolume)
	{
		caughtLemonOnce = true;

		if (ammoPercentage + ammoVolume <= 100f)
		{
			ammoPercentage += ammoVolume;
			UpdateAmmoUI();
		}
	}
}
