using UnityEngine;
using System.Collections;

public class PauseButtonActions : MonoBehaviour
{
	private Animator anim;
	
	void Awake()
	{
		anim = GameObject.Find("Canvas").GetComponent<Animator>();
	}
	
	public void ButtonResume()
	{
		GameManager.instance.paused = false;
		anim.SetTrigger(Constants.TransitionFromPause);
	}
	
	public void ButtonLevelSelect()
	{
		anim.SetTrigger(Constants.PauseToLevelSelect);
	}
	
	public void ButtonMainMenu()
	{
		Application.LoadLevel(Constants.MainMenuScene);
	}
}
