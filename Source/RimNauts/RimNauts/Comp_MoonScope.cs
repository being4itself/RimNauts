using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace ThatsAMoon
{
    // Token: 0x0200000A RID: 10
    public class Comp_MoonScope : ThingComp
    {
        // Token: 0x1700000A RID: 10
        // (get) Token: 0x06000027 RID: 39 RVA: 0x000033B4 File Offset: 0x000015B4
        public CompProperties_MoonScope Props
        {
            get
            {
                return this.props as CompProperties_MoonScope;
            }
        }

        // Token: 0x06000028 RID: 40 RVA: 0x000033D1 File Offset: 0x000015D1
        public override void CompTick()
        {
            base.CompTick();
        }

        // Token: 0x06000029 RID: 41 RVA: 0x000033DB File Offset: 0x000015DB
        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
            this.numberOfMoons = Current.Game.GetComponent<Gamecomp_SatellitesInOrbit>().numberOfSatellites;
        }

        // Token: 0x0600002A RID: 42 RVA: 0x000033FC File Offset: 0x000015FC
        public void lookAtMoon()
        {
            bool flag = Current.Game.GetComponent<Gamecomp_SatellitesInOrbit>().numberOfSatellites == 0;
            if (flag)
            {
                Current.Game.GetComponent<Gamecomp_SatellitesInOrbit>().tryGenSatellite();
                this.numberOfMoons++;
                Map map = Current.Game.GetComponent<Gamecomp_SatellitesInOrbit>().makeMoonMap();
                this.numberOfMaps++;
                CameraJumper.TryJump(map.Center, map);
                Find.MapUI.Notify_SwitchedMap();
            }
            Current.Game.GetComponent<Gamecomp_SatellitesInOrbit>().updateSatellites();
            Find.LetterStack.ReceiveLetter("To the moon!", "Your settlement have found a good place to land on the moon. Build a spaceship and explore!", LetterDefOf.NeutralEvent, null);
        }

        // Token: 0x0600002B RID: 43 RVA: 0x000034A4 File Offset: 0x000016A4
        public void lookAtMoonX()
        {
            bool flag = this.numberOfMaps < this.numberOfMoons;
            if (flag)
            {
                bool devMode = Prefs.DevMode;
                if (devMode)
                {
                    Log.Message("making map", false);
                }
                Map map = Current.Game.GetComponent<Gamecomp_SatellitesInOrbit>().makeMoonMap();
                this.numberOfMaps++;
                CameraJumper.TryJump(map.Center, map);
                Find.MapUI.Notify_SwitchedMap();
                this.triggerFlag = true;
            }
            else
            {
                bool flag2 = this.numberOfMaps == this.numberOfMoons;
                if (flag2)
                {
                    bool devMode2 = Prefs.DevMode;
                    if (devMode2)
                    {
                        Log.Message("making moon", false);
                    }
                    Current.Game.GetComponent<Gamecomp_SatellitesInOrbit>().tryGenSatellite();
                    this.numberOfMoons++;
                }
            }
            Current.Game.GetComponent<Gamecomp_SatellitesInOrbit>().updateSatellites();
            Find.LetterStack.ReceiveLetter("To the moon!", "Your settlement have found a good place to land on the moon. Build a spaceship and explore!", LetterDefOf.NeutralEvent, null);
        }

        // Token: 0x0600002C RID: 44 RVA: 0x00003590 File Offset: 0x00001790
        public float randomOrbit(float min, float range)
        {
            float num = min * (float)(Rand.Bool ? 1 : -1);
            return min + range * (Rand.Value - 0.5f);
        }

        // Token: 0x0600002D RID: 45 RVA: 0x000035C2 File Offset: 0x000017C2
        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            bool flag = Current.Game.GetComponent<Gamecomp_SatellitesInOrbit>().numberOfSatellites == 0;
            if (flag)
            {
                yield return new Command_Action
                {
                    defaultLabel = "Small Moon",
                    icon = ContentFinder<Texture2D>.Get("UI/teleIcon", true),
                    defaultDesc = "You see a tiny barren moon among the other space debris, Check for a good place to land!",
                    action = new Action(this.lookAtMoon)
                };
            }
            yield break;
        }

        // Token: 0x04000010 RID: 16
        public bool triggerFlag = false;

        // Token: 0x04000011 RID: 17
        public int numberOfMoons = 1;

        // Token: 0x04000012 RID: 18
        public int numberOfMaps = 0;
    }
}
