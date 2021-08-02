using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using StressLevelZero.Zones;

namespace Zombones
{
    internal static class Utils
    {
        internal struct ManagerData
        {
            internal float WaveTime;
            internal float SpawnFrequencyMultiplier;
            internal float CooldownTime;
            internal float SmallAmmoReward;
            internal float MediumAmmoReward;
        }

        internal static GameObject FindZombieManagerGameObject()
        {
            foreach (GameObject gameObject in GameObject.FindObjectsOfType<GameObject>())
            {
                if (gameObject.name.Contains("ZombieManager"))
                {
                    return gameObject;
                }
            }
            return null;
        }

        internal static ManagerData GetZombieManagerData()
        {
            ManagerData Data = new ManagerData()
            {
                WaveTime = 120f,
                CooldownTime = 30f,
                SpawnFrequencyMultiplier = 1.5f,
                SmallAmmoReward = 0f,
                MediumAmmoReward = 0f
            };

            Transform Manager = FindZombieManagerGameObject().transform;
            if (Manager.Find("WaveTime") != null)
            {
                Data.WaveTime = float.Parse(Manager.Find("WaveTime").GetChild(0).name);
            }

            if (Manager.Find("SpawnFrequencyMultiplier") != null)
            {
                Data.SpawnFrequencyMultiplier = float.Parse(Manager.Find("SpawnFrequencyMultiplier").GetChild(0).name);
            }

            if(Manager.Find("CooldownTime") != null)
            {
                Data.CooldownTime = float.Parse(Manager.Find("CooldownTime").GetChild(0).name);
            }

            if(Manager.Find("SmallAmmoReward") != null)
            {
                Data.SmallAmmoReward = float.Parse(Manager.Find("SmallAmmoReward").GetChild(0).name);
            }

            if(Manager.Find("MediumAmmoReward") != null)
            {
                Data.MediumAmmoReward = float.Parse(Manager.Find("MediumAmmoReward").GetChild(0).name);
            }

            return Data;
        }

        internal static List<ZoneSpawner> FindZombieSpawners()
        {
            return GameObject.FindObjectsOfType<ZoneSpawner>().ToList();
        }

        internal static List<GameObject> GetZombieCooldownObjects()
        {
            if (FindZombieManagerGameObject().transform.Find("CooldownEnableObjects") != null)
            {
                return FindZombieManagerGameObject().transform.Find("CooldownEnableObjects").GetComponentsInChildren<GameObject>().ToList();
            }
            return null;
        }
    }
}
