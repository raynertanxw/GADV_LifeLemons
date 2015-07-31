using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CreditButtonActions : MonoBehaviour
{
	private Animator canvasAnim;
	private RectTransform creditsText;

	private float scrollSpeed = 50.0f;

	void Awake()
	{
		canvasAnim = GameObject.Find("Canvas").GetComponent<Animator>();
		creditsText = GameObject.Find("Credits_Text").GetComponent<RectTransform>();
	}

	public void ButtonBack()
	{
		canvasAnim.SetTrigger(Constants.CreditsToMainMenu);
		// Give the effect of the scrolling stopping, then it fades away smoothly so don't reset it.
		StopCoroutine(Constants.ScrollCredits);
	}

	IEnumerator ScrollCredits()
	{
		// Reset the position when the credits is reloaded.
		creditsText.anchoredPosition = new Vector2(0f, -550f);
		// Wait for the 1 second animation to finish before statring to scroll the screen.
		yield return new WaitForSeconds(1.0f);

		// Keep looping the credits.
		while (true)
		{
			creditsText.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
			// The loop that loops the rect back to start so it will look like infinite continuous scroll.
			if (creditsText.anchoredPosition.y > 800.0f)
			{
				creditsText.anchoredPosition = new Vector2(0f, -550f);
			}

			yield return null;
		}
	}
}
