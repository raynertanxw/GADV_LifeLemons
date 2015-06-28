using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class GameManager : MonoBehaviour
{
	public static GameManager instance = null;
	public bool GameOver;
	public int NumOfEnemiesRemaining;
	public bool LastWaveHasSpawned;
	public int currentLoadedLevel;
	public bool paused = false;

	private Enemy_Spawner spawner;
	public Animator anim;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}

		GameManager.instance.GameOver = false;
		NumOfEnemiesRemaining = 0;
		LastWaveHasSpawned = false;

		spawner = GameObject.Find("Enemy_Spawner").GetComponent<Enemy_Spawner>();
		anim = GameObject.Find("Canvas").GetComponent<Animator>();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			if (GameManager.instance.GameOver == false)
			{
				if (paused == false)
				{
					paused = true;
					Time.timeScale = 0;
					anim.SetTrigger(Constants.TransitionToPause);
				}
			}
		}
	}

	public static void Resume()
	{
		GameManager.instance.paused = false;
		Time.timeScale = 1;
	}

	// For enemy gameobjects to call and deduct themselves from the count.
	public void UpdateEnemyCount()
	{
		GameManager.instance.NumOfEnemiesRemaining--;
		if (GameManager.instance.NumOfEnemiesRemaining == 0 && GameManager.instance.LastWaveHasSpawned == true)
		{
			EndGame();
		}
	}

	public static void EndGame()
	{
		// Set GameOver to true.
		GameManager.instance.GameOver = true;

		// Deactivate all enemies.
		GameObject[] enemies = GameObject.FindGameObjectsWithTag(Constants.tagEnemy);
		foreach (GameObject enemy in enemies)
		{
			MonoBehaviour[] scripts = enemy.GetComponents<MonoBehaviour>();
			foreach (MonoBehaviour script in scripts)
			{
				script.enabled = false;
			}
		}

		// Destroy all projectiles.
		GameObject[] projectiles = GameObject.FindGameObjectsWithTag(Constants.tagProjectile);
		foreach (GameObject projectile in projectiles)
		{
			Destroy(projectile);
		}

		// Check if player has cleared the level.
		if (GameManager.instance.NumOfEnemiesRemaining == 0)
		{
			if (PlayerPrefs.HasKey(Constants.HIGHEST_CLEARED_LEVEL))
			{
				if (GameManager.instance.currentLoadedLevel > PlayerPrefs.GetInt(Constants.HIGHEST_CLEARED_LEVEL))
				{
					if (GameManager.instance.currentLoadedLevel != Constants.NumOfLevels)
					{
						PlayerPrefs.SetInt(Constants.HIGHEST_CLEARED_LEVEL, GameManager.instance.currentLoadedLevel);
					}
				}
			}
			else
			{
				PlayerPrefs.SetInt(Constants.HIGHEST_CLEARED_LEVEL, 1);
			}

			GameObject.Find("GameOver_Button_RetryORNextLevel_Text").GetComponent<Text>().text = "NEXTLEVEL";
			GameManager.instance.anim.SetTrigger(Constants.FadeInGameOver);
			GameObject.Find("GameOver_Text").GetComponent<Text>().text = "LEVEL ClEARED!";
		}
		// Otherwise player failed level.
		else
		{
			GameObject.Find("GameOver_Button_RetryORNextLevel_Text").GetComponent<Text>().text = "RETRY";
			GameManager.instance.anim.SetTrigger(Constants.FadeInGameOver);
			GameObject.Find("GameOver_Text").GetComponent<Text>().text = "GAME OVER!";
		}
	}

	void Start()
	{
		int highestClearedLevel;
		if (PlayerPrefs.HasKey(Constants.HIGHEST_CLEARED_LEVEL))
		{
			highestClearedLevel = PlayerPrefs.GetInt(Constants.HIGHEST_CLEARED_LEVEL);
			if (highestClearedLevel < Constants.NumOfLevels)
			{
				// Load the next level after the highest cleared.
				LoadLevelEnemies(Constants.LevelFileNamePrefix + Convert.ToString(highestClearedLevel + 1));
				currentLoadedLevel = highestClearedLevel + 1;
				Debug.Log("Loaded Level " + (highestClearedLevel + 1));
			}
			else
			{
				// Load the final level
				LoadLevelEnemies(Constants.LevelFileNamePrefix + Convert.ToString(Constants.NumOfLevels));
				currentLoadedLevel = Constants.NumOfLevels;
				Debug.Log("Loaded Level " + Constants.NumOfLevels);
			}
		}
		else
		{
			//Load tutotial scene.
			Application.LoadLevel(Constants.TutorialScene);
		}
	}

	void LoadLevelEnemies(string levelName)
	{
		string[] testLevelArray = Utilities.getLevelTxtInstructions(levelName);
		
		if (testLevelArray != null)
		{
			StartCoroutine(SpawnWaveSet(testLevelArray));
		}
		else
		{
			Debug.Log("Error loading " + levelName);
		}
	}

	IEnumerator SpawnWaveSet(string[] waveSet)
	{
		for (int currentInstructuion = 0; currentInstructuion < waveSet.Length; currentInstructuion++)
		{
			string[] instructuion = waveSet[currentInstructuion].Split(',');

			for (int i = 0; i < instructuion.Length; i++)
			{
				instructuion[i] = instructuion[i].TrimStart(' ');
			}

			bool legalSyntax = true;

			// Check if delay syntax is correct first.
			try
			{
				float.Parse(instructuion[instructuion.Length-1]);
			}
			catch (FormatException fe)
			{
				legalSyntax = false;
				Debug.Log(fe.Message + "\n found in instruction Line: " + (currentInstructuion + 1) + " for delay parameter");
			}

			switch (instructuion[0])
			{
			case "SpawnFollow":

				// Check for any invalid parameters.
				try
				{
					int.Parse(instructuion[1]);
					int.Parse(instructuion[2]);
				}
				catch (FormatException fe)
				{
					legalSyntax = false;
					Debug.Log(fe.Message + "\n found in instruction Line: " + (currentInstructuion + 1) + " for int/float typed parameter");
					break;
				}

				if (instructuion.Length != 4) // Must be 4 parameters.
					legalSyntax = false;
				else if (int.Parse(instructuion[1]) < 0 || int.Parse(instructuion[1]) > 2) // Enemy type int out of index range.
					legalSyntax = false;
				else if (int.Parse(instructuion[2]) < 0 || int.Parse(instructuion[2]) > 15) // Spawnpoint int out of index range.
					legalSyntax = false;
				// Else, safe to execute.
				else
				{
					spawner.SpawnFollow(int.Parse(instructuion[1]), int.Parse(instructuion[2]));
					GameManager.instance.NumOfEnemiesRemaining++;
				}
				break;

			case "SpawnStrafe":

				// Check for first two invalid parameters.
				try
				{
					int.Parse(instructuion[1]);
					int.Parse(instructuion[2]);
				}
				catch (FormatException fe)
				{
					legalSyntax = false;
					Debug.Log(fe.Message + "\n found in instruction Line: " + (currentInstructuion + 1) + " for int/float typed parameter");
					break;
				}

				if (instructuion.Length < 8) // Minimum of 2 waypoints for strafe behaviour to work.
					legalSyntax = false;
				else if (instructuion.Length % 2 != 0) // Due to waypoints having two values each, it will always be even unless a value is missing.
					legalSyntax = false;
				else if (int.Parse(instructuion[1]) < 0 || int.Parse(instructuion[1]) > 2) // Enemy type int out of index range.
					legalSyntax = false;
				else if (int.Parse(instructuion[2]) < 0 || int.Parse(instructuion[2]) > 15) // Spawnpoint int out of index range.
					legalSyntax = false;
				// Else, safe to execute.
				else
				{
					// Convert values at index 3 to second before end of instruction array into array of Vector3s.
					int numOfWaypoints = (instructuion.Length - 4) / 2;
					Vector3[] waypoints = new Vector3[numOfWaypoints];

					// Check if any of the vectors have illegal syntax.
					try
					{
						for (int i = 0; i < numOfWaypoints; i++) // i is the index of the waypoint.
						{
							waypoints[i] = new Vector3(float.Parse(instructuion[(i*2 + 3)].TrimStart('(')), float.Parse(instructuion[(i*2 + 4)].TrimEnd(')')), 0f);
						}
					}
					catch (FormatException fe)
					{
						legalSyntax = false;
						Debug.Log(fe.Message + "\n found in instruction Line: " + (currentInstructuion + 1) + " for int/float typed parameter");
						break;
					}

					spawner.SpawnStrafe(int.Parse(instructuion[1]), int.Parse(instructuion[2]), waypoints);
					GameManager.instance.NumOfEnemiesRemaining++;
				}
				break;

			case "SpawnQuadCircleStrafe":

				// First Method Overload variant.
				if (instructuion.Length == 3)
				{
					// Check for any invalid parameters.
					try
					{
						int.Parse(instructuion[1]);
					}
					catch (FormatException fe)
					{
						legalSyntax = false;
						Debug.Log(fe.Message + "\n found in instruction Line: " + (currentInstructuion + 1) + " for int/float typed parameter");
						break;
					}

					if (int.Parse(instructuion[1]) < 0 || int.Parse(instructuion[1]) > 2) // Enemy type int out of index range.
						legalSyntax = false;
					// Else, safe to execute.
					else
					{
						spawner.SpawnQuadCircleStrafe(int.Parse(instructuion[1]));
						GameManager.instance.NumOfEnemiesRemaining += 4;
					}
				}
				// Second Method Overload variant, where the individual types are specified.
				else if (instructuion.Length == 6)
				{
					// Check for any invalid parameters.
					try
					{
						int.Parse(instructuion[1]);
						int.Parse(instructuion[2]);
						int.Parse(instructuion[3]);
						int.Parse(instructuion[4]);
					}
					catch (FormatException fe)
					{
						legalSyntax = false;
						Debug.Log(fe.Message + "\n found in instruction Line: " + (currentInstructuion + 1) + " for int/float typed parameter");
						break;
					}

					// Check for out of index range enemy type.
					for (int i = 1; i < 5; i++)
					{
						if (int.Parse(instructuion[i]) < 0 || int.Parse(instructuion[i]) > 2)
						{
							legalSyntax = false;
							break;
						}
					}
					// If still legal syntax, then safe to execute.
					if (legalSyntax == true)
					{
						spawner.SpawnQuadCircleStrafe(int.Parse(instructuion[1]), int.Parse(instructuion[2]), int.Parse(instructuion[3]), int.Parse(instructuion[4]));
						GameManager.instance.NumOfEnemiesRemaining += 4;
					}
				}
				// Otherwise, illegal syntax.
				else
				{
					legalSyntax = false;
				}

				break;

			default:
				Debug.Log("Error: No such spawn instruction exists\nError in instruction line: " + (currentInstructuion + 1));
				break;
			}

			// If the legalSyntax flag is false, state the line, and skip the delay, move on to the next wave by going to the next loop of this while loop.
			if (legalSyntax == false)
			{
				Debug.Log("Illegal Spawn Syntax in instruction line: " + (currentInstructuion + 1));
			}
			else
			{
				yield return new WaitForSeconds(float.Parse(instructuion[instructuion.Length-1]));
			}
		}

		GameManager.instance.LastWaveHasSpawned = true;
	}
}
