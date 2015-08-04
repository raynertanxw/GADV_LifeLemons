using UnityEngine;
using System.Collections;

public class MainMenu_FallingObjectSpawnController : MonoBehaviour
{
	public GameObject[] fallingObjectPrefabs;
	private float spawnDelay = 4.0f;

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == Constants.FallingObject)
		{
			Destroy(col.gameObject);
		}
	}

	void Start()
	{
		StartCoroutine(spawnFallingObjects());
	}

	IEnumerator spawnFallingObjects()
	{
		while (true)
		{
			// Position y = 6.0f, Position x = -8.5f to 8.5f
			Instantiate(fallingObjectPrefabs[Random.Range(0, fallingObjectPrefabs.Length)], new Vector3(Random.Range(-8.5f, 8.5f), 6.0f, 0.0f), Quaternion.identity);

			yield return new WaitForSeconds(spawnDelay);
		}
	}
}
