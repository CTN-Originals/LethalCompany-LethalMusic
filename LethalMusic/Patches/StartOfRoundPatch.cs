
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using GameNetcodeStuff;
using HarmonyLib;
using LethalMusic.Utilities;
using UnityEngine;

namespace LethalMusic.Patches {
	public class TestEnemyInstance {
		public Transform transform;
		public EnemyAI enemyAI;
		public Vector3 spawnPos;
		public int maxHealth;

		public void ReturnToSpawn() {
			if (enemyAI.enemyHP < maxHealth) {
				return;
			}
			transform.position = spawnPos;
		}
	}

	public class StartOfRoundPatch {
		public static void Patch() {
			On.StartOfRound.Update += UpdatePatch;
		}

		public static List<TestEnemyInstance> testEnemies = new List<TestEnemyInstance>();

		private static void UpdatePatch(On.StartOfRound.orig_Update orig, StartOfRound self) {
			foreach (TestEnemyInstance testEnemy in testEnemies) {
				if (!testEnemy.enemyAI.isEnemyDead) {
					testEnemy.ReturnToSpawn();
				}
				else {
					testEnemies.Remove(testEnemy);
				}
			}

			Console.LogInfo("StartOfRound.Update");
			orig(self);
		}

		public static EnemyType SpawnTestEnemy(string name) {
			EnemyType enemyType = GetEnemyTypeByName(name);
			if (enemyType == null) {
				Console.LogError($"Enemy type {name} not found");
				return null;
			}

			PlayerControllerB localPlayer = GameNetworkManager.Instance.localPlayerController;
			Vector3 spawnPos = localPlayer.transform.position + localPlayer.transform.forward * 5f;

			var res = RoundManager.Instance.SpawnEnemyGameObject(spawnPos, 0f, -1, enemyType);
			EnemyAI enemyAI = null;
			if (res.TryGet(out var networkObject)) {
				enemyAI = networkObject.GetComponent<EnemyAI>();
			}
			else {
				Console.LogError("Failed to spawn enemy");
				return null;
			}

			Console.LogDebug($"Spawned enemy: {enemyAI.gameObject.name}");

			TestEnemyInstance testEnemy = new() {
				transform = enemyAI.transform,
				enemyAI = enemyAI,
				spawnPos = spawnPos,
				maxHealth = enemyAI.enemyHP
			};

			testEnemies.Add(testEnemy);

			return enemyType;
		}

		public static EnemyType GetEnemyTypeByName(string name) {
			name = name.Replace(" ", "").ToLower();

			foreach (SelectableLevel level in StartOfRound.Instance.levels) {
				// Console.LogDebug($"-- Level: {level.sceneName} --");
				List<EnemyType> enemies = level.Enemies.ConvertAll(x => x.enemyType);
				enemies.AddRange(level.OutsideEnemies.ConvertAll(x => x.enemyType));
				enemies.AddRange(level.DaytimeEnemies.ConvertAll(x => x.enemyType));

				foreach (EnemyType enemy in enemies) {
					// Console.LogDebug($"Enemy: {enemy.enemyName}");
					string enemyName = enemy.enemyName.Replace(" ", "").ToLower();
					if (enemyName == name) {
						return enemy;
					}
				}
			}
			
			return null;
		}
	}
}