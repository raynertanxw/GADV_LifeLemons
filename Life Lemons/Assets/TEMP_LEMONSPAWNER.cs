using UnityEngine;
using System.Collections;

public class TEMP_LEMONSPAWNER : MonoBehaviour
{
	public GameObject projectile_lemon;

	private float projectileSpeed = 10f;

	void Start()
	{
		StartCoroutine("ShootLemon");
	}

	private IEnumerator ShootLemon()
	{
		while(true)
		{
			yield return new WaitForSeconds(2);

			GameObject projectile = (GameObject)Instantiate(projectile_lemon, transform.position, Quaternion.identity);
			projectile.GetComponent<Rigidbody2D>().velocity = Vector2.right * projectileSpeed;
		}
	}
}
