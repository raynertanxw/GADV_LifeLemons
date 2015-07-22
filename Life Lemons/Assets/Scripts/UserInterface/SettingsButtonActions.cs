using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingsButtonActions : MonoBehaviour
{
	private Animator anim;
	private bool fullscreen; // Boolean used to sync the toggle to fullscreen value.
	private int screenResolutionID = 0;
	private Vector2 screenResolution;
	private int numOfResolutions = 11;

	private Text resolutionText;
	private RectTransform resetDataConfirmPanel;
	private Button confirmDeleteDataButton;
	private Text confirmPanelText;
	
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
		resolutionText = GameObject.Find("Resolution_Text").GetComponent<Text>();
		resetDataConfirmPanel = GameObject.Find("ResetData_Confirm_Panel").GetComponent<RectTransform>();
		confirmDeleteDataButton = GameObject.Find("ResetData_Confirm_Yes_Button").GetComponent<Button>();
		confirmPanelText = GameObject.Find("ResetData_Confirm_Text").GetComponent<Text>();

		screenResolution = new Vector2(Screen.width, Screen.height);

		// Just to set the initial resolution display.
		if (PlayerPrefs.HasKey(Constants.SETTINGS_RESOLUTION_ID) == true)
		{
			screenResolutionID = PlayerPrefs.GetInt(Constants.SETTINGS_RESOLUTION_ID);
			changeResolutionID(screenResolutionID);
			applyResolutionChange();
		}
		else
		{
			PlayerPrefs.SetInt(Constants.SETTINGS_RESOLUTION_ID, 0);
			changeResolutionID(0);
			applyResolutionChange();
		}
	}

	public void TransitionBackToMainMenu()
	{
		anim.SetTrigger(Constants.SettingsToMainMenu);
	}

	public void DeleteData()
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

		confirmDeleteDataButton.interactable = false;
		confirmPanelText.text = "Data has been deleted";
	}

	public void presentConfirmPanel()
	{
		resetDataConfirmPanel.localScale = new Vector3(1,1,1);
	}

	public void dismissConfirmPanel()
	{
		resetDataConfirmPanel.localScale = new Vector3(0,1,1);
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

	public void changeResolutionID(bool IncreaseID)
	{
		if (IncreaseID == true)
		{
			screenResolutionID++;
			if (screenResolutionID == numOfResolutions)
				screenResolutionID = 0;
		}
		else
		{
			screenResolutionID--;
			if (screenResolutionID == -1)
				screenResolutionID = numOfResolutions - 1;
		}

		string aspectRatio = "";

		switch (screenResolutionID)
		{
		case 0:
			screenResolution.x = 1280;
			screenResolution.y = 720;
			aspectRatio = "(16:9)";
			break;
		case 1:
			screenResolution.x = 1366;
			screenResolution.y = 768;
			aspectRatio = "(16:9)";
			break;
		case 2:
			screenResolution.x = 1920;
			screenResolution.y = 1080;
			aspectRatio = "(16:9)";
			break;
		case 3:
			screenResolution.x = 2560;
			screenResolution.y = 1440;
			aspectRatio = "(16:9)";
			break;
		case 4:
			screenResolution.x = 1280;
			screenResolution.y = 800;
			aspectRatio = "(16:10)";
			break;
		case 5:
			screenResolution.x = 1440;
			screenResolution.y = 900;
			aspectRatio = "(16:10)";
			break;
		case 6:
			screenResolution.x = 1680;
			screenResolution.y = 1050;
			aspectRatio = "(16:10)";
			break;
		case 7:
			screenResolution.x = 1920;
			screenResolution.y = 1200;
			aspectRatio = "(16:10)";
			break;
		case 8:
			screenResolution.x = 640;
			screenResolution.y = 480;
			aspectRatio = "(4:3)";
			break;
		case 9:
			screenResolution.x = 800;
			screenResolution.y = 600;
			aspectRatio = "(4:3)";
			break;
		case 10:
			screenResolution.x = 1024;
			screenResolution.y = 768;
			aspectRatio = "(4:3)";
			break;
		default:
			Debug.Log("Screen Resolution ID out of range");
			break;
		}

		resolutionText.text = screenResolution.x + " x " + screenResolution.y + " " + aspectRatio;
	}

	public void changeResolutionID(int resolutionID)
	{
		string aspectRatio = "";
		
		switch (resolutionID)
		{
		case 0:
			screenResolution.x = 1280;
			screenResolution.y = 720;
			aspectRatio = "(16:9)";
			break;
		case 1:
			screenResolution.x = 1366;
			screenResolution.y = 768;
			aspectRatio = "(16:9)";
			break;
		case 2:
			screenResolution.x = 1920;
			screenResolution.y = 1080;
			aspectRatio = "(16:9)";
			break;
		case 3:
			screenResolution.x = 2560;
			screenResolution.y = 1440;
			aspectRatio = "(16:9)";
			break;
		case 4:
			screenResolution.x = 1280;
			screenResolution.y = 800;
			aspectRatio = "(16:10)";
			break;
		case 5:
			screenResolution.x = 1440;
			screenResolution.y = 900;
			aspectRatio = "(16:10)";
			break;
		case 6:
			screenResolution.x = 1680;
			screenResolution.y = 1050;
			aspectRatio = "(16:10)";
			break;
		case 7:
			screenResolution.x = 1920;
			screenResolution.y = 1200;
			aspectRatio = "(16:10)";
			break;
		case 8:
			screenResolution.x = 640;
			screenResolution.y = 480;
			aspectRatio = "(4:3)";
			break;
		case 9:
			screenResolution.x = 800;
			screenResolution.y = 600;
			aspectRatio = "(4:3)";
			break;
		case 10:
			screenResolution.x = 1024;
			screenResolution.y = 768;
			aspectRatio = "(4:3)";
			break;
		default:
			Debug.Log("Screen Resolution ID out of range");
			break;
		}
		
		resolutionText.text = screenResolution.x + " x " + screenResolution.y + " " + aspectRatio;
	}

	public void applyResolutionChange()
	{
		PlayerPrefs.SetInt(Constants.SETTINGS_RESOLUTION_ID, screenResolutionID);
		Screen.SetResolution((int)screenResolution.x, (int)screenResolution.y, Screen.fullScreen);
	}
}
