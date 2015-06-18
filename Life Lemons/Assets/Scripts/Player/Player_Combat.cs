using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum PlayerStates {collect, shoot};

public class Player_Combat : MonoBehaviour, IDamagable
{
	public PlayerStates playerState = PlayerStates.collect;
	public float ammoPercentage;
	public int health;
	public GameObject playerProjectile;
	public bool hasMalfunction;
	public float ammoCostPerShot;
	public float projectileSpeed;
	public int malfunctionDamage;
	public float malfunctionRepairTime;

	private Animator anim;
	private Transform ammoLevelLemonjuice;
	private const float ammoSpriteMaxScale = 0.25f; // Maximum Scale value of sprite. Used for calculation of ammo percenatge UI scale values.

	// UI elements
	private Text textPlayerHealth;
	private Text textPlayerAmmo;

	void Awake()
	{
		anim = gameObject.GetComponent<Animator>();
		ammoLevelLemonjuice = transform.FindChild("Ammo_Level_Indicator");

		textPlayerHealth = GameObject.Find("Text_Player_Health").GetComponent<Text>();
		textPlayerHealth.text = "Health: " + health;
		textPlayerAmmo = GameObject.Find("Text_Player_Ammo").GetComponent<Text>();
		UpdateAmmoUI();

		hasMalfunction = false;
	}

	void Update()
	{
		if (hasMalfunction == false)
		{
			if (Input.GetKeyDown(KeyCode.Q))
			{
				switch (playerState)
				{
				case PlayerStates.collect:
					playerState = PlayerStates.shoot;
					anim.SetTrigger("toggle_shoot");
					break;

				case PlayerStates.shoot:
					playerState = PlayerStates.collect;
					anim.SetTrigger("toggle_collect");
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
		textPlayerHealth.text = "Health: " + health;
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
