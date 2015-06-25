using UnityEngine;
using System.Collections;

public class SettingsButtonActions : MonoBehaviour
{
	private Animator anim;
	
	void Awake()
	{
		anim = GameObject.Find("Canvas").GetComponent<Animator>();
	}

	public void TransitionBackToMainMenu()
	{
		anim.SetTrigger(Constants.SettingsToMainMenu);
	}

	public void ButtonResetData()
	{
		PlayerPrefs.DeleteAll();
	}
}
