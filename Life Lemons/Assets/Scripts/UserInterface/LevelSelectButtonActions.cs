using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelSelectButtonActions : MonoBehaviour
{
	private Animator anim;
	private Animator levelPanelAnim;
	
	void Awake()
	{
		anim = GameObject.Find("Canvas").GetComponent<Animator>();
		levelPanelAnim = GameObject.Find("LevelSelectPanel").GetComponent<Animator>();

		// Disabled non-unlocked levels.
		// The highest unlocked level that the player can select is highest cleared level + 1 (only next level is unlocked).
		for (int i = PlayerPrefs.GetInt(Constants.HIGHEST_CLEARED_LEVEL) + 2; i < Constants.NumOfLevels + 1; i++)
		{
			Button currentButton = GameObject.Find(Constants.LevelButtonNamePrefix + i.ToString()).GetComponent<Button>();
			currentButton.interactable = false;
		}

		if (PlayerPrefs.HasKey(Constants.HIGHEST_CLEARED_LEVEL) == false)
		{
			GameObject.Find("LevelSelect_Button_Endless").GetComponent<Button>().interactable = false;
		}
		else
		{
			if (PlayerPrefs.GetInt(Constants.HIGHEST_CLEARED_LEVEL) < 1)
			{
				GameObject.Find("LevelSelect_Button_Endless").GetComponent<Button>().interactable = false;
			}
			else
			{
				GameObject.Find("LevelSelect_Button_Endless").GetComponent<Button>().interactable = true;
			}
		}
	}
	
	public void ButtonBack()
	{
		if (GameManager.instance.GameOver == true)
		{
			anim.SetTrigger(Constants.LevelSelectToGameOver);
		}
		else
		{
			anim.SetTrigger(Constants.LevelSelectToPause);
		}
	}

	public void ButtonTutorial()
	{
		Time.timeScale = 1f; // Reset the time scale.
		Application.LoadLevel(Constants.TutorialScene);
	}

	public void ButtonEndless()
	{
		// Reset timescale.
		GameManager.Resume();

		Constants.gameMode = GameMode.endless;
		// Load Level Scene.
		Application.LoadLevel(Constants.LevelScene);
	}
	
	public void ButtonPrev()
	{
		levelPanelAnim.SetTrigger(Constants.ScrollPrevious);
	}
	
	public void ButtonNext()
	{
		levelPanelAnim.SetTrigger(Constants.ScrollNext);
	}

	public void LoadLevel(int levelID)
	{
		Constants.gameMode = GameMode.normal;

		PlayerPrefs.SetInt(Constants.SELECTED_LEVEL, levelID);
		GameManager.Resume(); // To reset timescale.
		Application.LoadLevel(Constants.LevelScene);
	}
}
