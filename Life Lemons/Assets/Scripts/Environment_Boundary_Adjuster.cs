using UnityEngine;
using System.Collections;

public class Environment_Boundary_Adjuster : MonoBehaviour
{
	private float minX, maxX, minY, maxY;
	private float boundaryThickness = 1.0f;
	public BoxCollider2D boundaryTop, boundaryRight, boundaryBottom, boundaryLeft;
	
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
	}
}
