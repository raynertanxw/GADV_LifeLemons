using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public static GameManager instance = null;

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
	}

	public static void EndGame()
	{
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
}
