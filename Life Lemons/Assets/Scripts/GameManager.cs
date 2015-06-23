using UnityEngine;
using System.Collections;
using System;

public class GameManager : MonoBehaviour
{
	public static GameManager instance = null;
	public bool GameOver;

	private Enemy_Spawner spawner;

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

		spawner = GameObject.Find("Enemy_Spawner").GetComponent<Enemy_Spawner>();
	}

	public static void EndGame()
	{
		// Set GameOver to true.
		GameManager.instance.GameOver = true;

		// Deactivate all objects.
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (GameObject enemy in enemies)
		{
			MonoBehaviour[] scripts = enemy.GetComponents<MonoBehaviour>();
			foreach (MonoBehaviour script in scripts)
			{
				script.enabled = false;
			}
		}
	}

	void Start()
	{
		// DEBUG TESTING OF LEVEL LOADING
		LoadLevelEnemies("Level_test");
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
	}
}
