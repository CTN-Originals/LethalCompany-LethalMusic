using System.Collections;
using GameNetcodeStuff;
using LethalMusic.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LethalMusic.Patches {
	public class PlayerControllerBPatch {
		private static bool spawnOnCooldown = false;
		private static float spawnCooldown = 1f;


		public static void Patch() {
			On.GameNetcodeStuff.PlayerControllerB.Start += StartPatch;
			On.GameNetcodeStuff.PlayerControllerB.InspectItem_performed += InspectItem_performedPatch;
		}

		private static void StartPatch(On.GameNetcodeStuff.PlayerControllerB.orig_Start orig, PlayerControllerB self) {
			orig(self);
			Console.LogInfo("PlayerControllerB.Start");
		}

		//!! For Debugging Purposes Only!!
		private static void InspectItem_performedPatch(On.GameNetcodeStuff.PlayerControllerB.orig_InspectItem_performed orig, PlayerControllerB self, InputAction.CallbackContext context) {
			Console.LogInfo("PlayerControllerB.InspectItem_performed");
			if (!spawnOnCooldown) {
				StartOfRoundPatch.SpawnTestEnemy("Crawler");
				spawnOnCooldown = true;
				self.StartCoroutine(StartSpawnCooldown());
			}
			orig(self, context);
		}

		private static IEnumerator StartSpawnCooldown() {
			yield return new WaitForSeconds(spawnCooldown);
			spawnOnCooldown = false;
		}
	}
}
