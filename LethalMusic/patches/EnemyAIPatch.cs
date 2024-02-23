using LethalMusic.Utilities;
using LethalMusic.Managers;

namespace LethalMusic.Patches {
	public class EnemyAIPatch {
		public static void Patch() {
			On.EnemyAI.Start += StartPatch;
		}

		private static void StartPatch(On.EnemyAI.orig_Start orig, EnemyAI self) {
			orig(self);
			Console.LogInfo("EnemyAI.Start");
			Console.LogDebug($"EnemyAI: {self.enemyType.enemyName}");

			EnemyManager.ActiveEnemies.Add(self);
		}
	}
}