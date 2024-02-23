using System.Collections.Generic;
using GameNetcodeStuff;
using UnityEngine;

using LethalMusic.Utilities;
using System.Linq;

namespace LethalMusic.Managers {
	public static class EnemyManager {
		public static List<EnemyType> Enemies = new List<EnemyType>();
		public static List<EnemyAI> ActiveEnemies = new List<EnemyAI>();

		public static void Start() {
			Console.LogInfo("EnemyManager.Start");
			Initialize();
		}

		public static void Initialize() {
			//TODO Wait for x seconds to get all enemies
				//* this is to make sure that all enemies are loaded (also the modded ones)
			//> Go through all levels and get all enemies
			foreach (SelectableLevel level in StartOfRound.Instance.levels) {
				// Console.LogDebug($"-- Level: {level.sceneName} --");
				List<EnemyType> enemies = level.Enemies.ConvertAll(x => x.enemyType);
				enemies.AddRange(level.OutsideEnemies.ConvertAll(x => x.enemyType));
				enemies.AddRange(level.DaytimeEnemies.ConvertAll(x => x.enemyType));

				foreach (EnemyType enemy in enemies) {
					if (!Enemies.Contains(enemy)) {
						Console.LogDebug($"Enemy: {enemy.enemyName}");
						Enemies.Add(enemy);
					}
				}
			}
		}

		public static void Update() {
			if (Plugin.DebugMode) {
				EnemyTesting.Update();
			}
		}

		

		public static EnemyType GetEnemyTypeByName(string name) {
			name = name.Replace(" ", "").ToLower();

			foreach (EnemyType enemy in Enemies) {
				// Console.LogDebug($"Enemy: {enemy.enemyName}");
				string enemyName = enemy.enemyName.Replace(" ", "").ToLower();
				if (enemyName == name) {
					return enemy;
				}
			}
			return null;
		}
	}

	public class EnemyTesting {
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

		public static List<TestEnemyInstance> testEnemies = new List<TestEnemyInstance>();

		public static void Update() {
			foreach (TestEnemyInstance testEnemy in testEnemies) {
				if (!testEnemy.enemyAI.isEnemyDead) {
					testEnemy.ReturnToSpawn();
				}
				else {
					testEnemies.Remove(testEnemy);
					testEnemy.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
					continue;
				}

				if (Plugin.OutputDebugLogs) {
					string[] logText = {
						$"{testEnemy.enemyAI.enemyType.enemyName}:",
						$"\ttargetPlayer: {Plugin.LocalPlayer.name}",
						$"\tmovingTowardsTargetPlayer: {testEnemy.enemyAI.movingTowardsTargetPlayer}",
						$"\tmoveTowardsDestination: {testEnemy.enemyAI.moveTowardsDestination}",
						$"\tisEnemyDead: {testEnemy.enemyAI.isEnemyDead}",
						"",
						$"\tinsanityLevel: {StartOfRound.Instance.fearLevel}",
						$"\tdestination: {testEnemy.enemyAI.destination}",
					};
					Console.LogDebug(string.Join("\n", logText));
				}
			}
		}

		//? Spawns a random enemy by default
		public static EnemyType SpawnTestEnemy(int type = -1, string name = null) {
			EnemyType enemyType = (name == null) ? null : EnemyManager.GetEnemyTypeByName(name);
			if (enemyType == null && name != null) {
				Console.LogError($"Enemy type {name} not found");
				return null;
			}

			Vector3 spawnPos = Plugin.LocalPlayer.transform.position + Plugin.LocalPlayer.transform.forward * 5f;

			var res = RoundManager.Instance.SpawnEnemyGameObject(spawnPos, 0f, type, enemyType);
			EnemyAI enemyAI = null;
			if (res.TryGet(out var networkObject)) {
				enemyAI = networkObject.GetComponent<EnemyAI>();
				enemyAI.enemyType.canDie = true;
				enemyAI.enemyType.canBeStunned = true;
				enemyAI.enemyType.destroyOnDeath = true;
				enemyAI.enemyHP = 3;
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
	}
}

/* //? Stock enemy names
	> Centipede
	> Bunker Spider
	> Hoarding bug
	> Flowerman
	> Crawler
	> Blob
	> Girl
	> Puffer
	> Nutcracker
	> MouthDog
	> ForestGiant
	> Earth Leviathan
	> Red Locust Bees
	> Manticoil
	> Docile Locust Bees
	> Baboon hawk
	> Spring
	> Jester
	> Lasso
	> Masked
*/