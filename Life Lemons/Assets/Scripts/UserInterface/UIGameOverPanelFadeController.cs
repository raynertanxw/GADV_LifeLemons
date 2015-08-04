using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIGameOverPanelFadeController : MonoBehaviour
{
	// Arrays of components with color properties to be changed in their alpha.
	// Set through the editor.
	public Text[] textComponents;
	public Image[] imageComponents;
	
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == Constants.tagPlayer)
		{
			// Ensure that the player cleared the level. Else if player died, don't need to fade at all.
			if (GameManager.instance.GameOver == true && GameManager.instance.NumOfEnemiesRemaining == 0)
			{
				// Fade to 0.5 alpha.
				foreach (Text text in textComponents)
				{
					text.color = new Color(text.color.r, text.color.g, text.color.b, 0.5f);
				}
				foreach (Image image in imageComponents)
				{
					image.color = new Color(image.color.r, image.color.g, image.color.b, 0.5f);
				}
			}
		}
	}
	
	void OnTriggerExit2D(Collider2D col)
	{
		if (col.tag == Constants.tagPlayer)
		{
			// There is only one player so don't need to count, just fade out.
			// Fade to 1.0 alpha.
			foreach (Text text in textComponents)
			{
				text.color = new Color(text.color.r, text.color.g, text.color.b, 1.0f);
			}
			foreach (Image image in imageComponents)
			{
				image.color = new Color(image.color.r, image.color.g, image.color.b, 1.0f);
			}
		}
	}
}
