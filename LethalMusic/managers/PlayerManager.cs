using UnityEngine;
using System.Collections.Generic;
using GameNetcodeStuff;

using LethalMusic.Utilities;
using Unity.Netcode;

namespace LethalMusic.Managers {
	public static class PlayerManager {
		public static PlayerControllerB LocalPlayer;
		private static GameObject debugObjectHolder;

		public static void Initialize() {
			Console.LogInfo("PlayerManager.Initialize");
			LocalPlayer = GameNetworkManager.Instance.localPlayerController;
			Console.LogInfo($"LocalPlayer: {LocalPlayer.name}, ID: {LocalPlayer.OwnerClientId}, NetworkID: {LocalPlayer.NetworkObjectId}");
		}

		public static void Update() {
			if (Plugin.OutputDebugLogs) {
				GetEnemiesInLineOfSight();
			}
			// if (Plugin.OutputDebugLogs) {
			// 	//? position the debug object
			// 	if (debugObject == null) CreateDebugObject();
			// 	debugObject.transform.position = LocalPlayer.transform.position;
			// 	// debugObject.transform.LookAt(direction + playerPos);
			// 	debugObject.transform.localScale = new Vector3(1, 1, 1);

			// 	// Console.LogDebug($"capsule: {debugObject.transform.position} -> {direction} ({hit.distance})");
			// }
		}

		public static EnemyAI[] GetEnemiesInLineOfSight(float width = 45f, int range = 60, float proximityCheck = -1f) {
			List<EnemyAI> list = new List<EnemyAI>();

			Camera playerCam = LocalPlayer.gameplayCamera;
			Vector3 playerPos = playerCam.transform.position;
			// LayerMask mask = LayerMask.GetMask("Player", "IgnoreRaycast");
			LayerMask mask = ~(1 << LayerMask.GetMask("Player"));
			// mask &= ~(1 << LocalPlayer.gameObject.layer);
			
			Console.LogDebug($"Mask: {(int)mask} ({~(int)mask})");

			//? is player outside? and is it foggy?
			if (!LocalPlayer.isInsideFactory && TimeOfDay.Instance.currentLevelWeather == LevelWeatherType.Foggy) { 
				Mathf.Clamp(range, 0, 30); //? reduce max range due to foggy view distance
			}
			
			foreach (/*EnemyAI*/ EnemyTesting.TestEnemyInstance testEnemy in /*EnemyManager.ActiveEnemies*/ EnemyTesting.testEnemies) {
				EnemyAI enemy = testEnemy.enemyAI;
				Vector3 direction = playerCam.transform.TransformDirection(Vector3.forward);
				
				// Physics.Linecast(playerPos, enemy.transform.position, out RaycastHit hit, StartOfRound.Instance.collidersAndRoomMaskAndDefault, QueryTriggerInteraction.Ignore);
				// if (Physics.Raycast(playerPos + direction * 1f, direction, out RaycastHit hit, range, mask, QueryTriggerInteraction.Ignore)) {
				if (Physics.Raycast(playerPos + direction * 1f, direction, out RaycastHit hit, range, mask, QueryTriggerInteraction.Ignore)) {
					if (hit.transform == enemy.transform || enemy.transform.ContainsChild(hit.transform)) {
						Console.LogDebug($"Hit Enemy: {enemy.name}");
						list.Add(enemy);
					}
					else {
						Console.LogDebug($"Hit: {hit.transform.name}");
					}

					Console.LogDebug($"Hit Layer: {hit.transform.gameObject.layer} ({LayerMask.LayerToName(hit.transform.gameObject.layer)})");
					Console.LogDebug($"Direction: {direction}");

					if (Plugin.OutputDebugLogs) {
						if (debugObjectHolder == null) {
							debugObjectHolder = GameObject.Instantiate(new GameObject(name: "DebugObjectHolder"), LocalPlayer.transform);
							debugObjectHolder.transform.SetParent(null);
						}

						LineRenderer debugLine = debugObjectHolder.GetComponent<LineRenderer>();
						if (debugLine == null) {
							debugLine = debugObjectHolder.AddComponent<LineRenderer>();
							debugLine.material = new Material(Shader.Find("Sprites/Default"));
							debugLine.startColor = new Color(1, 0.3f, 0, 0.01f);
							debugLine.endColor = new Color(1, 0.3f, 0, 0.5f);
							
							debugLine.useWorldSpace = true;
							debugLine.alignment = LineAlignment.TransformZ;
							debugLine.numCapVertices = 2;
							debugLine.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;

							debugLine.startWidth = 0f;
							debugLine.endWidth = 1;
							
							debugLine.positionCount = 2;
						}

						debugObjectHolder.transform.position = playerPos;
						debugObjectHolder.transform.rotation = playerCam.transform.rotation;
						debugObjectHolder.transform.LookAt(hit.point);
						debugObjectHolder.transform.localScale = new Vector3(width, 1, 1);

						debugLine.SetPosition(0, new Vector3(playerPos.x, playerPos.y, playerPos.z));
						debugLine.SetPosition(1, hit.point);

						Console.LogDebug($"line: {playerPos} -> {hit.point} ({hit.distance})");
					}
				}
			}

			return list.ToArray();
		}

		private static void CreateDebugObject() {
			Console.LogDebug("Creating DebugObject");
			debugObjectHolder = GameObject.CreatePrimitive(PrimitiveType.Cube);

			Console.LogDebug("Setting DebugObject properties");
			debugObjectHolder.name = "DebugObject";

			Console.LogDebug("Registering DebugObject to Network");
			NetworkObject netObj = NetworkHandler.RegisterObjectToNetwork(debugObjectHolder);
			Console.LogDebug("done");

			//? give it a material
			Console.LogDebug("Creating Material");
			Material material = new Material(Shader.Find("Standard")) {
				color = Color.red,
				mainTexture = Texture2D.whiteTexture
			};

			Console.LogDebug("Getting MeshRenderer");
			MeshRenderer rend = netObj.GetComponent<MeshRenderer>();
			Console.LogDebug("finally");
			rend.material = material;
			Console.LogDebug("done");
		}
	}
}