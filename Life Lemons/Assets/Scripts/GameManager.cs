using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class GameManager : MonoBehaviour
{
	public static GameManager instance = null;
	public bool GameOver;
	public int NumOfEnemiesRemaining;
	public int totalNumEnemies; // Used for difference in sorting order for enemy layer.
	public bool LastWaveHasSpawned;
	public int currentLoadedLevel;
	public bool paused = false;
	public bool playerIsInvincible = false; // Used for invincibility frames.

	// Stat related variables.
	public float ammoConversionRate;

	private Enemy_Spawner spawner;
	private Animator anim;
	private Text levelNameText;
	private Text endlessSurvivalTime;
	private Text endlessBestTime;
	private float startTime = 0.0f;
	private float endTime = 0.0f;

	void Awake()
	{
		ammoConversionRate = 5.0f;
		if (PlayerPrefs.HasKey(Constants.UPGRADE_OFFENSE_CONVERSION_RATE))
		{
			int statPoint = PlayerPrefs.GetInt(Constants.UPGRADE_OFFENSE_CONVERSION_RATE);
			ammoConversionRate += statPoint;
		}

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
		totalNumEnemies = 0;
		LastWaveHasSpawned = false;

		spawner = GameObject.Find("Enemy_Spawner").GetComponent<Enemy_Spawner>();
		anim = GameObject.Find("Canvas").GetComponent<Animator>();
		levelNameText = GameObject.Find("Level_Name_Text").GetComponent<Text>();
		endlessSurvivalTime = GameObject.Find("Endless_Survival_Time").GetComponent<Text>();
		endlessBestTime = GameObject.Find("Endless_Best_Survival_Time").GetComponent<Text>();

		// Hide endless text.
		endlessSurvivalTime.enabled = false;
		endlessBestTime.enabled = false;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
		{
			if (GameManager.instance.GameOver == false)
			{
				if (paused == false)
				{
					paused = true;
					Time.timeScale = 0;
					anim.SetTrigger(Constants.TransitionToPause);

					// Show the level name.
					StopCoroutine(Constants.fadeOutLevelText);
					Color orgColor = levelNameText.color;
					orgColor.a = 1.0f;
					levelNameText.color = orgColor;
				}
			}
		}
	}

	public static void Resume()
	{
		GameManager.instance.paused = false;
		Time.timeScale = 1;

		GameManager.instance.StartCoroutine(Constants.fadeOutLevelText);
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

		if (Constants.gameMode == GameMode.endless)
		{
			GameManager.instance.endTime = Time.time;
			GameManager.instance.StopCoroutine(Constants.SpawnEndless);

			// Level can only end when player dies.
			GameObject.Find("GameOver_Button_RetryORNextLevel_Text").GetComponent<Text>().text = "RETRY";
			GameManager.instance.anim.SetTrigger(Constants.FadeInGameOver);
			GameObject.Find("GameOver_Text").GetComponent<Text>().text = "SOUR DEFEAT!";

			GameManager.instance.endlessSurvivalTime.text = "TIME SURVIVED: " + (GameManager.instance.endTime - GameManager.instance.startTime).ToString("N2") + "s";
			if (PlayerPrefs.HasKey(Constants.BEST_SURVIVAL_TIME))
			{
				if (GameManager.instance.endTime - GameManager.instance.startTime > PlayerPrefs.GetFloat(Constants.BEST_SURVIVAL_TIME))
				{
					GameManager.instance.endlessSurvivalTime.text = "NEW BEST TIME";
					GameManager.instance.endlessBestTime.text = "BEST TIME: " + (GameManager.instance.endTime - GameManager.instance.startTime).ToString("N2") + "s";
					PlayerPrefs.SetFloat(Constants.BEST_SURVIVAL_TIME, GameManager.instance.endTime - GameManager.instance.startTime); // Save new highscore.
				}
				else
				{
					GameManager.instance.endlessBestTime.text = "BEST TIME: " + PlayerPrefs.GetFloat(Constants.BEST_SURVIVAL_TIME).ToString("N2") + "s";
				}
			}
			else
			{
				GameManager.instance.endlessSurvivalTime.text = "NEW BEST TIME";
				GameManager.instance.endlessBestTime.text = "BEST TIME: " + (GameManager.instance.endTime - GameManager.instance.startTime).ToString("N2") + "s";
				PlayerPrefs.SetFloat(Constants.BEST_SURVIVAL_TIME, GameManager.instance.endTime - GameManager.instance.startTime); // Save new highscore.
			}
			GameManager.instance.endlessBestTime.enabled = true;

			return;
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

			if (PlayerPrefs.HasKey(Constants.SELECTED_LEVEL))
			{
				if (GameManager.instance.currentLoadedLevel != Constants.NumOfLevels)
				{
					PlayerPrefs.SetInt(Constants.SELECTED_LEVEL, GameManager.instance.currentLoadedLevel + 1);
				}
			}

			if (GameManager.instance.currentLoadedLevel != Constants.NumOfLevels)
			{
				GameObject.Find("GameOver_Button_RetryORNextLevel_Text").GetComponent<Text>().text = "NEXT LEVEL";
			}
			else
			{
				GameObject.Find("GameOver_Button_RetryORNextLevel_Text").GetComponent<Text>().text = "REPLAY";
			}
			GameManager.instance.anim.SetTrigger(Constants.FadeInGameOver);
			if (GameManager.instance.currentLoadedLevel != Constants.NumOfLevels)
			{
				GameObject.Find("GameOver_Text").GetComponent<Text>().text = "LEVEL CLEARED!";
			}
			else
			{
				GameObject.Find("GameOver_Text").GetComponent<Text>().text = "GAME CLEARED!";
			}

			// Enable endless mode form level select after player cleared at least level 1.
			if (PlayerPrefs.HasKey(Constants.HIGHEST_CLEARED_LEVEL) == false)
			{
				GameObject.Find("LevelSelect_Button_Endless").GetComponent<Button>().interactable = false;
			}
			else
			{
				if (PlayerPrefs.GetInt(Constants.HIGHEST_CLEARED_LEVEL) < 1)
				{
					GameObject.Find("LevelSelect_Button_Endless").GetComponent<Button>().interactable = false;
				}
				else
				{
					GameObject.Find("LevelSelect_Button_Endless").GetComponent<Button>().interactable = true;
				}
			}

			// Unlock the next level in level select.
			if (GameManager.instance.currentLoadedLevel != Constants.NumOfLevels)
			{
				GameObject.Find(Constants.LevelButtonNamePrefix + (GameManager.instance.currentLoadedLevel + 1).ToString()).GetComponent<Button>().interactable = true;
			}
		}
		// Otherwise player failed level.
		else
		{
			GameObject.Find("GameOver_Button_RetryORNextLevel_Text").GetComponent<Text>().text = "RETRY";
			GameManager.instance.anim.SetTrigger(Constants.FadeInGameOver);
			GameObject.Find("GameOver_Text").GetComponent<Text>().text = "SOUR DEFEAT!";
		}
	}

	void Start()
	{
		if (Constants.gameMode == GameMode.endless)
		{
			StartCoroutine(Constants.SpawnEndless);

			// Set level name text.
			levelNameText.text = "ENDLESS";
			StartCoroutine(Constants.fadeOutLevelText);

			// Highlight the endless mode in level select as yellow.
			Button endlessModeButton = GameObject.Find("LevelSelect_Button_Endless").GetComponent<Button>();
			ColorBlock endlessCB = endlessModeButton.colors;
			endlessCB.normalColor = new Color(0.98f, 0.98f, 0.0f, 1.0f);
			endlessCB.highlightedColor = new Color(0.9f, 0.9f, 0.0f, 1.0f);
			endlessCB.pressedColor = new Color(0.8f, 0.8f, 0.0f, 1.0f);
			endlessCB.disabledColor = new Color(0.8f, 0.8f, 0.0f, 0.5f);
			endlessModeButton.colors = endlessCB;

			return;
		}

		if (PlayerPrefs.HasKey(Constants.SELECTED_LEVEL))
		{
			int selectedLevel = PlayerPrefs.GetInt(Constants.SELECTED_LEVEL);
			// Load the next level after the highest cleared.
			LoadLevelEnemies(Constants.LevelFileNamePrefix + Convert.ToString(selectedLevel));
			currentLoadedLevel = selectedLevel;
			Debug.Log("Loaded Level " + (selectedLevel));
		}
		else
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

		// Set level name text.
		levelNameText.text = "Level " + currentLoadedLevel;
		StartCoroutine(Constants.fadeOutLevelText);

		// If the level is >15, set the level select to load the next page of levels.
		if (GameManager.instance.currentLoadedLevel > 15)
		{
			GameObject.Find("LevelSelectPanel").GetComponent<LevelSelectButtonActions>().ButtonNext();
		}

		// Highlight the current level in level select as yellow.
		Button currentLevelButton = GameObject.Find("LevelButton_Level_" + GameManager.instance.currentLoadedLevel).GetComponent<Button>();
		ColorBlock cb = currentLevelButton.colors;
		cb.normalColor = new Color(0.98f, 0.98f, 0.0f, 1.0f);
		cb.highlightedColor = new Color(0.9f, 0.9f, 0.0f, 1.0f);
		cb.pressedColor = new Color(0.8f, 0.8f, 0.0f, 1.0f);
		cb.disabledColor = new Color(0.8f, 0.8f, 0.0f, 0.5f);
		currentLevelButton.colors = cb;
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

	IEnumerator fadeOutLevelText()
	{
		yield return new WaitForSeconds(1.0f);

		Color orgColor = levelNameText.color;

		while (levelNameText.color.a != 0)
		{
			orgColor.a -= Time.deltaTime / 0.5f;
			levelNameText.color = orgColor;
			yield return null;
		}

		levelNameText.gameObject.SetActive(false);
		orgColor.a = 1.0f;
		levelNameText.color = orgColor;
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

			case "SpawnSingleCircleStrafe":
				// Is the instruction 5 parameters long?
				if (instructuion.Length == 5)
				{
					// Check for any invalid parameters.
					try
					{
						int.Parse(instructuion[1]);
						int.Parse(instructuion[2]);
						float.Parse(instructuion[3]);
					}
					catch (FormatException fe)
					{
						legalSyntax = false;
						Debug.Log(fe.Message + "\n found in instruction Line: " + (currentInstructuion + 1) + " for int/float typed parameter");
						break;
					}

					if (int.Parse(instructuion[1]) < 0 || int.Parse(instructuion[1]) > 2) // Enemy int out of index range.
						legalSyntax = false;
					else if (int.Parse(instructuion[2]) < 0 || int.Parse(instructuion[2]) > 15) // Spawnpoint int out of index range.
						legalSyntax = false;
					else if (legalSyntax == true)
					{
						spawner.SpawnSingleCircleStrafe(int.Parse(instructuion[1]), int.Parse(instructuion[2]), float.Parse(instructuion[3]));
						GameManager.instance.NumOfEnemiesRemaining++;
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

	IEnumerator SpawnEndless()
	{
		// Variables to track survival time.
		startTime = Time.time;

		// Enable endless UI.
		endlessSurvivalTime.text = "TIME SURVIVED: " + (Time.time - startTime).ToString("N2") + "s";
		endlessSurvivalTime.enabled = true;

		float timeSinceLastSpawn;
		float waveDelayReductionPercentage = 0.0f; // Percentage of wave delay subtraction.

		// Spawn only follow type enemies for the first 15 seconds.
		for (int i = 0; i < 5; i++)
		{
			if (GameManager.instance.GameOver == false)
			{
				spawner.SpawnFollow(UnityEngine.Random.Range(0, 3), UnityEngine.Random.Range(0, 16));
				GameManager.instance.NumOfEnemiesRemaining++;
				timeSinceLastSpawn = Time.time;

				while (Time.time - timeSinceLastSpawn < 5.0f)
				{
					endlessSurvivalTime.text = "TIME SURVIVED: " + (Time.time - startTime).ToString("N2") + "s";
					if (GameManager.instance.NumOfEnemiesRemaining == 0)
						break; // Break from the wait loop and spawn the next wave if there are no enemies left.
					yield return null;
				}
			}
		}

		timeSinceLastSpawn = Time.time;
		while (GameManager.instance.GameOver == false)
		{
			int spawnType = UnityEngine.Random.Range(0, 5);
			float waveDelay = 0.0f;
			switch (spawnType)
			{
			case 0:
				spawner.SpawnFollow(UnityEngine.Random.Range(0, 3), UnityEngine.Random.Range(0, 16));
				GameManager.instance.NumOfEnemiesRemaining++;

				timeSinceLastSpawn = Time.time;
				waveDelay = 3.0f;
				break;

			case 1:
				int numWaypoints = UnityEngine.Random.Range(2, 6);
				Vector3[] waypointArray = new Vector3[numWaypoints];

				// x = -16 to 16
				// y = -8 to 8
				for (int i = 0; i < numWaypoints; i++)
				{
					waypointArray[i] = new Vector3(UnityEngine.Random.Range(-16.0f, 16.0f), UnityEngine.Random.Range(-8.0f, 8.0f));
				}

				spawner.SpawnStrafe(UnityEngine.Random.Range(0, 3), UnityEngine.Random.Range(0, 16), waypointArray);
				GameManager.instance.NumOfEnemiesRemaining++;

				timeSinceLastSpawn = Time.time;
				waveDelay = 5.0f;
				break;

			case 2:
				spawner.SpawnQuadCircleStrafe(UnityEngine.Random.Range(0, 3));
				GameManager.instance.NumOfEnemiesRemaining += 4;

				timeSinceLastSpawn = Time.time;
				waveDelay = 30.0f;
				break;

			case 3:
				spawner.SpawnQuadCircleStrafe(UnityEngine.Random.Range(0, 3), UnityEngine.Random.Range(0, 3), UnityEngine.Random.Range(0, 3), UnityEngine.Random.Range(0, 3));
				GameManager.instance.NumOfEnemiesRemaining += 4;
				
				timeSinceLastSpawn = Time.time;
				waveDelay = 40.0f;
				break;

			case 4:
				spawner.SpawnSingleCircleStrafe(UnityEngine.Random.Range(0, 3), UnityEngine.Random.Range(0, 16), UnityEngine.Random.Range(0.0f, 360.0f));
				GameManager.instance.NumOfEnemiesRemaining++;
				
				timeSinceLastSpawn = Time.time;
				waveDelay = 20.0f;
				break;

			default:
				Debug.Log("RANDOM GEN FOR SPAWN TYPE IN ENDLESS OUT OF RANGE");
				break;
			}

			waveDelay -= (waveDelay * waveDelayReductionPercentage);
			if (waveDelayReductionPercentage < 0.5f)
			{
				waveDelayReductionPercentage += 0.02f;
			}
			Debug.Log(waveDelay);

			while (Time.time - timeSinceLastSpawn < waveDelay)
			{
				endlessSurvivalTime.text = "TIME SURVIVED: " + (Time.time - startTime).ToString("N2") + "s";
				if (GameManager.instance.NumOfEnemiesRemaining == 0)
					break; // Break from the wait loop and spawn the next wave if there are no enemies left.
				yield return null;
			}
		}
	}
}
