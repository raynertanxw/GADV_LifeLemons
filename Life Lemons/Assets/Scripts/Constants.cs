using UnityEngine;
using System.Collections;

public static class Constants
{
	public static int NumOfLevels = 5;

	// Save data strings.
	public static string HIGHEST_CLEARED_LEVEL = "HIGHEST_CLEARED_LEVEL";
	public static string NUM_OF_QUARTERS = "NUM_OF_QUARTERS";

	// Scene filename strings.
	public static string MainMenuScene = "Main_Menu";
	public static string LevelScene = "Level_Scene";
	public static string TutorialScene = "Tutorial_Level";

	// UI Animation parameter names.
	public static string MainMenuToSettings = "MainMenuToSettings";
	public static string SettingsToMainMenu = "SettingsToMainMenu";
	public static string MainMenuToUpgrade = "MainMenuToUpgrade";
	public static string UpgradeToMainMenu = "UpgradeToMainMenu";

	// Other Animation parameter names.
	public static string toggle_shoot = "toggle_shoot";
	public static string toggle_collect = "toggle_collect";
	public static string Shoot = "Shoot";
	public static string PunchLeft = "PunchLeft";
	public static string PunchRight = "PunchRight";

	// Tag strings.
	public static string tagEnemy = "Enemy";
	public static string tagPlayer = "Player";
	public static string tagProjectile = "Projectile";

	// Other strings.
	public static string LevelFileNamePrefix = "Level_";
}
