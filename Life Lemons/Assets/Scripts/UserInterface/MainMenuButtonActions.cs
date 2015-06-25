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
		Application.LoadLevel(Constants.LevelScene);
	}

	public void ButtonEndless()
	{

	}

	public void ButtonUpgrade()
	{

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
