using UnityEngine;
using System.Collections;

public class Environment_Boundary_Adjuster : MonoBehaviour
{
	private float minX, maxX, minY, maxY;
	private float boundaryThickness = 1.0f;
	public BoxCollider2D boundaryTop, boundaryRight, boundaryBottom, boundaryLeft;
	public BoxCollider2D UITrigger_AmmoLevelIndicator, UITrigger_Hearts, UITrigger_LevelProgressionBar;
	
	void Start()
	{
		float camDistance = Vector3.Distance(transform.position, Camera.main.transform.position);
		Vector2 bottomCorner = Camera.main.ViewportToWorldPoint(new Vector3(0,0, camDistance));
		Vector2 topCorner = Camera.main.ViewportToWorldPoint(new Vector3(1,1, camDistance));
		
		minX = bottomCorner.x;
		maxX = topCorner.x;
		minY = bottomCorner.y;
		maxY = topCorner.y;

		boundaryTop.offset = new Vector2(0f, maxY + boundaryThickness/2.0f);
		boundaryTop.size = new Vector2(maxX - minX + boundaryThickness * 2, boundaryThickness);

		boundaryRight.offset = new Vector2(maxX + boundaryThickness/2.0f, 0f);
		boundaryRight.size = new Vector2(boundaryThickness, maxY - minY);

		boundaryBottom.offset = new Vector2(0f, minY - boundaryThickness/2.0f);
		boundaryBottom.size = new Vector2(maxX - minX + boundaryThickness * 2, boundaryThickness);

		boundaryLeft.offset = new Vector2(minX - boundaryThickness/2.0f, 0f);
		boundaryLeft.size = new Vector2(boundaryThickness, maxY - minY);


		// Set up UITriggers
		if (Application.loadedLevelName == Constants.LevelScene)
		{
			// AmmoLevelIndicator is size 75x75 pivot at -55 -55 from top right.
			float pixelToWorldSpaceEquivilent = 1f/800f * (maxX - minX);
			UITrigger_AmmoLevelIndicator.offset = new Vector2(maxX - (55f * pixelToWorldSpaceEquivilent), maxY - (55f * pixelToWorldSpaceEquivilent));
			UITrigger_AmmoLevelIndicator.size = new Vector2(75f * pixelToWorldSpaceEquivilent, 75f * pixelToWorldSpaceEquivilent);

			// Hearts is size 30x30 and first heart pivot is 30x from left and -30y from top
			// Hearts are spaced 5 pixels apart.
			int numHearts = 5 + PlayerPrefs.GetInt(Constants.UPGRADE_DEFENSE_HEALTH);
			float heartSizeX = numHearts * (30f * pixelToWorldSpaceEquivilent) + ((numHearts-1) * (5f * pixelToWorldSpaceEquivilent));
			float heartOffsetX = (heartSizeX / 2.0f) + (30f * pixelToWorldSpaceEquivilent);
			UITrigger_Hearts.offset = new Vector2(minX + heartOffsetX, maxY - (30f * pixelToWorldSpaceEquivilent));
			UITrigger_Hearts.size = new Vector2(heartSizeX, 30f * pixelToWorldSpaceEquivilent);

			// LevelProgressionBar is size 300x50 starting from bottom right + (-5, 5).
			UITrigger_LevelProgressionBar.offset = new Vector2(245f * pixelToWorldSpaceEquivilent, -200f * pixelToWorldSpaceEquivilent);
			UITrigger_LevelProgressionBar.size = new Vector2(300f * pixelToWorldSpaceEquivilent, 50f * pixelToWorldSpaceEquivilent);
		}
	}
}
