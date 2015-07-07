﻿using UnityEngine;
using System.Collections;

public class MainMenuButtonActions : MonoBehaviour
{
	private Animator anim;

	void Awake()
	{
		anim = GameObject.Find("Canvas").GetComponent<Animator>();
		PlayerPrefs.DeleteKey(Constants.SELECTED_LEVEL); // Ensure that select level key is deleted everytime player starts game of moves back to menu.
		if (PlayerPrefs.HasKey(Constants.SELECTED_LEVEL) == false)
		{
			Debug.Log("SELECTED_LEVEL key successfully deleted");
		}
	}

	public void ButtonStart()
	{
		if (PlayerPrefs.HasKey(Constants.HIGHEST_CLEARED_LEVEL))
		{
			// Load Level Scene.
			Application.LoadLevel(Constants.LevelScene);
		}
		else
		{
			//Load tutotial scene.
			Application.LoadLevel(Constants.TutorialScene);
		}
	}

	public void ButtonEndless()
	{

	}

	public void ButtonUpgrade()
	{
		anim.SetTrigger(Constants.MainMenuToUpgrade);
	}

	public void ButtonSettings()
	{
		anim.SetTrigger(Constants.MainMenuToSettings);
	}

	public void ButtonQuit()
	{
		Application.Quit();
	}
}
