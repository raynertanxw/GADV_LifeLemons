using UnityEngine;
using System.Collections;

public class MainMenuButtonActions : MonoBehaviour
{
	private Animator anim;

	void Awake()
	{
		anim = GameObject.Find("Canvas").GetComponent<Animator>();
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
