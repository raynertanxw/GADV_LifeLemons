using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIFadeController : MonoBehaviour
{
	public CanvasGroup UIElementCG;

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == Constants.tagPlayer)
		{
			// Fade to 0.5 alpha.
			UIElementCG.alpha = 0.5f;
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if (col.tag == Constants.tagPlayer)
		{
			// There is only one player so don't need to count, just fade out.
			// Fade to 1.0 alpha.
			UIElementCG.alpha = 1.0f;
		}
	}
	
}
