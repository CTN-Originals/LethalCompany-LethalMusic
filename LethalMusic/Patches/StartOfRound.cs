
using System.Reflection;
using HarmonyLib;
using LethalMusic.Utilities;
using UnityEngine;

namespace LethalMusic.Patches {
	public class StartOfRoundPatch {
		public static void Patch() {
			On.StartOfRound.TeleportPlayerInShipIfOutOfRoomBounds += TeleportPlayerInShipIfOutOfRoomBoundsPatch;
		}

		private static bool roundHasStarted = false;
		//!!Debug Patch
		private static void TeleportPlayerInShipIfOutOfRoomBoundsPatch(On.StartOfRound.orig_TeleportPlayerInShipIfOutOfRoomBounds orig, StartOfRound self) {
			if (roundHasStarted) {
				orig(self);
				return;
			}
			orig(self);

			MethodInfo findEntranceMethod = AccessTools.Method(typeof(RoundManager), "FindMainEntranceScript");
			EntranceTeleport entrance = (EntranceTeleport)findEntranceMethod.Invoke(null, new object[] { false });
			Transform entrancePoint = entrance.entrancePoint;

			Vector3 spawnPos = entrancePoint.position + entrancePoint.forward * 5f;

			Console.LogDebug($"Spawning enemy at {spawnPos.x} {spawnPos.y} {spawnPos.z}");

			var res = RoundManager.Instance.SpawnEnemyGameObject(spawnPos, 0f, 3, null);

			EnemyAI enemyAI = null;
			if (res.TryGet(out var networkObject)) {
				enemyAI = networkObject.GetComponent<EnemyAI>();
			}
			else {
				Console.LogError("Failed to spawn enemy");
				return;
			}

			Console.LogDebug($"Spawned enemy: {enemyAI.gameObject.name}");

			roundHasStarted = true;
		}
	}

}