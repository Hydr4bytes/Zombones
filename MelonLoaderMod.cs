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
using StressLevelZero.Props.Weapons;
using StressLevelZero.Player;

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
        private float WaveTimer = 0f;
        private int CurrentWave = 0;

        internal static Utils.ManagerData ManagerData;

        internal static bool MapIsZombieMap = false;
        internal static bool GameStarted = false;
        internal static Gamemode CurrentGamemode;

        internal static List<Gamemode> Gamemodes = new List<Gamemode>(); 

        public override void OnApplicationStart()
        {
            CustomMaps.CustomMaps.OnCustomMapLoad += CustomMaps_OnCustomMapLoad;

            Gamemodes.Add(new Endless());

            MenuCategory ZombonesCategory = MenuManager.CreateCategory(BuildInfo.Name, Color.green);
            MenuCategory SubCategory = ZombonesCategory.CreateSubCategory("Start Game", Color.white);
            foreach (Gamemode gamemode in Gamemodes)
            {
                SubCategory.CreateFunctionElement(gamemode.Name, Color.white, () => {
                    TryStartGame(gamemode);
                    MelonLogger.Msg("Starting " + gamemode.Name);
                });
            }
        }

        private void CustomMaps_OnCustomMapLoad(string name)
        {
            MapIsZombieMap = Utils.FindZombieManagerGameObject() != null;
            MelonLogger.Msg(MapIsZombieMap ? "Map is a zombie map" : "Map is not a zombie map");

            if(MapIsZombieMap)
            {
                ManagerData = Utils.GetZombieManagerData();

                foreach (ZoneSpawner spawner in Utils.FindZombieSpawners())
                {
                    spawner.OnSpawnDelegate = (Action<GameObject, GameObject>)((GameObject a, GameObject b) =>
                    {
                        if(a.gameObject.GetComponent<AIBrain>() != null)
                        {
                            CurrentGamemode.OnSpawn(a.gameObject.GetComponent<AIBrain>());
                        }
                        else if (b.gameObject.GetComponent<AIBrain>() != null)
                        {
                            CurrentGamemode.OnSpawn(b.gameObject.GetComponent<AIBrain>());
                        }
                    });
                }

                UIWatch.MakeUI();
            }
        }

        internal IEnumerator MainLoop()
        {
            while(MapIsZombieMap && GameStarted)
            {
                WaveTimer += Time.deltaTime;

                TimeSpan time = TimeSpan.FromSeconds(ManagerData.WaveTime - WaveTimer);
                UIWatch.SetText($"Wave:{CurrentWave}\n{time.ToString(@"m\:ss")}");

                if (WaveTimer > ManagerData.WaveTime && CurrentGamemode.CanAdvance())
                {
                    WaveTimer = 0f;
                    CurrentWave++;
                    CurrentGamemode.OnCooldown();
                    UIWatch.SetText($"Cooldown");
                    GiveAmmo(CurrentGamemode.SmallAmmoReward(), CurrentGamemode.MediumAmmoReward());
                    yield return new WaitForSeconds(ManagerData.CooldownTime);
                    CurrentGamemode.OnNewWave();
                }

                yield return null;
            }
        }

        internal bool TryStartGame(Gamemode gamemode)
        {
            if (MapIsZombieMap) {
                GameStarted = true;
                UIWatch.Haptic(10);
                foreach (ZoneSpawner spawner in Utils.FindZombieSpawners())
                {
                    spawner.StartSpawn();
                }
                CurrentGamemode = gamemode;
                MelonCoroutines.Start(MainLoop());
                return true;
            }
            return false;
        }

        internal static void GiveAmmo(float small, float medium)
        {
            PlayerInventory inventory = GameObject.FindObjectOfType<PlayerInventory>();
            inventory.AddAmmo(StressLevelZero.Combat.Weight.LIGHT, (int)small);
            inventory.AddAmmo(StressLevelZero.Combat.Weight.MEDIUM, (int)medium);
        }
    }
}
