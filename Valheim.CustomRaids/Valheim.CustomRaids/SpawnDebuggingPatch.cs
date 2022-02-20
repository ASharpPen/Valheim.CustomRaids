#if DEBUG && VERBOSE

using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Valheim.CustomRaids;


namespace Valheim.CustomRaids.Patches
{
	[HarmonyPatch(typeof(MonsterAI), "UpdateAI", new Type[] { typeof(float) })]
	public static class MonsterAIPatch
	{
		private static void Postfix(ref MonsterAI __instance, float dt)
		{
			string status = (string)AccessTools.Field(typeof(MonsterAI), "m_aiStatus").GetValue(__instance);

			if (status != null)
			{
				if ("Trying to despawn " == status)
				{
					Log.LogTrace("Monster status: " + status);
				}

			}
		}
	}

	[HarmonyPatch(typeof(SpawnSystem), "UpdateSpawnList")]
	public static class SpawnSystemPatch
	{
		private static bool Prefix(ref SpawnSystem __instance, List<SpawnSystem.SpawnData> spawners, DateTime currentTime, bool eventSpawners)
		{
			if (!eventSpawners)
			{
				return false;
			}

			string str = eventSpawners ? "e_" : "b_";
			int num = 0;
			foreach (SpawnSystem.SpawnData spawnData in spawners)
			{

				num++;

				if (!spawnData.m_enabled)
				{
					Log.LogTrace($"Skipping {spawnData.m_name} due to {nameof(spawnData.m_enabled)}");
					continue;
				}
				var heightMap = (Heightmap)AccessTools.Field(typeof(SpawnSystem), "m_heightmap").GetValue(__instance);

				if (!heightMap.HaveBiome(spawnData.m_biome))
				{
					Log.LogTrace($"Skipping {spawnData.m_name} due to spawn biome {spawnData.m_biome} not in {heightMap}");
					continue;
				}

				if (true)
				{
					var view = (ZNetView)AccessTools.Field(typeof(SpawnSystem), "m_nview").GetValue(__instance);

					int stableHashCode = (str + spawnData.m_prefab.name + num.ToString()).GetStableHashCode();
					DateTime d = new DateTime(view.GetZDO().GetLong(stableHashCode, 0L));
					TimeSpan timeSpan = currentTime - d;
					int num2 = Mathf.Min(spawnData.m_maxSpawned, (int)(timeSpan.TotalSeconds / (double)spawnData.m_spawnInterval));
					if (num2 > 0)
					{
						view.GetZDO().Set(stableHashCode, currentTime.Ticks);
					}

					if(num2 == 0)
                    {
						Log.LogTrace($"Skipping {spawnData.m_name} due to {nameof(spawnData.m_maxSpawned)}");
					}

					for (int i = 0; i < num2; i++)
					{
						if (UnityEngine.Random.Range(0f, 100f) > spawnData.m_spawnChance)
						{
							Log.LogTrace("Nope. Random chance not hit.");
							continue;
						}

						var globalKeyConstraint = (!string.IsNullOrEmpty(spawnData.m_requiredGlobalKey) && !ZoneSystem.instance.GetGlobalKey(spawnData.m_requiredGlobalKey));
						var environmentConstraint = (spawnData.m_requiredEnvironments.Count > 0 && !EnvMan.instance.IsEnvironment(spawnData.m_requiredEnvironments));
						var dayConstraint = (!spawnData.m_spawnAtDay && EnvMan.instance.IsDay());
						var nightConstraint = (!spawnData.m_spawnAtNight && EnvMan.instance.IsNight());
						var instanceCount = SpawnSystem.GetNrOfInstances(spawnData.m_prefab, Vector3.zero, 0f, eventSpawners, false);
						var instanceCountConstraint = instanceCount >= spawnData.m_maxSpawned;

						if(globalKeyConstraint)
                        {
							Log.LogTrace($"Skipping {spawnData.m_name} due to global key constraint");
							continue;
						}
						if (environmentConstraint)
						{
							Log.LogTrace($"Skipping {spawnData.m_name} due to environment constraint");
							continue;
						}
						if (dayConstraint)
						{
							Log.LogTrace($"Skipping {spawnData.m_name} due to day constraint");
							continue;
						}
						if (nightConstraint)
						{
							Log.LogTrace($"Skipping {spawnData.m_name} due to night constraint");
							continue;
						}
						if (instanceCountConstraint)
						{
							Log.LogTrace($"Skipping {spawnData.m_name} due to mob count constraint {spawnData.m_maxSpawned} < {instanceCount} spawned");
							continue;
						}

						Vector3 vector;
						Player player;

						var nearPlayers = (List<Player>)AccessTools.Field(typeof(SpawnSystem), "m_nearPlayers").GetValue(__instance);

						if (!FindBaseSpawnPoint(spawnData, nearPlayers, out vector, out player))
						{
							Log.LogTrace("No base spawn point nearby");
							continue;
						}

						bool distanceClose;
						if (distanceClose = spawnData.m_spawnDistance <= 0f)
						{
							Log.LogTrace("Spawn distance 0 or less");
						}

						bool instanceInRange;
						if (instanceInRange = !HaveInstanceInRange(spawnData.m_prefab, vector, spawnData.m_spawnDistance))
						{
							Log.LogTrace($"Instance in range: {instanceInRange}");
						}

						if (distanceClose || !HaveInstanceInRange(spawnData.m_prefab, vector, spawnData.m_spawnDistance))
						{
							int num3 = UnityEngine.Random.Range(spawnData.m_groupSizeMin, spawnData.m_groupSizeMax + 1);
							float d2 = (num3 > 1) ? spawnData.m_groupRadius : 0f;
							int num4 = 0;

							Log.LogTrace($"Start spawning {num3} {spawnData.m_prefab.name}.");

							for (int j = 0; j < num3 * 2; j++)
							{
								Vector2 insideUnitCircle = UnityEngine.Random.insideUnitCircle;
								Vector3 a = vector + new Vector3(insideUnitCircle.x, 0f, insideUnitCircle.y) * d2;
								if (IsSpawnPointGood(spawnData, ref a))
								{
									Log.LogTrace($"Good spawn {num4}");

									Spawn(__instance.m_levelupChance, spawnData, a + Vector3.up * spawnData.m_groundOffset, eventSpawners);
									num4++;
									if (num4 >= num3)
									{
										break;
									}
								}
							}
							Log.LogTrace(string.Concat(new object[]
							{
									"Spawned ",
									spawnData.m_prefab.name,
									" x ",
									num4
							}));
						}
					}
				}
			}
			return false;
		}

		private static bool HaveInstanceInRange(GameObject prefab, Vector3 centerPoint, float minDistance)
		{
			string name = prefab.name;
			if (prefab.GetComponent<BaseAI>() != null)
			{
				foreach (BaseAI baseAI in BaseAI.GetAllInstances())
				{
					if (baseAI.gameObject.name.StartsWith(name) && DistanceXZ(baseAI.transform.position, centerPoint) < minDistance)
					{
						return true;
					}
				}
				return false;
			}
			foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("spawned"))
			{
				if (gameObject.gameObject.name.StartsWith(name) && DistanceXZ(gameObject.transform.position, centerPoint) < minDistance)
				{
					return true;
				}
			}
			return false;
		}

		public static float DistanceXZ(Vector3 v0, Vector3 v1)
		{
			float num = v1.x - v0.x;
			float num2 = v1.z - v0.z;
			return Mathf.Sqrt(num * num + num2 * num2);
		}

		private static void Spawn(float levelUpChance, SpawnSystem.SpawnData critter, Vector3 spawnPoint, bool eventSpawner)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(critter.m_prefab, spawnPoint, Quaternion.identity);
			BaseAI component = gameObject.GetComponent<BaseAI>();
			if (component != null)
			{
				if (critter.m_huntPlayer)
				{
					component.SetHuntPlayer(true);
				}
				if (critter.m_maxLevel > 1 && (critter.m_levelUpMinCenterDistance <= 0f || spawnPoint.magnitude > critter.m_levelUpMinCenterDistance))
				{
					Character component2 = gameObject.GetComponent<Character>();
					if (component2)
					{
						int num = critter.m_minLevel;
						while (num < critter.m_maxLevel && UnityEngine.Random.Range(0f, 100f) <= levelUpChance)
						{
							num++;
						}
						if (num > 1)
						{
							component2.SetLevel(num);
						}
					}
				}
				MonsterAI monsterAI = component as MonsterAI;
				if (monsterAI)
				{
					if (!critter.m_spawnAtDay)
					{
						monsterAI.SetDespawnInDay(true);
					}
					if (eventSpawner)
					{
						monsterAI.SetEventCreature(true);
					}
				}
			}
			else
			{
				Log.LogTrace($"Did not find BaseAI component of {critter.m_prefab.name}");
			}
		}

		private static bool FindBaseSpawnPoint(SpawnSystem.SpawnData spawn, List<Player> allPlayers, out Vector3 spawnCenter, out Player targetPlayer)
		{
			float min = (spawn.m_spawnRadiusMin > 0f) ? spawn.m_spawnRadiusMin : 40f;
			float max = (spawn.m_spawnRadiusMax > 0f) ? spawn.m_spawnRadiusMax : 80f;
			for (int i = 0; i < 20; i++)
			{
				Player player = allPlayers[UnityEngine.Random.Range(0, allPlayers.Count)];
				Vector3 a = Quaternion.Euler(0f, (float)UnityEngine.Random.Range(0, 360), 0f) * Vector3.forward;
				Vector3 vector = player.transform.position + a * UnityEngine.Random.Range(min, max);
				if (IsSpawnPointGood(spawn, ref vector))
				{
					spawnCenter = vector;
					targetPlayer = player;
					return true;
				}
			}
			spawnCenter = Vector3.zero;
			targetPlayer = null;
			return false;
		}

		private static bool IsSpawnPointGood(SpawnSystem.SpawnData spawn, ref Vector3 spawnPoint)
		{
			Vector3 vector;
			Heightmap.Biome biome;
			Heightmap.BiomeArea biomeArea;
			Heightmap heightmap;
			ZoneSystem.instance.GetGroundData(ref spawnPoint, out vector, out biome, out biomeArea, out heightmap);
			if ((spawn.m_biome & biome) == Heightmap.Biome.None)
			{
				Log.LogTrace($"Wrong biome. Was {spawn.m_biome}:{biome} with value {spawn.m_biome & biome}");
				return false;
			}
			if ((spawn.m_biomeArea & biomeArea) == (Heightmap.BiomeArea)0)
			{
				Log.LogTrace("Wrong biome area");
				return false;
			}
			if (ZoneSystem.instance.IsBlocked(spawnPoint))
			{
				Log.LogTrace("Spawnpoint blocked");
				return false;
			}
			float num = spawnPoint.y - ZoneSystem.instance.m_waterLevel;
			if (num < spawn.m_minAltitude || num > spawn.m_maxAltitude)
			{
				Log.LogTrace($"Altitude wrong {num}");
				return false;
			}
			float num2 = Mathf.Cos(0.017453292f * spawn.m_maxTilt);
			float num3 = Mathf.Cos(0.017453292f * spawn.m_minTilt);
			if (vector.y < num2 || vector.y > num3)
			{
				Log.LogTrace("Tilt wrong");
				return false;
			}
			float range = (spawn.m_spawnRadiusMin > 0f) ? spawn.m_spawnRadiusMin : 40f;
			if (Player.IsPlayerInRange(spawnPoint, range))
			{
				Log.LogTrace("Player not in range");
				return false;
			}
			if (EffectArea.IsPointInsideArea(spawnPoint, EffectArea.Type.PlayerBase, 0f))
			{
				Log.LogTrace("Point inside base");
				return false;
			}
			if (!spawn.m_inForest || !spawn.m_outsideForest)
			{
				bool flag = WorldGenerator.InForest(spawnPoint);
				if (!spawn.m_inForest && flag)
				{
					Log.LogTrace("In forest, and shouldn't be");
					return false;
				}
				if (!spawn.m_outsideForest && !flag)
				{
					Log.LogTrace("Outside forest, and shouldn't be");
					return false;
				}
			}
			if (spawn.m_minOceanDepth != spawn.m_maxOceanDepth && heightmap != null)
			{
				float oceanDepth = heightmap.GetOceanDepth(spawnPoint);
				if (oceanDepth < spawn.m_minOceanDepth || oceanDepth > spawn.m_maxOceanDepth)
				{
					Log.LogTrace("Ocean depth wrong");
					return false;
				}
			}
			return true;
		}

		public static int GetStableHashCode(this string str)
		{
			int num = 5381;
			int num2 = num;
			int num3 = 0;
			while (num3 < str.Length && str[num3] != '\0')
			{
				num = ((num << 5) + num ^ (int)str[num3]);
				if (num3 == str.Length - 1 || str[num3 + 1] == '\0')
				{
					break;
				}
				num2 = ((num2 << 5) + num2 ^ (int)str[num3 + 1]);
				num3 += 2;
			}
			return num + num2 * 1566083941;
		}
	}
}

#endif