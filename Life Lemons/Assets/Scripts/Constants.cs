using UnityEngine;
using System.Collections;

public enum GameMode {normal, endless};

public static class Constants
{
	// Non string constants.
	public static int NumOfLevels = 30;
	public static GameMode gameMode = GameMode.normal;
	public static bool toggledUpgradeFromLevel = false;

	// Save data strings.
	public static string HIGHEST_CLEARED_LEVEL = "HIGHEST_CLEARED_LEVEL";
	public static string NUM_OF_QUARTERS = "NUM_OF_QUARTERS";
	public static string SELECTED_LEVEL = "SELECTED_LEVEL";
	public static string BEST_SURVIVAL_TIME = "BEST_SURVIVAL_TIME";
	
	public static string UPGRADE_OFFENSE_BULLET_SPEED = "UPGRADE_OFFENSE_BULLET_SPEED";
	public static string UPGRADE_OFFENSE_AMMO_COST = "UPGRADE_OFFENSE_AMMO_COST";
	public static string UPGRADE_OFFENSE_CONVERSION_RATE = "UPGRADE_OFFENSE_CONVERSION_RATE";
	public static string UPGRADE_DEFENSE_HEALTH = "UPGRADE_DEFENSE_HEALTH";
	public static string UPGRADE_DEFENSE_MOVEMENT_SPEED = "UPGRADE_DEFENSE_MOVEMENT_SPEED";
	public static string UPGRADE_DEFENSE_REPAIR_TIME = "UPGRADE_DEFENSE_REPAIR_TIME";

	public static string SETTINGS_RESOLUTION_ID = "SETTINGS_RESOLUTION_ID";

	// Scene filename strings.
	public static string MainMenuScene = "Main_Menu";
	public static string LevelScene = "Level_Scene";
	public static string TutorialScene = "Tutorial_Level";

	// UI Animation parameter names.
	public static string MainMenuToSettings = "MainMenuToSettings";
	public static string SettingsToMainMenu = "SettingsToMainMenu";
	public static string MainMenuToUpgrade = "MainMenuToUpgrade";
	public static string UpgradeToMainMenu = "UpgradeToMainMenu";
	public static string MainMenuToCredits = "MainMenuToCredits";
	public static string CreditsToMainMenu = "CreditsToMainMenu";
	public static string toggleUpgradeFromLevel = "toggleUpgradeFromLevel";

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
	public static string trigger_player_death = "trigger_player_death";
	public static string Shoot = "Shoot";
	public static string Die = "Die";
	public static string PunchLeft = "PunchLeft";
	public static string PunchRight = "PunchRight";

	// GameObject Tags.
	public static string tagEnemy = "Enemy";
	public static string tagPlayer = "Player";
	public static string tagProjectile = "Projectile";
	public static string FallingObject = "Falling Object";

	// Couroutine names.
	public static string shootAtPlayer = "shootAtPlayer";
	public static string punchAtPlayer = "punchAtPlayer";
	public static string fadeOutLevelText = "fadeOutLevelText";
	public static string SpawnEndless = "SpawnEndless";
	public static string increaseProgressionBarFill = "increaseProgressionBarFill";
	public static string ScrollCredits = "ScrollCredits";

	// Sprite Renderer Sorting Layer Names.
	public static string DeadEnemy = "DeadEnemy";

	// Prefix strings.
	public static string LevelFileNamePrefix = "Level_";
	public static string LevelButtonNamePrefix = "LevelButton_Level_";

	// Tips text
	public static string[] tips = {
		"Having trouble? Remember that you can upgrade your stats by going to the main menu.",
		"Dual wielding lemons won't hurt you if you stay still. The lemons will fly past you",
		"To deal with single shooting lemons, just use your funnel and directly face them.",
		"When enemies circle around you, stay calm! Lead your shots in a circular fashion.",
		"Melee units don't shoot lemons, sometimes it's best to leave one or two shooters alive.",
		"You can collect lemons from dual wielding lemons by staying still and spinning fast.",
		"Melee units don't provide lemons, get rid of them quickly before they hoard you."
	};
}
