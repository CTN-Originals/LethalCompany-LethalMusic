using BepInEx;
using BepInEx.Logging;
using LethalMusic.Utilities;

namespace LethalMusic {
	[BepInPlugin("com.ctnoriginals.lethalmusic", "LethalMusic", "0.0.1")]
	public class Plugin : BaseUnityPlugin {
		public static ManualLogSource CLog;

		public static string PluginGUID = "com.ctnoriginals.lethalmusic";
		public static string PluginName = "LethalMusic";
		public static string PluginVersion = "0.0.1";

		public static bool DebugMode = false;

		public void Awake() {
			#if DEBUG
				DebugMode = true;
			#endif

			CLog = Logger;
			CLog.LogInfo($"Plugin {PluginName} is loaded! Version: {PluginVersion} ({(DebugMode ? "Debug" : "Release")})");

			// Patches
			Patches.StartOfRoundPatch.Patch();
			Patches.PlayerControllerBPatch.Patch();
			Patches.EnemyAIPatch.Patch();
		}

		private void Update() {
			// Console.LogDebug("Update");
			// if (Input.GetKeyDown(KeyCode.Space)) {
			// 	Console.LogInfo("Spawning enemy keybind pressed!");
			// 	StartOfRoundPatch.SpawnTestEnemy("Crawler");
			// }
		}
	}
}


/* //? Useful Variables/Methods
	_ EnemyAI
		> public PlayerControllerB targetPlayer
		> public bool movingTowardsTargetPlayer
		> public bool moveTowardsDestination = true
		> public Vector3 destination
		> public bool isEnemyDead
		() public PlayerControllerB CheckLineOfSightForPlayer(float width = 45f, int range = 60, int proximityAwareness = -1)
		() public PlayerControllerB CheckLineOfSightForClosestPlayer(float width = 45f, int range = 60, int proximityAwareness = -1, float bufferDistance = 0f)
		() public PlayerControllerB[] GetAllPlayersInLineOfSight(float width = 45f, int range = 60, Transform eyeObject = null, float proximityCheck = -1f, int layerMask = -1)
*/