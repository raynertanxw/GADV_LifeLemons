﻿using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public static GameManager instance = null;
	public bool GameOver;

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
}
