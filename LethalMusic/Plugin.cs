using BepInEx;
using BepInEx.Logging;
using LethalMusic.Patches;
using LethalMusic.Utilities;
using UnityEngine;
//
namespace LethalMusic {
	[BepInPlugin("com.github.legoandmars.lethalmusic", "LethalMusic", "0.0.1")]
	public class Plugin : BaseUnityPlugin {
		public static ManualLogSource CLog;
		
		public static string PluginGUID = "com.github.legoandmars.lethalmusic";
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