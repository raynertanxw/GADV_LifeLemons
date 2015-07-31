using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuButtonActions : MonoBehaviour
{
	private Animator anim;

	void Awake()
	{
		anim = GameObject.Find("Canvas").GetComponent<Animator>();

		// Enable Endless ONLY if player has cleared at least the first level.
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

		// If loaded upgrade from level menu, load the upgrade menu directly and skip the selection of selected level keys.
		if (Constants.toggledUpgradeFromLevel == true)
		{
			anim.enabled = false; // Disable the animator canvas as we do not need it since we are only using the upgrade panel.
			GameObject.Find("Initial Panel").GetComponent<CanvasGroup>().alpha = 0f;
			GameObject.Find("Upgrade Panel").GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
		}
		else
		{
			PlayerPrefs.DeleteKey(Constants.SELECTED_LEVEL); // Ensure that select level key is deleted everytime player starts game of moves back to menu.
			if (PlayerPrefs.HasKey(Constants.SELECTED_LEVEL) == false)
			{
				Debug.Log("SELECTED_LEVEL key successfully deleted");
			}
		}
	}

	public void ButtonStart()
	{
		// Ensure gamemode is normal.
		Constants.gameMode = GameMode.normal;

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
		// Set gamemode to endless.
		Constants.gameMode = GameMode.endless;
		// Load Level Scene.
		Application.LoadLevel(Constants.LevelScene);
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

	public void ButtonCredits()
	{
		anim.SetTrigger(Constants.MainMenuToCredits);
		GameObject.Find("Credits Panel").GetComponent<CreditButtonActions>().StartCoroutine(Constants.ScrollCredits);
	}
}
