using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using MelonLoader;
using UnityEngine;
using ModThatIsNotMod;

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
        public float RoundTime;
        public float SpawnTime;

        private float RoundTimer = 0f;
        private float SpawnTimer = 0f;
        private int CurrentRound = 0;

        public bool MapIsZombieMap = false;

        public override void OnApplicationStart()
        {
            CustomMaps.CustomMaps.OnCustomMapLoad += CustomMaps_OnCustomMapLoad;
        }

        private void CustomMaps_OnCustomMapLoad(string name)
        {
            MapIsZombieMap = FindZombieManager() != null;
            MelonLogger.Msg(MapIsZombieMap ? "Map is a zombie map" : "Map is not a zombie map");

            if(MapIsZombieMap)
            {
                Transform Manager = FindZombieManager().transform;
                if (Manager.Find("RoundTime") != null)
                {
                    RoundTime = float.Parse(Manager.Find("RoundTime").GetChild(0).name);
                }

                if (Manager.Find("SpawnTime") != null)
                {
                    SpawnTime = float.Parse(Manager.Find("SpawnTime").GetChild(0).name);
                }

                UIWatch.MakeUI();
            }
        }

        public override void OnUpdate()
        {
            if (MapIsZombieMap)
            {
                RoundTimer += Time.deltaTime;
                SpawnTimer += Time.deltaTime;

                UIWatch.SetText($"Wave:{CurrentRound}\n{Math.Round(RoundTime -RoundTimer, 1)}\n{Math.Round(SpawnTime-SpawnTimer, 1)}");

                if (RoundTimer > RoundTime)
                {
                    RoundTimer = 0f;
                    CurrentRound++;
                    UIWatch.Haptic(1);
                    SpawnTime *= 0.9f;
                }

                if (SpawnTimer > SpawnTime)
                {
                    SpawnTimer = 0f;
                    MelonLogger.Msg($"Spawning from {FindZombieSpawners().Count} Spawners");
                    foreach (GameObject gameObject in FindZombieSpawners())
                    {
                        CustomItems.SpawnFromPool(gameObject.transform.GetChild(0).name, gameObject.transform.position, gameObject.transform.rotation);
                    }
                }
            }
        }

        private GameObject FindZombieManager()
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

        private List<GameObject> FindZombieSpawners()
        {
            return GameObject.FindObjectsOfType<GameObject>().Where(obj => obj.name.Contains("ZombieSpawner")).ToList();
        }
    }
}
