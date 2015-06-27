using UnityEngine;
using System.Collections;

public class Tutorial_EnemyMovement : MonoBehaviour
{
	private Vector3 directionVec;
	private GameObject player; // Reference to the player.
	
	void Awake()
	{
		player = GameObject.FindGameObjectWithTag(Constants.tagPlayer);
	}
	
	void Update()
	{
		// Handle rotation of the enemy.
		directionVec = player.transform.position - transform.position;
		transform.rotation = Quaternion.Euler(0, 0, Utilities.DirectionVec2RotationZ(directionVec) % 360);
	}
}
