using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum PlayerStates {collect, shoot};

public class Player_Combat : MonoBehaviour, IDamagable
{
	public PlayerStates playerState = PlayerStates.collect;
	public float ammoPercentage;
	public int maxHealth;
	public int health;
	public GameObject playerProjectile;
	public bool hasMalfunction;
	public float ammoCostPerShot;
	public float projectileSpeed;
	public int malfunctionDamage;
	public float malfunctionRepairTime;

	public Sprite[] chassisDamageStates, glassDamageStates, blasterDamageStates, funnelDamageStates;

	private Animator anim;
	private Transform ammoLevelLemonjuice;
	private const float ammoSpriteMaxScale = 0.25f; // Maximum Scale value of sprite. Used for calculation of ammo percenatge UI scale values.
	private SpriteRenderer chassisSpriteRen, glassSpriteRen, blasterSpriteRen, funnelSpriteRen;

	// UI elements
	private Text textPlayerAmmo;
	private Image[] healthHearts;
	public Sprite emptyHeart;

	void Awake()
	{
		anim = gameObject.GetComponent<Animator>();
		ammoLevelLemonjuice = transform.FindChild("Ammo_Level_Indicator");
		chassisSpriteRen = transform.FindChild("Lemonator_Chassis").GetComponent<SpriteRenderer>();
		glassSpriteRen = transform.FindChild("Lemonator_Glass").GetComponent<SpriteRenderer>();
		blasterSpriteRen = transform.FindChild("Lemonator_Blaster").GetComponent<SpriteRenderer>();
		funnelSpriteRen = transform.FindChild("Lemonator_Funnel").GetComponent<SpriteRenderer>();

		textPlayerAmmo = GameObject.Find("Text_Player_Ammo").GetComponent<Text>();
		UpdateAmmoUI();

		List<Image> heartsList = new List<Image>();
		Transform health_hearts = GameObject.Find("Health_Hearts").transform;
		foreach (Transform heart in health_hearts)
		{
			heartsList.Add(heart.gameObject.GetComponent<Image>());
		}
		healthHearts = heartsList.ToArray();

		hasMalfunction = false;
		health = maxHealth;
	}

	void Update()
	{
		// If the game is paused, immediately return to do nothing.
		if(Time.timeScale == 0)return;

		if (hasMalfunction == false)
		{
			if (Input.GetKeyDown(KeyCode.Q))
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
				if (playerState == PlayerStates.shoot)
				{
					Shoot();
				}
			}
		}
	}

	void UpdateAmmoUI()
	{
		float newScale = ammoSpriteMaxScale * ammoPercentage / 100.0f;
		if (newScale < 0)
		{
			newScale = 0f;
		}
		ammoLevelLemonjuice.localScale = new Vector3(newScale, ammoSpriteMaxScale, 1f);
		textPlayerAmmo.text = "Ammo: " + ammoPercentage + "%";
	}

	void Shoot()
	{
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
			GameObject projectile = (GameObject)Instantiate(playerProjectile, transform.position, Quaternion.Euler(playerRotation));
			projectile.GetComponent<Rigidbody2D>().velocity = velocity;
		}
		else
		{
			Debug.Log("Not enough ammo");
		}
	}

	// For enemy projectiles to call.
	public void TakeDamage(int damage)
	{
		health -= damage;

		// Update UI elements.
		UpdateHealthUI(damage);

		CheckGameOver();
	}

	private void UpdateHealthUI(int lostHearts)
	{
		for (int i = health+lostHearts; i > health; i--)
		{
			healthHearts[i-1].sprite = emptyHeart;
		}

		int healthState = health * 3 / maxHealth;
		// As Take damage is only called after the player takes damage, it will never be full health.
		// Hench, healthState will always be 0-2 as health will always be < maxHealth*3 and never resulting in >2 when divided by maxHealth.
		chassisSpriteRen.sprite = chassisDamageStates[healthState];
		glassSpriteRen.sprite = glassDamageStates[healthState];
		blasterSpriteRen.sprite = blasterDamageStates[healthState];
		funnelSpriteRen.sprite = funnelDamageStates[healthState];
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

	// For enemy projectiles to call.
	public void CollectAmmo(float ammoVolume)
	{
		if (ammoPercentage + ammoVolume <= 100f)
		{
			ammoPercentage += ammoVolume;
			UpdateAmmoUI();
		}
		else
		{
			Debug.Log("malfunction");
			// Damage player.
			TakeDamage(malfunctionDamage);
			hasMalfunction = true;
			StartCoroutine("repairMalfunction");
		}
	}

	private IEnumerator repairMalfunction()
	{
		yield return new WaitForSeconds(malfunctionRepairTime);
		hasMalfunction = false;
	}
}
