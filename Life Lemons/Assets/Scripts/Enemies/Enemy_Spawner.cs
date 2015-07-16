using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy_Spawner : MonoBehaviour
{
	public Vector3[] spawnPoints;
	public GameObject[] EnemyPrefabs;

	private List<Vector3> childTransforms;

	void Awake()
	{
		childTransforms = new List<Vector3>();
		foreach (Transform child in transform)
		{
			childTransforms.Add(child.position);
		}

		spawnPoints = childTransforms.ToArray();
	}

	// Spawn a single lemon mafia at one of the 16 waypoints with default movement speed.
	public void SpawnFollow(int type, int spawnPoint)
	{
		GameObject enemy = (GameObject)Instantiate(EnemyPrefabs[type], spawnPoints[spawnPoint], Quaternion.identity);
		enemy.GetComponent<Enemy_Movement>().movementState = EnemyMovementStates.follow;
	}

	// Spawn a single lemon mafia at one of the 16 waypoints with default movement speed. Strafe behaviour.
	public void SpawnStrafe(int type, int spawnPoint, Vector3[] waypointArray)
	{
		GameObject enemy = (GameObject)Instantiate(EnemyPrefabs[type], spawnPoints[spawnPoint], Quaternion.identity);
		enemy.GetComponent<Enemy_Movement>().movementState = EnemyMovementStates.strafing;
		enemy.GetComponent<Enemy_Movement>().strafingWaypoints = waypointArray;
	}

	// Spawn a circle strafing formation all of same type, with default radius.
	public void SpawnQuadCircleStrafe(int type)
	{
		GameObject enemy1 = (GameObject)Instantiate(EnemyPrefabs[type], spawnPoints[1], Quaternion.identity);
		enemy1.GetComponent<Enemy_Movement>().circleStrafingTargetAngle = 90;
		enemy1.GetComponent<Enemy_Movement>().movementState = EnemyMovementStates.circleStrafing;
		GameObject enemy2 = (GameObject)Instantiate(EnemyPrefabs[type], spawnPoints[5], Quaternion.identity);
		enemy2.GetComponent<Enemy_Movement>().circleStrafingTargetAngle = 0;
		enemy2.GetComponent<Enemy_Movement>().movementState = EnemyMovementStates.circleStrafing;
		GameObject enemy3 = (GameObject)Instantiate(EnemyPrefabs[type], spawnPoints[9], Quaternion.identity);
		enemy3.GetComponent<Enemy_Movement>().circleStrafingTargetAngle = 270;
		enemy3.GetComponent<Enemy_Movement>().movementState = EnemyMovementStates.circleStrafing;
		GameObject enemy4 = (GameObject)Instantiate(EnemyPrefabs[type], spawnPoints[13], Quaternion.identity);
		enemy4.GetComponent<Enemy_Movement>().circleStrafingTargetAngle = 180;
		enemy4.GetComponent<Enemy_Movement>().movementState = EnemyMovementStates.circleStrafing;
	}

	// Spawn a circle strafing formation all of different type, with default radius.
	public void SpawnQuadCircleStrafe(int type1, int type2, int type3, int type4)
	{
		GameObject enemy1 = (GameObject)Instantiate(EnemyPrefabs[type1], spawnPoints[1], Quaternion.identity);
		enemy1.GetComponent<Enemy_Movement>().circleStrafingTargetAngle = 90;
		enemy1.GetComponent<Enemy_Movement>().movementState = EnemyMovementStates.circleStrafing;
		GameObject enemy2 = (GameObject)Instantiate(EnemyPrefabs[type2], spawnPoints[5], Quaternion.identity);
		enemy2.GetComponent<Enemy_Movement>().circleStrafingTargetAngle = 0;
		enemy2.GetComponent<Enemy_Movement>().movementState = EnemyMovementStates.circleStrafing;
		GameObject enemy3 = (GameObject)Instantiate(EnemyPrefabs[type3], spawnPoints[9], Quaternion.identity);
		enemy3.GetComponent<Enemy_Movement>().circleStrafingTargetAngle = 270;
		enemy3.GetComponent<Enemy_Movement>().movementState = EnemyMovementStates.circleStrafing;
		GameObject enemy4 = (GameObject)Instantiate(EnemyPrefabs[type4], spawnPoints[13], Quaternion.identity);
		enemy4.GetComponent<Enemy_Movement>().circleStrafingTargetAngle = 180;
		enemy4.GetComponent<Enemy_Movement>().movementState = EnemyMovementStates.circleStrafing;
	}

	// Spawn a single circle strafing unit.
	public void SpawnSingleCircleStrafe(int type, int spawnPoint, float startAngle)
	{
		GameObject enemy = (GameObject)Instantiate(EnemyPrefabs[type], spawnPoints[spawnPoint], Quaternion.identity);
		enemy.GetComponent<Enemy_Movement>().circleStrafingTargetAngle = startAngle;
		enemy.GetComponent<Enemy_Movement>().movementState = EnemyMovementStates.circleStrafing;
	}
}
