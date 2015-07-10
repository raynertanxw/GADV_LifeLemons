using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpgradeButtonActions : MonoBehaviour
{
	private Animator anim;
	private Animator upgradeAnim;
	private bool offenseMenuActive = true;

	void Awake()
	{
		anim = GameObject.Find("Canvas").GetComponent<Animator>();
		upgradeAnim = GameObject.Find("Upgrade Panel").GetComponent<Animator>();
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
}
