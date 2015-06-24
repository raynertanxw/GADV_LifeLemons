using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

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

	// Uses txtFileToStringArray to read the level.txt file and obtain the string array of instructions for the level. 
	public static string[] getLevelTxtInstructions(string levelName)
	{
		string[] rawStringArray = txtFileToStringArray(Application.dataPath + "/Levels/" + levelName + ".txt");
		if (rawStringArray != null)
		{
			return rawStringArray;
		}
		else
		{
			Debug.Log("Error in reading file: " + levelName);
			return null;
		}
	}
	
	// Read in the txt file named filename, return a string array of each line in the txt file seperated by \n.
	public static string[] txtFileToStringArray(string fileName)
	{
		try
		{
			List<string> stringList = new List<string>();
			string currentLine;

			StreamReader sr = new StreamReader(fileName);

			// Read the first line first. do-while loop to ensure first line is read first. If any of the lines are null, stop reading.
			do
			{
				currentLine = sr.ReadLine();
				if (currentLine != null)
				{
					stringList.Add(currentLine);
				}
			}
			while (currentLine != null);

			sr.Close();

			return stringList.ToArray();
		}
		catch (IOException e)
		{
			Debug.Log(e.Message);
			return null;
		}
	}
}
