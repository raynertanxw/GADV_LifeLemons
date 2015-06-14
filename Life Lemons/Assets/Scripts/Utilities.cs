using UnityEngine;
using System.Collections;

public static class Utilities
{
	// Calculates the z rotation that a direction vector is pointing to.
	public static float DirectionVec2RotationZ(Vector3 directionVector)
	{
		float atanAngle = Mathf.Atan(directionVector.y / directionVector.x) * Mathf.Rad2Deg;
		float x = directionVector.x, y = directionVector.y;
		float angle = 0f;
		if (x >= 0 && y >= 0) // x and y are positive
		{
			angle = atanAngle;
		}
		else if (x < 0 && y >= 0) // x is negative and y is positive
		{
			angle = 180 + atanAngle;
		}
		else if (x < 0 && y < 0) // x and y are negative
		{
			angle = 180 + atanAngle;
		}
		else // x is positive and y is negative
		{
			angle = 360 + atanAngle;
		}
		
		return angle;
	}

	// Calculates a normalized vector direction from z rotation.
	public static Vector2 RotationZ2DirectionVec(float rotationZ)
	{
		Vector2 directionVec = new Vector2();
		directionVec.y = Mathf.Sin(rotationZ * Mathf.Deg2Rad);
		directionVec.x = Mathf.Cos(rotationZ * Mathf.Deg2Rad);
		return directionVec;
	}

	// Get the mouseposition and convert it to a vector 3 in world space.
	public static Vector3 MousePosInWorldSpace()
	{
		Vector3 pos = Input.mousePosition;
		pos.z = -1.0f;
		pos = Camera.main.ScreenToWorldPoint(pos);
		return pos;
	}
}
