using BepInEx;
using BepInEx.Logging;
using GameNetcodeStuff;
using LethalMusic.Utilities;
using UnityEngine.InputSystem;

namespace LethalMusic {
	[BepInPlugin("com.ctnoriginals.lethalmusic", "LethalMusic", "0.0.1")]
	public class Plugin : BaseUnityPlugin {
		public static ManualLogSource CLog;

		public static string PluginGUID = "com.ctnoriginals.lethalmusic";
		public static string PluginName = "LethalMusic";
		public static string PluginVersion = "0.0.1";

		public static bool DebugMode = false;
		public static bool OutputDebugLogs = false;

		public static PlayerControllerB LocalPlayer;

		private void Awake() {
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

		private void Start() {
		}

		private void Update() {
			Managers.EnemyManager.Update();

			if (DebugMode) {
				if (Keyboard.current.numpadMultiplyKey.wasPressedThisFrame) OutputDebugLogs = true;
				else if (OutputDebugLogs) OutputDebugLogs = false;
			}
		}
	}
}


/* //? Useful Variables/Methods
	_ EnemyAI
		> public PlayerControllerB targetPlayer
		> public bool movingTowardsTargetPlayer
		> public bool moveTowardsDestination = true
		> public bool isEnemyDead
		> public Vector3 destination
		() public PlayerControllerB CheckLineOfSightForPlayer(float width = 45f, int range = 60, int proximityAwareness = -1)
		() public PlayerControllerB CheckLineOfSightForClosestPlayer(float width = 45f, int range = 60, int proximityAwareness = -1, float bufferDistance = 0f)
		() public PlayerControllerB[] GetAllPlayersInLineOfSight(float width = 45f, int range = 60, Transform eyeObject = null, float proximityCheck = -1f, int layerMask = -1)
		() public PlayerControllerB GetClosestPlayer(bool requireLineOfSight = false, bool cannotBeInShip = false, bool cannotBeNearShip = false)
	
	_ PlayerControllerB
	
	_ StartOfRound
		> public float fearLevel
			- <= 0: No Fear
			- ~0.5: looking at monster
			- ~0.8: Discovered dead body
			- >= 1: Chased by monster

*/