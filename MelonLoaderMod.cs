using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using MelonLoader;
using UnityEngine;
using ModThatIsNotMod;
using ModThatIsNotMod.BoneMenu;

using StressLevelZero.Zones;
using StressLevelZero.AI;

namespace Zombones
{
    public static class BuildInfo
    {
        public const string Name = "Zombones";
        public const string Author = "L4rs";
        public const string Company = null;
        public const string Version = "0.0.2";
        public const string DownloadLink = null;
    }

    public class Zombones : MelonMod
    {
        private float WaveTime;
        private float WaveTimer = 0f;
        private int CurrentWave = 0;

        public float SpawnFrequencyMultiplier = 1.1f;

        internal static bool MapIsZombieMap = false;
        internal static bool GameStarted = false;

        internal static MenuCategory ZombonesCategory;

        public override void OnApplicationStart()
        {
            CustomMaps.CustomMaps.OnCustomMapLoad += CustomMaps_OnCustomMapLoad;

            ZombonesCategory = MenuManager.CreateCategory(BuildInfo.Name, Color.green);
            ZombonesCategory.CreateFunctionElement("Start Game", Color.white, () => {
                TryStartGame();
            });

        }

        private void CustomMaps_OnCustomMapLoad(string name)
        {
            MapIsZombieMap = FindZombieManager() != null;
            MelonLogger.Msg(MapIsZombieMap ? "Map is a zombie map" : "Map is not a zombie map");

            if(MapIsZombieMap)
            {
                Transform Manager = FindZombieManager().transform;
                if (Manager.Find("WaveTime") != null)
                {
                    WaveTime = float.Parse(Manager.Find("WaveTime").GetChild(0).name);
                }

                if (Manager.Find("SpawnFrequencyMultiplier") != null)
                {
                    SpawnFrequencyMultiplier = float.Parse(Manager.Find("SpawnFrequencyMultiplier").GetChild(0).name);
                }

                foreach (ZoneSpawner spawner in FindZombieSpawners())
                {
                    spawner.OnSpawnDelegate = (Action<GameObject, GameObject>)((GameObject a, GameObject b) =>
                    {
                        if(a.gameObject.GetComponent<AIBrain>() != null)
                        {
                            a.gameObject.GetComponent<AIBrain>().behaviour.breakAgroTargetDistance = float.PositiveInfinity;
                            a.gameObject.GetComponent<AIBrain>().behaviour.SetAgro(Player.rightHand.triggerRefProxy);
                        }

                        if (b.gameObject.GetComponent<AIBrain>() != null)
                        {
                            b.gameObject.GetComponent<AIBrain>().behaviour.breakAgroTargetDistance = float.PositiveInfinity;
                            b.gameObject.GetComponent<AIBrain>().behaviour.SetAgro(Player.rightHand.triggerRefProxy);
                        }
                    });
                }

                UIWatch.MakeUI();
            }
        }

        public override void OnUpdate()
        {
            if (MapIsZombieMap && GameStarted)
            {
                WaveTimer += Time.deltaTime;

                TimeSpan time = TimeSpan.FromSeconds(WaveTime - WaveTimer);
                UIWatch.SetText($"Wave:{CurrentWave}\n{time.Minutes}:{time.Seconds}");

                if (WaveTimer > WaveTime)
                {
                    WaveTimer = 0f;
                    CurrentWave++;
                    UIWatch.Haptic(10);
                    foreach(ZoneSpawner spawner in FindZombieSpawners())
                    {
                        spawner.frequency *= SpawnFrequencyMultiplier;
                    }
                }
            }
        }

        internal static bool TryStartGame()
        {
            if (MapIsZombieMap) {
                GameStarted = true;
                UIWatch.Haptic(10);
                foreach (ZoneSpawner spawner in FindZombieSpawners())
                {
                    spawner.StartSpawn();
                }
                return true;
            }
            return false;
        }

        internal static GameObject FindZombieManager()
        {
            foreach(GameObject gameObject in GameObject.FindObjectsOfType<GameObject>())
            {
                if(gameObject.name.Contains("ZombieManager"))
                {
                    return gameObject;
                }
            }
            return null;
        }

        internal static List<ZoneSpawner> FindZombieSpawners()
        {
            return GameObject.FindObjectsOfType<ZoneSpawner>().ToList();
        }
    }
}
