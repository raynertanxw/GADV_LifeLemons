using UnityEngine;
using System.Collections;

public enum EnemyMovementStates {follow, strafing, circleStrafing};

public class Enemy_Movement : MonoBehaviour
{
	public EnemyMovementStates movementState;

	// follow state variables.
	public float followMovementSpeed;
	public float followMinDistanceFromPlayer;

	// strafing state variables.
	public float strafingMovementSpeed;
	public int strafingWaypointIndex;
	public float strafingWaypointTolerence; // The maximum distance to the current waypoint before switching waypoints.
	public Vector3[] strafingWaypoints;

	// circleStrafing state variables.
	public float circleStrafingRadius;
	public float circleStrafingMovementSpeed;
	public float circleStrafingTargetTolerence; // The maximum distance to the current targetVector.
	public float circleStrafingTargetIncrement; // Increment of target angle (i.e. how fast the target position moves around the player).
	public float circleStrafingTargetAngle; // The target angle of rotation used to calculate the target vector of the circle around the player.
	public Vector3 circleStrafingTargetVector; // The target vector of the circle around the player.
	
	private Vector3 directionVec;
	private GameObject player; // Reference to the player.
	
	void Awake()
	{
		player = GameObject.FindGameObjectWithTag(Constants.tagPlayer);
	}
	
	void Update()
	{
		// If the game is paused, immediately return to do nothing.
		if(Time.timeScale == 0)return;

		// Stop moving when the game is over.
		if(GameManager.instance.GameOver == true)return;

		// Handling movement of the enemy.
		switch (movementState)
		{
		case EnemyMovementStates.follow:
			directionVec = player.transform.position - transform.position;
			if (directionVec.magnitude > followMinDistanceFromPlayer)
			{
				transform.position += directionVec.normalized * followMovementSpeed * Time.deltaTime;
			}
			break;

		case EnemyMovementStates.strafing:
			directionVec = strafingWaypoints[strafingWaypointIndex] - transform.position;
			if (directionVec.magnitude < strafingWaypointTolerence)
			{
				// Switch waypoint.
				strafingWaypointIndex++;
				if (strafingWaypointIndex == strafingWaypoints.Length)
				{
					strafingWaypointIndex = 0; // Reset the waypoint index.
				}
			}
			else
			{
				transform.position += directionVec.normalized * strafingMovementSpeed * Time.deltaTime;
			}

			directionVec = player.transform.position - transform.position; // For facing the player.
			break;

		case EnemyMovementStates.circleStrafing:
			// Calcuate the circleStrafingTargetVector.
			circleStrafingTargetAngle = (circleStrafingTargetAngle + circleStrafingTargetIncrement) % 360;
			circleStrafingTargetVector = Utilities.RotationZ2DirectionVec(circleStrafingTargetAngle) * circleStrafingRadius;
			circleStrafingTargetVector = player.transform.position + circleStrafingTargetVector;

			directionVec = circleStrafingTargetVector - transform.position;
			if (directionVec.magnitude > circleStrafingTargetTolerence)
			{
				// Move.
				transform.position += directionVec.normalized * circleStrafingMovementSpeed * Time.deltaTime;
			}

			directionVec = player.transform.position - transform.position; // For facing the player.
			break;

		default:
			Debug.Log("ERROR: EnemyMovementStates out of range");
			break;
		}
		
		// Handle rotation of the enemy.
		transform.rotation = Quaternion.Euler(0, 0, Utilities.DirectionVec2RotationZ(directionVec) % 360);
	}
}
