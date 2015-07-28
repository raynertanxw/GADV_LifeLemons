using UnityEngine;
using System.Collections;

public class Tutorial_Enemy_Quarter : MonoBehaviour
{
	private SpriteRenderer spriteRen;
	
	void Awake()
	{
		spriteRen = gameObject.GetComponent<SpriteRenderer>();
	}
	
	void Start()
	{
		StartCoroutine(FlashAndDespawn());
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == Constants.tagPlayer)
		{
			// Add to total quarter count.
			if (PlayerPrefs.HasKey(Constants.NUM_OF_QUARTERS) == true)
			{
				PlayerPrefs.SetInt(Constants.NUM_OF_QUARTERS, PlayerPrefs.GetInt(Constants.NUM_OF_QUARTERS) + 1);
			}
			else
			{
				PlayerPrefs.SetInt(Constants.NUM_OF_QUARTERS, 1);
			}
			Destroy(gameObject); // Despawn self.
		}
	}
	
	IEnumerator FlashAndDespawn()
	{
		Color orgColor = spriteRen.color;
		
		yield return new WaitForSeconds(5.0f);
		
		for (int i = 0; i < 5; i++)
		{
			orgColor.a = 0.0f;
			spriteRen.color = orgColor;
			yield return new WaitForSeconds(0.2f);
			orgColor.a = 1.0f;
			spriteRen.color = orgColor;
			yield return new WaitForSeconds(0.8f);
		}
		
		for (int i = 0; i < 10; i++)
		{
			orgColor.a = 0.0f;
			spriteRen.color = orgColor;
			yield return new WaitForSeconds(0.2f);
			orgColor.a = 1.0f;
			spriteRen.color = orgColor;
			yield return new WaitForSeconds(0.2f);
		}
		
		Destroy(gameObject); // Despawn self.
	}
}
