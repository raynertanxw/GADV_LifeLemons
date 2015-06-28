﻿using UnityEngine;
using System.Collections;

public class LevelSelectButtonActions : MonoBehaviour
{
	private Animator anim;
	
	void Awake()
	{
		anim = GameObject.Find("Canvas").GetComponent<Animator>();
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
	
	public void ButtonPrev()
	{
		Debug.Log("PREV");
	}
	
	public void ButtonNext()
	{
		Debug.Log("NEXT");
	}
}