using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIGameOverPanelFadeController : MonoBehaviour
{
	public Text[] textComponents;
	public Image[] imageComponents;
	
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == Constants.tagPlayer)
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
