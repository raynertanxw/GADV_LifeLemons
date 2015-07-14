using UnityEngine;
using UnityEngine.UI;
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
		Screen.fullScreen = !Screen.fullScreen;
	}
}
