using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingsButtonActions : MonoBehaviour
{
	private Animator anim;
	private bool fullscreen; // Boolean used to sync the toggle to fullscreen value.
	
	void Awake()
	{
		anim = GameObject.Find("Canvas").GetComponent<Animator>();
		fullscreen = Screen.fullScreen; // Set fullscreen to actual fullscreen value.
		if (fullscreen == true) // If already Screen.fullscreen is true, set fullscreen to be opposite of it. 
		{
			fullscreen = false;
		}
		// BY DEFAULT, fullscreen toggle component is set to false. Hence if player previously left it in fullscreen,
		// The toggle will still be false but the game runs in fullscreen, desyncing this set of booleans.
		// So if fullscreen is true on start, set the toggle isOn to true to sync in. This however,
		// triggers the OnValueChanged method which will call the below ToggleFullScreen function.
		if (Screen.fullScreen == true)
		{
			transform.FindChild("Fullscreen_Toggle").GetComponent<Toggle>().isOn = true;
		}
	}

	public void TransitionBackToMainMenu()
	{
		anim.SetTrigger(Constants.SettingsToMainMenu);
	}

	public void ButtonResetData()
	{
		PlayerPrefs.DeleteAll();
		GameObject.Find("Upgrade Panel").GetComponent<UpgradeButtonActions>().resetAllUpgradeUI();

		if (PlayerPrefs.HasKey(Constants.HIGHEST_CLEARED_LEVEL) == false)
		{
			GameObject.Find("Button_Endless").GetComponent<Button>().interactable = false;
		}
		else
		{
			if (PlayerPrefs.GetInt(Constants.HIGHEST_CLEARED_LEVEL) < 1)
			{
				GameObject.Find("Button_Endless").GetComponent<Button>().interactable = false;
			}
			else
			{
				GameObject.Find("Button_Endless").GetComponent<Button>().interactable = true;
			}
		}
	}

	public void ToggleFullScreen()
	{
		// If the game started in fullscreen, the toggle component is desynced, hence check if the booleans are desycned.
		if (fullscreen == Screen.fullScreen)
		{
			// If they aren't resume normal behaviour.
			Screen.fullScreen = !Screen.fullScreen;
		}
		// If they are, don't do anything to flip only one of the booleans. This WILL sync the toggle to the actual fullscreen state.
		fullscreen = !fullscreen;
	}
}
