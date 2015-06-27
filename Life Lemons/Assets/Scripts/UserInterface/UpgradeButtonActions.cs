using UnityEngine;
using System.Collections;

public class UpgradeButtonActions : MonoBehaviour
{
	private Animator anim;
	
	void Awake()
	{
		anim = GameObject.Find("Canvas").GetComponent<Animator>();
	}
	
	public void TransitionBackToMainMenu()
	{
		anim.SetTrigger(Constants.UpgradeToMainMenu);
	}
}
