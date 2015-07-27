using UnityEngine;
using System.Collections;

public class GameOverButtonActions : MonoBehaviour
{
	private Animator anim;
	
	void Awake()
	{
		anim = GameObject.Find("Canvas").GetComponent<Animator>();
	}
	
	public void ButtonRetry()
	{
		Application.LoadLevel(Constants.LevelScene);
	}
	
	public void ButtonLevelSelect()
	{
		anim.SetTrigger(Constants.GameOverToLevelSelect);
	}

	public void ButtonUpgrade()
	{
		Constants.toggledUpgradeFromLevel = true;

		Application.LoadLevel(Constants.MainMenuScene);
	}

	public void ButtonMainMenu()
	{
		Application.LoadLevel(Constants.MainMenuScene);
	}
}
