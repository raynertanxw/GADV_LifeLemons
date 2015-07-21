using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpgradeButtonActions : MonoBehaviour
{
	public Sprite statBarFilled, statBarEmpty;

	private Animator anim;
	private Animator upgradeAnim;
	private int totalQuarterCount;
	private Text totalQuarterCountText;
	private int[] statCost;

	private RectTransform upgradeConfirmPanel;
	private Text upgradeConfirmText;
	private string confirmKeyName; // Key name for the confirm upgrade panel.
	private int confirmStatNum; // Stat number for the confirm upgrade panel.
	private Button offenseButton, defenseButton; // Buttons to switch between the modes.

	void Awake()
	{
		anim = GameObject.Find("Canvas").GetComponent<Animator>();
		upgradeAnim = GameObject.Find("Upgrade Panel").GetComponent<Animator>();

		totalQuarterCountText = GameObject.Find("Upgrade_QuaterAmount_Text").GetComponent<Text>();
		upgradeConfirmPanel = GameObject.Find("Upgrade_Confirm_Panel").GetComponent<RectTransform>();
		upgradeConfirmText = GameObject.Find("Upgrade_Confirm_Text").GetComponent<Text>();
		dismissConfirmPanel();

		offenseButton = GameObject.Find("Upgrade_Offense_Button").GetComponent<Button>();
		defenseButton = GameObject.Find("Upgrade_Defense_Button").GetComponent<Button>();
		offenseButton.interactable = false;

		statCost = new int[6];

		// Update UI Elements.
		updateQuarterCount();
		for (int i = 0; i < 6; i++)
		{
			updateStatUI(i);
		}
	}
	
	public void TransitionBackToMainMenu()
	{
		anim.SetTrigger(Constants.UpgradeToMainMenu);
	}

	public void TransitionFromOffense2Defense()
	{
		defenseButton.interactable = false;
		offenseButton.interactable = true;

		upgradeAnim.SetTrigger(Constants.OffenseToDefense);
	}

	public void TransitionFromDefense2Offense()
	{
		offenseButton.interactable = false;
		defenseButton.interactable = true;

		upgradeAnim.SetTrigger(Constants.DefenseToOffense);
	}

	void updateQuarterCount()
	{
		if (PlayerPrefs.HasKey(Constants.NUM_OF_QUARTERS) == true)
		{
			totalQuarterCount = PlayerPrefs.GetInt(Constants.NUM_OF_QUARTERS);
			totalQuarterCountText.text = totalQuarterCount.ToString();
		}
		else
		{
			PlayerPrefs.SetInt(Constants.NUM_OF_QUARTERS, 0);
			totalQuarterCount = 0;
			totalQuarterCountText.text = "0";
		}
	}

	int calculateCost(int statPoint)
	{
		return (25 + (25 * statPoint));
	}

	void updateStatUI(int statNum)
	{
		int statPoint = 0;
		string statNamePrefix = "";
		string statBarPrefix = "";

		switch (statNum)
		{
		case 0:
			if (PlayerPrefs.HasKey(Constants.UPGRADE_OFFENSE_BULLET_SPEED) == false)
			{
				PlayerPrefs.SetInt(Constants.UPGRADE_OFFENSE_BULLET_SPEED, 0);
			}
			statPoint = PlayerPrefs.GetInt(Constants.UPGRADE_OFFENSE_BULLET_SPEED);
			statNamePrefix = "Upgrade_Stat_BulletSpeed_";
			statBarPrefix = "Stat_Bar_BulletSpeed_";
			break;
			
		case 1:
			if (PlayerPrefs.HasKey(Constants.UPGRADE_OFFENSE_AMMO_COST) == false)
			{
				PlayerPrefs.SetInt(Constants.UPGRADE_OFFENSE_AMMO_COST, 0);
			}
			statPoint = PlayerPrefs.GetInt(Constants.UPGRADE_OFFENSE_AMMO_COST);
			statNamePrefix = "Upgrade_Stat_AmmoCostReduction_";
			statBarPrefix = "Stat_Bar_AmmoCostReduction_";
			break;
			
		case 2:
			if (PlayerPrefs.HasKey(Constants.UPGRADE_OFFENSE_CONVERSION_RATE) == false)
			{
				PlayerPrefs.SetInt(Constants.UPGRADE_OFFENSE_CONVERSION_RATE, 0);
			}
			statPoint = PlayerPrefs.GetInt(Constants.UPGRADE_OFFENSE_CONVERSION_RATE);
			statNamePrefix = "Upgrade_Stat_LemonConversionRate_";
			statBarPrefix = "Stat_Bar_LemonConversionRate_";
			break;
			
		case 3:
			if (PlayerPrefs.HasKey(Constants.UPGRADE_DEFENSE_HEALTH) == false)
			{
				PlayerPrefs.SetInt(Constants.UPGRADE_DEFENSE_HEALTH, 0);
			}
			statPoint = PlayerPrefs.GetInt(Constants.UPGRADE_DEFENSE_HEALTH);
			statNamePrefix = "Upgrade_Stat_Health_";
			statBarPrefix = "Stat_Bar_Health_";
			break;
			
		case 4:
			if (PlayerPrefs.HasKey(Constants.UPGRADE_DEFENSE_MOVEMENT_SPEED) == false)
			{
				PlayerPrefs.SetInt(Constants.UPGRADE_DEFENSE_MOVEMENT_SPEED, 0);
			}
			statPoint = PlayerPrefs.GetInt(Constants.UPGRADE_DEFENSE_MOVEMENT_SPEED);
			statNamePrefix = "Upgrade_Stat_MovementSpeed_";
			statBarPrefix = "Stat_Bar_MovementSpeed_";
			break;
			
		case 5:
			if (PlayerPrefs.HasKey(Constants.UPGRADE_DEFENSE_REPAIR_TIME) == false)
			{
				PlayerPrefs.SetInt(Constants.UPGRADE_DEFENSE_REPAIR_TIME, 0);
			}
			statPoint = PlayerPrefs.GetInt(Constants.UPGRADE_DEFENSE_REPAIR_TIME);
			statNamePrefix = "Upgrade_Stat_MalfunctionRepairTime_";
			statBarPrefix = "Stat_Bar_MalfunctionRepairTime_";
			break;
			
		default:
			Debug.Log("Specified upgrade stat num out of range.");
			break;
		}

		// Handle sprites for filled stat bars.
		for (int i = 0; i < statPoint; i++)
		{
			GameObject.Find(statBarPrefix + i.ToString()).GetComponent<Image>().sprite = statBarFilled;
		}
		
		// Handle sprites for empty stat bars.
		for (int i = statPoint; i < 5; i++)
		{
			GameObject.Find(statBarPrefix + i.ToString()).GetComponent<Image>().sprite = statBarEmpty;
		}

		statCost[statNum] = calculateCost(statPoint);
		if (statPoint == 5)
		{
			GameObject.Find(statNamePrefix + "UpgradeCost_Text").GetComponent<Text>().text = "MAX";
			GameObject.Find(statNamePrefix + "AddButton").GetComponent<Button>().interactable = false;
			GameObject.Find(statNamePrefix + "AddButton_Text").GetComponent<Text>().text = "MAX";
		}
		else
		{
			GameObject.Find(statNamePrefix + "UpgradeCost_Text").GetComponent<Text>().text = statCost[statNum].ToString();
			if (statCost[statNum] > totalQuarterCount)
			{
				GameObject.Find(statNamePrefix + "AddButton").GetComponent<Button>().interactable = false;
				GameObject.Find(statNamePrefix + "AddButton_Text").GetComponent<Text>().text = "INSUFFICIENT";
			}
			else
			{
				GameObject.Find(statNamePrefix + "AddButton").GetComponent<Button>().interactable = true;
				GameObject.Find(statNamePrefix + "AddButton_Text").GetComponent<Text>().text = "UPGRADE";
			}
		}
	}

	public void UpgradeStat(int statNum)
	{
		if ((totalQuarterCount - statCost[statNum]) < 0)
		{
			Debug.Log("CANNOT AFFORD");
			return;
		}

		string confirmStatName = "";

		switch (statNum)
		{
		case 0:
			confirmKeyName = Constants.UPGRADE_OFFENSE_BULLET_SPEED;
			confirmStatName = "Bullet Speed";
			break;
		
		case 1:
			confirmKeyName = Constants.UPGRADE_OFFENSE_AMMO_COST;
			confirmStatName = "Ammo Cost Reduction";
			break;
		
		case 2:
			confirmKeyName = Constants.UPGRADE_OFFENSE_CONVERSION_RATE;
			confirmStatName = "Lemon Conversion Rate";
			break;
		
		case 3:
			confirmKeyName = Constants.UPGRADE_DEFENSE_HEALTH;
			confirmStatName = "Health";
			break;
		
		case 4:
			confirmKeyName = Constants.UPGRADE_DEFENSE_MOVEMENT_SPEED;
			confirmStatName = "Movement Speed";
			break;
		
		case 5:
			confirmKeyName = Constants.UPGRADE_DEFENSE_REPAIR_TIME;
			confirmStatName = "Malfunction Repair Time";
			break;
		
		default:
			Debug.Log("Specified upgrade stat num out of range.");
			break;
		}

		confirmStatNum = statNum;
		upgradeConfirmText.text = "Upgrade " + confirmStatName +"\nFor " + statCost[confirmStatNum] + " quarters?";

		presentConfirmPanel();
	}

	public void confirmStatUpgrade()
	{
		PlayerPrefs.SetInt(confirmKeyName, (PlayerPrefs.GetInt(confirmKeyName) + 1));
		PlayerPrefs.SetInt(Constants.NUM_OF_QUARTERS, totalQuarterCount - statCost[confirmStatNum]);
		resetAllUpgradeUI();

		dismissConfirmPanel();
	}

	public void presentConfirmPanel()
	{
		upgradeConfirmPanel.localScale = new Vector3(1,1,1);
	}

	public void dismissConfirmPanel()
	{
		upgradeConfirmPanel.localScale = new Vector3(0,1,1);
	}

	public void resetAllUpgradeUI()
	{
		updateQuarterCount();
		for (int i = 0; i < 6; i++)
		{
			updateStatUI(i);
		}
	}
}
