using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using MelonLoader;
using UnityEngine;
using ModThatIsNotMod.BoneMenu;

using StressLevelZero.Zones;

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
        private int CurrentRound = 0;

        public float SpawnFrequencyMultiplier = 1.1f;

        public static bool MapIsZombieMap = false;
        public static bool GameStarted = false;

        public override void OnApplicationStart()
        {
            CustomMaps.CustomMaps.OnCustomMapLoad += CustomMaps_OnCustomMapLoad;

            MenuCategory ZombonesCategory = MenuManager.CreateCategory(BuildInfo.Name, Color.green);
            ZombonesCategory.CreateFunctionElement("Start Game", Color.white, () => {
                if (!MapIsZombieMap) return;
            });
        }

        public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
        {
            MapIsZombieMap = false;
            GameStarted = false;
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

                UIWatch.MakeUI();
            }
        }

        public override void OnUpdate()
        {
            if (MapIsZombieMap && GameStarted)
            {
                WaveTimer += Time.deltaTime;

                UIWatch.SetText($"Wave:{CurrentRound}\n{Math.Round(WaveTime-WaveTimer, 1)}");

                if (WaveTimer > WaveTime)
                {
                    WaveTimer = 0f;
                    CurrentRound++;
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
                foreach(ZoneSpawner spawner in FindZombieSpawners())
                {
                    spawner.AllowSpawning(true);
                }
                UIWatch.Haptic(10);
                GameStarted = true;
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
