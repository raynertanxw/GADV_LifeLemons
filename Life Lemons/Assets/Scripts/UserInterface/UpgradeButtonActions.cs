using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpgradeButtonActions : MonoBehaviour
{
	public Sprite statBarFilled, statBarEmpty;

	private Animator anim;
	private Animator upgradeAnim;
	private bool offenseMenuActive = true;
	private int totalQuarterCount;
	private Text totalQuarterCountText;
	private int[] statCost;

	void Awake()
	{
		anim = GameObject.Find("Canvas").GetComponent<Animator>();
		upgradeAnim = GameObject.Find("Upgrade Panel").GetComponent<Animator>();

		totalQuarterCountText = GameObject.Find("Upgrade_QuaterAmount_Text").GetComponent<Text>();

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
		if (offenseMenuActive == false) return; // If already Defense, return to avoid doing anything.

		upgradeAnim.SetTrigger(Constants.OffenseToDefense);
		offenseMenuActive = false;
	}

	public void TransitionFromDefense2Offense()
	{
		if (offenseMenuActive == true) return; // If already Offense, return to avoid doing anything.

		upgradeAnim.SetTrigger(Constants.DefenseToOffense);
		offenseMenuActive = true;
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
		}
		else
		{
			GameObject.Find(statNamePrefix + "UpgradeCost_Text").GetComponent<Text>().text = statCost[statNum].ToString();
			GameObject.Find(statNamePrefix + "AddButton").GetComponent<Button>().interactable = true;
		}
	}

	public void UpgradeStat(int statNum)
	{
		if ((totalQuarterCount - statCost[statNum]) < 0)
		{
			Debug.Log("CANNOT AFFORD");
			return;
		}

		switch (statNum)
		{
		case 0:
			PlayerPrefs.SetInt(Constants.UPGRADE_OFFENSE_BULLET_SPEED, (PlayerPrefs.GetInt(Constants.UPGRADE_OFFENSE_BULLET_SPEED) + 1));
			break;
		
		case 1:
			PlayerPrefs.SetInt(Constants.UPGRADE_OFFENSE_AMMO_COST, (PlayerPrefs.GetInt(Constants.UPGRADE_OFFENSE_AMMO_COST) + 1));
			break;
		
		case 2:
			PlayerPrefs.SetInt(Constants.UPGRADE_OFFENSE_CONVERSION_RATE, (PlayerPrefs.GetInt(Constants.UPGRADE_OFFENSE_CONVERSION_RATE) + 1));
			break;
		
		case 3:
			PlayerPrefs.SetInt(Constants.UPGRADE_DEFENSE_HEALTH, (PlayerPrefs.GetInt(Constants.UPGRADE_DEFENSE_HEALTH) + 1));
			break;
		
		case 4:
			PlayerPrefs.SetInt(Constants.UPGRADE_DEFENSE_MOVEMENT_SPEED, (PlayerPrefs.GetInt(Constants.UPGRADE_DEFENSE_MOVEMENT_SPEED) + 1));
			break;
		
		case 5:
			PlayerPrefs.SetInt(Constants.UPGRADE_DEFENSE_REPAIR_TIME, (PlayerPrefs.GetInt(Constants.UPGRADE_DEFENSE_REPAIR_TIME) + 1));
			break;
		
		default:
			Debug.Log("Specified upgrade stat num out of range.");
			break;
		}

		PlayerPrefs.SetInt(Constants.NUM_OF_QUARTERS, totalQuarterCount - statCost[statNum]);
		updateQuarterCount();
		updateStatUI(statNum);
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
