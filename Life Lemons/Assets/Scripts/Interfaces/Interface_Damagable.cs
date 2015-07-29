using UnityEngine;
using System.Collections;

public interface IDamagable
{
	void TakeDamage(int damage); // Common TakeDamage Function for projectiles to call.
	void CheckGameOver(); // Common check to see if IDamagable object has died.
	// (SPELLING MISTAKE SHOULD BE CheckDead). Not changing in fear of breaking code this near to deadline.
}