using UnityEngine;
using System.Collections;

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

			switch (instructuion[0])
			{
			case "SpawnFollow":
				spawner.SpawnFollow(int.Parse(instructuion[1]), int.Parse(instructuion[2]));
				break;

			case "SpawnStrafe":
				// Convert values at index 3 to second before end of instruction array into array of Vector3s.
				int numOfWaypoints = (instructuion.Length - 4) / 2;
				Vector3[] waypoints = new Vector3[numOfWaypoints];
				for (int i = 0; i < numOfWaypoints; i++) // i is the index of the waypoint.
				{
					waypoints[i] = new Vector3(float.Parse(instructuion[(i*2 + 3)].TrimStart('(')), float.Parse(instructuion[(i*2 + 4)].TrimEnd(')')), 0f);
				}
				spawner.SpawnStrafe(int.Parse(instructuion[1]), int.Parse(instructuion[2]), waypoints);
				break;

			case "SpawnQuadCircleStrafe":
				// First Method Overload variant.
				if (instructuion.Length == 3)
				{
					spawner.SpawnQuadCircleStrafe(int.Parse(instructuion[1]));
				}
				// Second Method Overload variant, where the individual types are specified.
				else
				{
					spawner.SpawnQuadCircleStrafe(int.Parse(instructuion[1]), int.Parse(instructuion[2]), int.Parse(instructuion[3]), int.Parse(instructuion[4]));
				}
				break;

			default:
				Debug.Log("Error: No such spawn instruction exists\nError in instruction line: " + (currentInstructuion + 1));
				break;
			}
			yield return new WaitForSeconds(float.Parse(instructuion[instructuion.Length-1]));
		}
	}
}
