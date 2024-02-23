using System.Collections.Generic;
using GameNetcodeStuff;
using UnityEngine;
using LethalMusic.Utilities;

namespace LethalMusic.Patches {
	public class StartOfRoundPatch {
		public static void Patch() {
			On.StartOfRound.Start += StartPatch;
		}

		private static void StartPatch(On.StartOfRound.orig_Start orig, StartOfRound self) {
			orig(self);
			Console.LogInfo("StartOfRound.Start");
			Managers.EnemyManager.Start();
		}
	}
}