using System.Collections;
using GameNetcodeStuff;
using UnityEngine;
using UnityEngine.InputSystem;

using LethalMusic.Managers;
using LethalMusic.Utilities;

namespace LethalMusic.Patches {
	public class PlayerControllerBPatch {
		public static void Patch() {
			On.GameNetcodeStuff.PlayerControllerB.Start += StartPatch;
			On.GameNetcodeStuff.PlayerControllerB.ConnectClientToPlayerObject += ConnectClientToPlayerObjectPatch;
		}

		private static void StartPatch(On.GameNetcodeStuff.PlayerControllerB.orig_Start orig, PlayerControllerB self) {
			orig(self);
			Console.LogInfo("PlayerControllerB.Start");
		}
		private static void ConnectClientToPlayerObjectPatch(On.GameNetcodeStuff.PlayerControllerB.orig_ConnectClientToPlayerObject orig, PlayerControllerB self) {
			orig(self);
			Console.LogInfo("PlayerControllerB.ConnectClientToPlayerObject");
			if (PlayerManager.LocalPlayer == null) PlayerManager.Initialize();
		}

		#region Debugging //! For Debugging Purposes Only!! (Only works if dll is compiled with DEBUG flag)
			private static bool spawnOnCooldown = false;
			private static float spawnCooldown = 1f;
			
			public static void StartSpawnEnemy(PlayerControllerB self, int type = -1, string enemyName = null) {
				if (spawnOnCooldown) return;
				EnemyTesting.SpawnTestEnemy(type, enemyName);
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
