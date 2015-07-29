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
		// Reset timescale.
		GameManager.Resume();
		anim.SetTrigger(Constants.TransitionFromPause);
	}
	
	public void ButtonLevelSelect()
	{
		anim.SetTrigger(Constants.PauseToLevelSelect);
	}
	
	public void ButtonMainMenu()
	{
		// Reset timescale.
		GameManager.Resume();
		Application.LoadLevel(Constants.MainMenuScene);
	}
}
