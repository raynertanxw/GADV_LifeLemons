using UnityEngine;
using System.Collections;

public class Tutorial_EnemyMovement : MonoBehaviour
{
	// follow state variables.
	public float followMovementSpeed;
	public float followMinDistanceFromPlayer;
	
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
		if (directionVec.magnitude > followMinDistanceFromPlayer)
		{
			transform.position += directionVec.normalized * followMovementSpeed * Time.deltaTime;
		}
		
		// Handle rotation of the enemy.
		transform.rotation = Quaternion.Euler(0, 0, Utilities.DirectionVec2RotationZ(directionVec) % 360);
	}
}
