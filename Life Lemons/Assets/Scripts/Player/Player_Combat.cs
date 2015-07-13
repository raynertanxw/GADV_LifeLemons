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

	public Sprite[] chassisDamageStates, glassDamageStates, blasterDamageStates, funnelDamageStates, malfunctionParticles;

	private Animator anim;
	private Transform ammoLevelLemonjuice;
	private Transform projectileSpawn;
	private const float ammoSpriteMaxScale = 0.25f; // Maximum Scale value of sprite. Used for calculation of ammo percenatge UI scale values.
	private SpriteRenderer chassisSpriteRen, glassSpriteRen, blasterSpriteRen, funnelSpriteRen;
	private SpriteRenderer lemonJuiceSpriteRen, malfunctionParticleSpriteRen;

	// UI elements
	private Text textPlayerAmmo;
	private RectTransform imagePlayerAmmoRectTransform;
	private Image[] healthHearts;
	public Sprite emptyHeart;

	void Awake()
	{
		// Setting up of values affected by upgrade.
		if (PlayerPrefs.HasKey(Constants.UPGRADE_OFFENSE_BULLET_SPEED))
		{
			int statPoint = PlayerPrefs.GetInt(Constants.UPGRADE_OFFENSE_BULLET_SPEED);
			projectileSpeed += (statPoint * 2);
		}

		if (PlayerPrefs.HasKey(Constants.UPGRADE_OFFENSE_AMMO_COST))
		{
			int statPoint = PlayerPrefs.GetInt(Constants.UPGRADE_OFFENSE_AMMO_COST);
			ammoCostPerShot -= (statPoint * 0.5f);
		}

		if (PlayerPrefs.HasKey(Constants.UPGRADE_DEFENSE_HEALTH))
		{
			int statPoint = PlayerPrefs.GetInt(Constants.UPGRADE_DEFENSE_HEALTH);
			maxHealth += statPoint;
		}

		if (PlayerPrefs.HasKey(Constants.UPGRADE_DEFENSE_REPAIR_TIME))
		{
			int statPoint = PlayerPrefs.GetInt(Constants.UPGRADE_DEFENSE_REPAIR_TIME);
			malfunctionRepairTime -= (statPoint * 0.3f);
		}

		anim = gameObject.GetComponent<Animator>();
		ammoLevelLemonjuice = transform.FindChild("Ammo_Level_Indicator");
		projectileSpawn = transform.FindChild("Projectile_Spawn");
		chassisSpriteRen = transform.FindChild("Lemonator_Chassis").GetComponent<SpriteRenderer>();
		glassSpriteRen = transform.FindChild("Lemonator_Glass").GetComponent<SpriteRenderer>();
		blasterSpriteRen = transform.FindChild("Lemonator_Blaster").GetComponent<SpriteRenderer>();
		funnelSpriteRen = transform.FindChild("Lemonator_Funnel").GetComponent<SpriteRenderer>();
		lemonJuiceSpriteRen = transform.FindChild("Ammo_Level_Indicator").GetComponent<SpriteRenderer>();
		malfunctionParticleSpriteRen = transform.FindChild("Malfunction_Transform").GetComponent<SpriteRenderer>();

		textPlayerAmmo = GameObject.Find("Text_Player_Ammo").GetComponent<Text>();
		imagePlayerAmmoRectTransform = GameObject.Find("Ammo_Indicator_LemonJuice_Level").GetComponent<RectTransform>();
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

		// Hide hearts according to max health.
		for (int i = maxHealth; i < 10; i++)
		{
			healthHearts[i].enabled = false;
		}
	}

	void Update()
	{
		// If the game is paused, immediately return to do nothing.
		if(Time.timeScale == 0)return;

		// If the game is over don't do anything.
		if(GameManager.instance.GameOver == true)return;

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

		// Update player ammo indicator.
		ammoLevelLemonjuice.localScale = new Vector3(newScale, ammoSpriteMaxScale, 1f);

		// Update UI ammo indicator.
		if (ammoPercentage % 1 != 0)
			textPlayerAmmo.text = ammoPercentage + "%";
		else
			textPlayerAmmo.text = ammoPercentage + ".0%";
		imagePlayerAmmoRectTransform.localScale = new Vector3(1f, ammoPercentage / 100.0f, 1f);
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
			GameObject projectile = (GameObject)Instantiate(playerProjectile, projectileSpawn.position, Quaternion.Euler(playerRotation));
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
		// Flash Red.
		StartCoroutine(showHurt());

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

	public void CheckGameOver()
	{
		if (health == 0)
		{
			// GameOver.
			GameManager.EndGame();
			Destroy(gameObject);
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
			ammoPercentage = 100f;
			UpdateAmmoUI();

			Debug.Log("malfunction");
			// Damage player.
			TakeDamage(malfunctionDamage);
			hasMalfunction = true;
			StartCoroutine("repairMalfunction");
			StartCoroutine("animateMalfunction");
		}
	}

	private IEnumerator animateMalfunction()
	{
		while (true)
		{
			for (int i = 0; i < malfunctionParticles.Length; i++)
			{
				malfunctionParticleSpriteRen.sprite = malfunctionParticles[i];
				yield return new WaitForSeconds(0.1f);
			}
		}
	}

	private IEnumerator repairMalfunction()
	{
		yield return new WaitForSeconds(malfunctionRepairTime);
		StopCoroutine("animateMalfunction");
		malfunctionParticleSpriteRen.sprite = null;
		hasMalfunction = false;
	}
}
