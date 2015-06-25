using UnityEngine;
using System.Collections;

public class Tutorial_GameManager : MonoBehaviour
{
	public static Tutorial_GameManager instance = null;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
	}
	
	public void EndTutorial()
	{
		// Switch to Main Level_scene.
	}
	
	void Start()
	{

	}
}
