using System.Collections;
using GameNetcodeStuff;
using LethalMusic.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LethalMusic.Patches {
	public class PlayerControllerBPatch {
		public static void Patch() {
			On.GameNetcodeStuff.PlayerControllerB.Start += StartPatch;
			On.GameNetcodeStuff.PlayerControllerB.InspectItem_performed += InspectItem_performedPatch;
			On.GameNetcodeStuff.PlayerControllerB.Emote1_performed += Emote1_performedPatch;
			On.GameNetcodeStuff.PlayerControllerB.Emote2_performed += Emote2_performedPatch;
		}

		private static void StartPatch(On.GameNetcodeStuff.PlayerControllerB.orig_Start orig, PlayerControllerB self) {
			orig(self);
			Console.LogInfo("PlayerControllerB.Start");
		}

		//!! For Debugging Purposes Only!!
		#region Debugging
		private static bool spawnOnCooldown = false;
		private static float spawnCooldown = 1f;
		private static void InspectItem_performedPatch(On.GameNetcodeStuff.PlayerControllerB.orig_InspectItem_performed orig, PlayerControllerB self, InputAction.CallbackContext context) {
			StartSpawnEnemy(self, -1, "Crawler"); orig(self, context);
		}
		private static void Emote1_performedPatch(On.GameNetcodeStuff.PlayerControllerB.orig_Emote1_performed orig, PlayerControllerB self, InputAction.CallbackContext context) {
			StartSpawnEnemy(self, Random.Range(-3, -1)); orig(self, context);
		}
		private static void Emote2_performedPatch(On.GameNetcodeStuff.PlayerControllerB.orig_Emote2_performed orig, PlayerControllerB self, InputAction.CallbackContext context) {
			StartSpawnEnemy(self); orig(self, context);
		}
		
		private static void StartSpawnEnemy(PlayerControllerB self, int type = -1, string enemyName = null) {
			if (spawnOnCooldown) return;
			StartOfRoundPatch.SpawnTestEnemy(type, enemyName);
			spawnOnCooldown = true;
			self.StartCoroutine(StartSpawnCooldown());
		}

		private static IEnumerator StartSpawnCooldown() {
			yield return new WaitForSeconds(spawnCooldown);
			spawnOnCooldown = false;
		}
		#endregion
	}
}
