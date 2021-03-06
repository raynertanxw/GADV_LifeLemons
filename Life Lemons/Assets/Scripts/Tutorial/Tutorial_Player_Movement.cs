﻿using UnityEngine;
using System.Collections;

public class Tutorial_Player_Movement : MonoBehaviour
{
	public float speed = 10.0f; // Movement Speed.

	public bool canMove = false;

	private Tutorial_Player_Combat playerCombat;
	private Vector2 directionVec;

	void Update()
	{
		if (canMove == true)
		{
			// Move the player's transform according to Horizontal and Vertical raw axis values.
			gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * speed;
		}

		// Calculate out the angle of which the player turns to face the mouse cursor.
		directionVec = (Utilities.MousePosInWorldSpace() - transform.position);
		transform.rotation = Quaternion.Euler(0, 0, Utilities.DirectionVec2RotationZ(directionVec) % 360);
	}
}
