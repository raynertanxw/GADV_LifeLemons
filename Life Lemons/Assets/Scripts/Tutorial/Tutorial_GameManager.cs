using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tutorial_GameManager : MonoBehaviour
{
	public static Tutorial_GameManager instance = null;

	private GameObject player;

	// UI elements
	private Text tutText;
	private Button nextButton;

	// Coroutine variables
	//Universal.
	private bool nextButtonClicked = false;
	public GameObject EnemeyPrefab;
	public GameObject PlayerPrefab;
	// Tutorial Segment 1.
	private float Tutorial_totalZRotationOfPlayer = 0.0f;
	private float Tutorial_zRotationOfPreviousFrame = 0.0f;

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

		player = (GameObject) Instantiate(PlayerPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
		tutText = GameObject.Find("Tutorial Text").GetComponent<Text>();
		nextButton = GameObject.Find("Tutorial_Button_Next").GetComponent<Button>();
		// Disable next button.
		nextButton.gameObject.SetActive(false);
	}
	
	public void EndTutorial()
	{
		StartCoroutine(TutorialEndingSequence());
		Debug.Log("TUTORIAL END");
	}
	
	void Start()
	{
		StartCoroutine(TutorialSequence());
	}

	public void NextButtonClicked()
	{
		nextButtonClicked = true;
	}

	IEnumerator TutorialSequence()
	{
		// SEGMENT 1
		tutText.text = "Welcome! This is you: the E-Lemonator!\n\nUse the mouse cursor to rotate.\nYou will always face the cursor.";

		yield return null; // Delay the coroutine until it runs in the first update so accurate player rotation can be recorded.

		nextButton.gameObject.SetActive(true);
		
		while (nextButtonClicked == false)
		{
			yield return null;
		}
		
		nextButtonClicked = false; // Reset the button clicked status.
		nextButton.gameObject.SetActive(false); // Hide the button again.

		tutText.text = "Try turning three full circles.";

		// Detect three full rotation.
		Tutorial_zRotationOfPreviousFrame = player.transform.rotation.eulerAngles.z;

		for (int i = 0; i < 3; i++)
		{
			while (Mathf.Abs(Tutorial_totalZRotationOfPlayer) < 360.0f)
			{
				float changeInZRotation = player.transform.rotation.eulerAngles.z - Tutorial_zRotationOfPreviousFrame;
				Tutorial_zRotationOfPreviousFrame = player.transform.rotation.eulerAngles.z;

				if (changeInZRotation < -180.0f) // Special case to handle big change for rotation angles when goes one full circle from 359 to 0.
				{
					Tutorial_totalZRotationOfPlayer += (360.0f + changeInZRotation);
				}
				else if (changeInZRotation > 180.0f) // Special case to handle big change for rotation angles when goes one full circle from 0 to 359.
				{
					Tutorial_totalZRotationOfPlayer -= (360.0f - changeInZRotation);
				}
				else
				{
					Tutorial_totalZRotationOfPlayer += changeInZRotation; // If changes are negative it will minus from total so regardless of direction, it will add accodringly.
				}

				yield return null;
			}

			Tutorial_totalZRotationOfPlayer = 0;
			tutText.text = i+1 + "...";
		}

		tutText.text = "Good job! Click next to continue.";
		nextButton.gameObject.SetActive(true);

		while (nextButtonClicked == false)
		{
			yield return null;
		}

		nextButtonClicked = false; // Reset the button clicked status.
		nextButton.gameObject.SetActive(false); // Hide the button again.

		Debug.Log("Ended Segment 1");



		// SEGMENT 2.
		tutText.text = "Now let's learn how to move.\nUse the A and D keys to move left and right.";
		player.GetComponent<Tutorial_Player_Movement>().canMove = true;

		bool movedLeft = false;
		bool movedRight = false;
		while (movedLeft == false || movedRight == false)
		{
			if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
				movedLeft = true;
			if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
				movedRight = true;

			yield return null;
		}

		tutText.text = "Good! Now try using W and S to move up and down.";
		bool movedUp = false;
		bool movedDown = false;
		while (movedUp == false || movedDown == false)
		{
			if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
				movedUp = true;
			if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
				movedDown = true;

			yield return null;
		}

		tutText.text = "Good job!\nNote that you can press 'P' or 'Escape' to pause the game at anytime.\nClick next to continue.";
		nextButton.gameObject.SetActive(true);
		
		while (nextButtonClicked == false)
		{
			yield return null;
		}
		
		nextButtonClicked = false; // Reset the button clicked status.
		nextButton.gameObject.SetActive(false); // Hide the button again.

		Debug.Log("Ended Segment 2");


		// SEGMENT 3.
		GameObject enemy = (GameObject) Instantiate(EnemeyPrefab, new Vector3 (-10, 0, 0), Quaternion.identity);
		tutText.text = "This is a henchman of the Lemon Mafia.\nHe is out to get you!\nHe will shoot mini lemons at you. So watch out!";

		nextButton.gameObject.SetActive(true);
		
		while (nextButtonClicked == false)
		{
			yield return null;
		}
		
		nextButtonClicked = false; // Reset the button clicked status.
		nextButton.gameObject.SetActive(false); // Hide the button again.

		enemy.GetComponent<Tutorial_NormalHenchCombat>().Shoot();
		tutText.text = "If he hits you. You will lose health.\nSo try to dodge his lemon projectiles.";
		yield return new WaitForSeconds(2.0f);

		nextButton.gameObject.SetActive(true);
		
		while (nextButtonClicked == false)
		{
			yield return null;
		}
		
		nextButtonClicked = false; // Reset the button clicked status.
		nextButton.gameObject.SetActive(false); // Hide the button again.

		tutText.text = "Alternatively, you can try catching his lemons with your funnel.\n Caught lemons convert into your ammunition.\nClick next to try catching one.";

		nextButton.gameObject.SetActive(true);
		
		while (nextButtonClicked == false)
		{
			yield return null;
		}
		
		nextButtonClicked = false; // Reset the button clicked status.
		nextButton.gameObject.SetActive(false); // Hide the button again.

		enemy.GetComponent<Tutorial_NormalHenchCombat>().canShoot = true;

		bool playerCaughtAmmo = false;
		player.GetComponent<Tutorial_Player_Combat>().caughtLemonOnce = false; // In case player caught ammo by accident during first shot.
		while (playerCaughtAmmo == false)
		{
			if (player.GetComponent<Tutorial_Player_Combat>().caughtLemonOnce == true)
			{
				playerCaughtAmmo = true;
			}

			yield return null;
		}

		Debug.Log("Ended Segment 3");


		// SEGMENT 4.

		enemy.GetComponent<Tutorial_NormalHenchCombat>().canShoot = false;
		tutText.text = "Good job you caught one!\nHowever, beware! If you convert more lemons than you can contain, you will malfunction.\nMalfunctioning not only damages you, but also paralyses you momentarily!";

		nextButton.gameObject.SetActive(true);
		
		while (nextButtonClicked == false)
		{
			yield return null;
		}
		
		nextButtonClicked = false; // Reset the button clicked status.
		nextButton.gameObject.SetActive(false); // Hide the button again.

		tutText.text = "Now to retaliate! However, you still cannot shoot.\nYou are now in collect mode. Press Q to switch to shooting mode.";

		player.GetComponent<Tutorial_Player_Combat>().canSwitch = true;

		while (Input.GetKeyDown(KeyCode.Q) == false)
		{
			yield return null;
		}

		tutText.text = "Great! Now you are in shooting mode.\nYou can toggle between collect and shoot by pressing Q.";

		nextButton.gameObject.SetActive(true);
		
		while (nextButtonClicked == false)
		{
			yield return null;
		}
		
		nextButtonClicked = false; // Reset the button clicked status.
		nextButton.gameObject.SetActive(false); // Hide the button again.

		tutText.text = "Click the left mouse button to fire.\nMake sure you are in shooting mode!\nRemember, you face where your mouse is pointing.\nNow... ATTACK!";
		player.GetComponent<Tutorial_Player_Combat>().canShoot = true;

		Debug.Log("Ended Segment 4");

		yield return null;
	}

	IEnumerator TutorialEndingSequence()
	{
		tutText.text = "Once e-lemonated, the Lemon Mafia will drop shiny Quarters which you can pick up by moving over them.\nThese can be used to upgrade your stats from the upgrade section, which you can access from the Main Menu.";

		player.GetComponent<Tutorial_Player_Combat>().canShoot = false;

		nextButton.gameObject.SetActive(true);
		
		while (nextButtonClicked == false)
		{
			yield return null;
		}
		
		nextButtonClicked = false; // Reset the button clicked status.
		nextButton.gameObject.SetActive(false); // Hide the button again.

		tutText.text = "Congratulations! You are now ready for war against the Lemon Mafia.\nAll the best out there E-Lemonator!";

		nextButton.gameObject.SetActive(true);
		GameObject.Find("Tutorial_Button_Next_Text").GetComponent<Text>().text = "END";
		
		while (nextButtonClicked == false)
		{
			yield return null;
		}
		
		nextButtonClicked = false; // Reset the button clicked status.
		nextButton.gameObject.SetActive(false); // Hide the button again.

		// if have save data load main menu.
		if (PlayerPrefs.HasKey(Constants.HIGHEST_CLEARED_LEVEL))
		{
			Application.LoadLevel(Constants.MainMenuScene);
		}
		// Otherwise, load first level.
		else
		{
			//Load level scene.
			Constants.gameMode = GameMode.normal;
			PlayerPrefs.SetInt(Constants.HIGHEST_CLEARED_LEVEL, 0);
			Application.LoadLevel(Constants.LevelScene);
		}
	}
}
