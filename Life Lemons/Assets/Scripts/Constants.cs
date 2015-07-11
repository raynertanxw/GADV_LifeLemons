using UnityEngine;
using System.Collections;

public static class Constants
{
	public static int NumOfLevels = 30;

	// Save data strings.
	public static string HIGHEST_CLEARED_LEVEL = "HIGHEST_CLEARED_LEVEL";
	public static string NUM_OF_QUARTERS = "NUM_OF_QUARTERS";
	public static string SELECTED_LEVEL = "SELECTED_LEVEL";
	
	public static string UPGRADE_OFFENSE_BULLET_SPEED = "UPGRADE_OFFENSE_BULLET_SPEED";
	public static string UPGRADE_OFFENSE_AMMO_COST = "UPGRADE_OFFENSE_AMMO_COST";
	public static string UPGRADE_OFFENSE_CONVERSION_RATE = "UPGRADE_OFFENSE_CONVERSION_RATE";
	public static string UPGRADE_DEFENSE_HEALTH = "UPGRADE_DEFENSE_HEALTH";
	public static string UPGRADE_DEFENSE_MOVEMENT_SPEED = "UPGRADE_DEFENSE_MOVEMENT_SPEED";
	public static string UPGRADE_DEFENSE_REPAIR_TIME = "UPGRADE_DEFENSE_REPAIR_TIME";

	// Scene filename strings.
	public static string MainMenuScene = "Main_Menu";
	public static string LevelScene = "Level_Scene";
	public static string TutorialScene = "Tutorial_Level";

	// UI Animation parameter names.
	public static string MainMenuToSettings = "MainMenuToSettings";
	public static string SettingsToMainMenu = "SettingsToMainMenu";
	public static string MainMenuToUpgrade = "MainMenuToUpgrade";
	public static string UpgradeToMainMenu = "UpgradeToMainMenu";

	public static string FadeInGameOver = "FadeInGameOver";
	public static string GameOverToLevelSelect = "GameOverToLevelSelect";
	public static string LevelSelectToGameOver = "LevelSelectToGameOver";
	public static string PauseToLevelSelect = "PauseToLevelSelect";
	public static string LevelSelectToPause = "LevelSelectToPause";
	public static string TransitionToPause = "TransitionToPause";
	public static string TransitionFromPause = "TransitionFromPause";

	public static string ScrollNext = "ScrollNext";
	public static string ScrollPrevious = "ScrollPrevious";

	public static string OffenseToDefense = "OffenseToDefense";
	public static string DefenseToOffense = "DefenseToOffense";

	// Other Animation parameter names.
	public static string toggle_shoot = "toggle_shoot";
	public static string toggle_collect = "toggle_collect";
	public static string Shoot = "Shoot";
	public static string Die = "Die";
	public static string PunchLeft = "PunchLeft";
	public static string PunchRight = "PunchRight";

	// Tag strings.
	public static string tagEnemy = "Enemy";
	public static string tagPlayer = "Player";
	public static string tagProjectile = "Projectile";

	// Couroutine names.
	public static string shootAtPlayer = "shootAtPlayer";
	public static string punchAtPlayer = "punchAtPlayer";
	public static string fadeOutLevelText = "fadeOutLevelText";

	// Sprite Renderer Sorting Layer Names.
	public static string DeadEnemy = "DeadEnemy";

	// Other strings.
	public static string LevelFileNamePrefix = "Level_";
	public static string LevelButtonNamePrefix = "LevelButton_Level_";
}
