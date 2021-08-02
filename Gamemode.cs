using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ModThatIsNotMod;
using StressLevelZero.AI;
using StressLevelZero.Zones;
using StressLevelZero.Pool;
using static Zombones.Utils;

using UnityEngine;

namespace Zombones
{
    abstract class Gamemode
    {
        public string Name = "";

        public abstract void OnSpawn(AIBrain brain);

        public abstract void OnCooldown();
        public abstract void OnNewWave();
        public abstract bool CanAdvance();
        public abstract float SmallAmmoReward();
        public abstract float MediumAmmoReward();
    }

    internal class Endless : Gamemode
    {
        public Endless()
        {
            Name = "Endless";
        }

        public override void OnSpawn(AIBrain brain)
        {
            brain.behaviour.breakAgroHomeDistance = float.PositiveInfinity;
            brain.behaviour.breakAgroTargetDistance = float.PositiveInfinity;
            brain.behaviour.SetAgro(Player.rightHand.triggerRefProxy);
        }

        public override void OnNewWave()
        {
            UIWatch.Haptic(10);
            foreach (ZoneSpawner spawner in Utils.FindZombieSpawners())
            {
                spawner.frequency *= Zombones.ManagerData.SpawnFrequencyMultiplier;
            }

            foreach (GameObject gameObject in GetZombieCooldownObjects())
            {
                gameObject.SetActive(false);
            }
        }

        public override void OnCooldown()
        {
            UIWatch.Haptic(10);

            AIBrain brain = GameObject.FindObjectOfType<AIBrain>();
            if (brain.poolee != null)
            {
                brain.poolee.pool.DespawnAll();
            }
            else
            {
                MelonLoader.MelonLogger.Msg("Poolee not found");
            }

            foreach(GameObject gameObject in GetZombieCooldownObjects())
            {
                gameObject.SetActive(true);
            }
        }

        public override bool CanAdvance()
        {
            return true;
        }

        public override float SmallAmmoReward()
        {
            return Zombones.ManagerData.SmallAmmoReward;
        }

        public override float MediumAmmoReward()
        {
            return Zombones.ManagerData.MediumAmmoReward;
        }
    }
}
