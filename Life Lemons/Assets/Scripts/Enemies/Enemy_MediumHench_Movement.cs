using UnityEngine;
using System.Collections;

public class Enemy_MediumHench_Movement : MonoBehaviour
{
	public float movementSpeed;
	public float minDistanceFromPlayer;
	
	private Vector3 directionVec;
	private GameObject player; // Reference to the player.
	
	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	void Update()
	{
		// Handling movement of the enemy.
		directionVec = player.transform.position - transform.position;
		if (directionVec.magnitude > minDistanceFromPlayer)
		{
			transform.position += directionVec * movementSpeed * Time.deltaTime;
		}
		
		// Handle rotation of the enemy.
		transform.rotation = Quaternion.Euler(0, 0, Utilities.DirectionVec2RotationZ(directionVec) % 360);
	}
}
