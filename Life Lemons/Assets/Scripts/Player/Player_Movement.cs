using UnityEngine;
using System.Collections;

public class Player_Movement : MonoBehaviour
{
	public float speed = 10.0f; // Movement Speed.

	private Player_Combat playerCombat;
	private Vector2 directionVec;

	void Awake()
	{
		// Reference to PlayerCombat script to check for malfunction states.
		playerCombat = gameObject.GetComponent<Player_Combat>();
	}

	void Update()
	{
		// If the game is paused, immediately return to do nothing.
		if(Time.timeScale == 0)return;

		// If the game is over don't do anything.
		if(GameManager.instance.gameObject == true)return;

		// Check if the player has a malfunction before allowing it to move.
		if (playerCombat.hasMalfunction == false)
		{
			// Move the player's transform according to Horizontal and Vertical raw axis values.
			gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * speed;

			// Calculate out the angle of which the player turns to face the mouse cursor.
			directionVec = (Utilities.MousePosInWorldSpace() - transform.position);
			transform.rotation = Quaternion.Euler(0, 0, Utilities.DirectionVec2RotationZ(directionVec) % 360);
		}
	}
}
