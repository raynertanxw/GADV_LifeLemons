using UnityEngine;
using System.Collections;

public class MainMenu_FallingObject : MonoBehaviour
{
	private float rotationIncrement, fallingSpeed;
	private Vector3 rotationEulerVec, positionVec;

	void Awake()
	{
		if (Random.Range(0,2) == 1)
		{
			rotationIncrement = Random.Range(25.0f, 75.0f);
		}
		else
		{
			rotationIncrement = Random.Range(-75.0f, -25.0f);
		}

		fallingSpeed = Random.Range(0.2f, 2.0f);

		rotationEulerVec = transform.rotation.eulerAngles;
		positionVec = transform.position;
	}

	void Update()
	{
		rotationEulerVec.z += rotationIncrement * Time.deltaTime;
		transform.rotation = Quaternion.Euler(rotationEulerVec);

		positionVec.y -= fallingSpeed * Time.deltaTime;
		transform.position = positionVec;
	}
}
