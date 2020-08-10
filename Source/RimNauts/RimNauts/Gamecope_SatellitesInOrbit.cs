using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace ThatsAMoon
{
    // Token: 0x0200000E RID: 14
    internal class Gamecomp_SatellitesInOrbit : GameComponent
    {
        // Token: 0x06000032 RID: 50 RVA: 0x00003760 File Offset: 0x00001960
        public Gamecomp_SatellitesInOrbit(Game game)
        {
        }

        // Token: 0x06000033 RID: 51 RVA: 0x00003828 File Offset: 0x00001A28
        public Tile getTile(int tileNum)
        {
            return Enumerable.ElementAt<Tile>(Find.World.grid.tiles, tileNum);
        }

        // Token: 0x06000034 RID: 52 RVA: 0x00003850 File Offset: 0x00001A50
        public bool applySatelliteSurface(int tileNum)
        {
            bool result;
            try
            {
                Enumerable.ElementAt<Tile>(Find.World.grid.tiles, tileNum).elevation = 5f;
                Enumerable.ElementAt<Tile>(Find.World.grid.tiles, tileNum).hilliness = Hilliness.Flat;
                Enumerable.ElementAt<Tile>(Find.World.grid.tiles, tileNum).rainfall = 0f;
                Enumerable.ElementAt<Tile>(Find.World.grid.tiles, tileNum).swampiness = 0f;
                Enumerable.ElementAt<Tile>(Find.World.grid.tiles, tileNum).temperature = -40f;
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        // Token: 0x06000035 RID: 53 RVA: 0x00003914 File Offset: 0x00001B14
        public bool tryGenSatellite()
        {
            int num = Find.World.grid.TilesCount - this.numberOfSatellites - 1;
            Vector3 getOrbitVectorBase = this.satDef.getOrbitVectorBase;
            Vector3 getOrbitVectorRange = this.satDef.getOrbitVectorRange;
            bool result;
            try
            {
                WorldObjectChild_Satellite worldObjectChild_Satellite = (WorldObjectChild_Satellite)WorldObjectMaker.MakeWorldObject(DefDatabase<WorldObjectDef>.GetNamed(this.satDef.WorldObjectDefNames.RandomElement<string>(), true));
                worldObjectChild_Satellite.Tile = num;
                worldObjectChild_Satellite.period = (float)((int)this.randomOrbit(this.satDef.getOrbitPeriod, this.satDef.getOrbitPeriodVar));
                Find.WorldObjects.Add(worldObjectChild_Satellite);
                this.numberOfSatellites++;
                this.satelliteTiles.Add(num);
                this.satellites.Add(worldObjectChild_Satellite);
                this.applySatelliteSurface(num);
                this.satelliteTilesReal.Add(this.getTile(num));
                result = true;
            }
            catch
            {
                Log.Error("Failed to add satellite", false);
                result = false;
            }
            return result;
        }

        // Token: 0x06000036 RID: 54 RVA: 0x00003A1C File Offset: 0x00001C1C
        public void updateSatellites()
        {
            foreach (MapParent mapParent in this.satellites)
            {
                mapParent.SetFaction(Faction.OfPlayer);
            }
        }

        // Token: 0x06000037 RID: 55 RVA: 0x00003A7C File Offset: 0x00001C7C
        public bool tryGenSatellite(int tile, Vector3 Periapsis, Vector3 Apoapsis, int period)
        {
            bool result;
            try
            {
                WorldObjectChild_Satellite worldObjectChild_Satellite = (WorldObjectChild_Satellite)WorldObjectMaker.MakeWorldObject(DefDatabase<WorldObjectDef>.GetNamed(this.defs.RandomElement<string>(), true));
                worldObjectChild_Satellite.Tile = tile;
                worldObjectChild_Satellite.period = (float)period;
                Find.WorldObjects.Add(worldObjectChild_Satellite);
                result = true;
            }
            catch
            {
                Log.Error("Failed to add satellite", false);
                result = false;
            }
            return result;
        }

        // Token: 0x06000038 RID: 56 RVA: 0x00003AEC File Offset: 0x00001CEC
        public float randomOrbit(float min, float range)
        {
            float num = min * (float)(Rand.Bool ? 1 : -1);
            return min + range * (Rand.Value - 0.5f);
        }

        // Token: 0x06000039 RID: 57 RVA: 0x00003B20 File Offset: 0x00001D20
        public Map makeMoonMap()
        {
            Log.Message("Look at that moon!", false);
            Map map = MapGenerator.GenerateMap(new IntVec3(300, 1, 300), Enumerable.Last<WorldObjectChild_Satellite>(this.satellites), Enumerable.Last<WorldObjectChild_Satellite>(this.satellites).MapGeneratorDef, Enumerable.Last<WorldObjectChild_Satellite>(this.satellites).ExtraGenStepDefs, null);
            try
            {
                bool flag = false;
                List<WeatherDef> list = Enumerable.ToList<WeatherDef>(DefDatabase<WeatherDef>.AllDefs);
                foreach (WeatherDef weatherDef in list)
                {
                    bool flag2 = weatherDef.defName.Equals("OuterSpaceWeather");
                    if (flag2)
                    {
                        flag = true;
                    }
                }
                bool flag3 = flag;
                if (flag3)
                {
                    Log.Message("set weather", false);
                    map.weatherManager.curWeather = WeatherDef.Named("OuterSpaceWeather");
                }
                else
                {
                    Log.Message("no weather", false);
                }
            }
            catch
            {
                bool devMode = Prefs.DevMode;
                if (devMode)
                {
                    Log.Message("No space weather catch", false);
                }
            }
            Find.World.WorldUpdate();
            return map;
        }

        // Token: 0x0600003A RID: 58 RVA: 0x00003C5C File Offset: 0x00001E5C
        public void resetSatellite()
        {
            this.satelliteTilesReal = new List<Tile>();
            this.satelliteTiles = new List<int>();
            this.numberOfSatellites = 0;
            this.satellites = new List<WorldObjectChild_Satellite>();
        }

        // Token: 0x0600003B RID: 59 RVA: 0x00003C88 File Offset: 0x00001E88
        public bool tryGenSatelliteMap(int tile)
        {
            return false;
        }

        // Token: 0x04000013 RID: 19
        public int numberOfSatellites = 0;

        // Token: 0x04000014 RID: 20
        public List<int> satelliteTiles = new List<int>();

        // Token: 0x04000015 RID: 21
        public List<Tile> satelliteTilesReal = new List<Tile>();

        // Token: 0x04000016 RID: 22
        private SatelliteDef satDef = DefDatabase<SatelliteDef>.GetNamed("SatelliteCore", true);

        // Token: 0x04000017 RID: 23
        public List<WorldObjectChild_Satellite> satellites = new List<WorldObjectChild_Satellite>();

        // Token: 0x04000018 RID: 24
        private List<string> defs = new List<string>
        {
            "Junk1",
            "Junk2",
            "Junk3",
            "Junk4",
            "Junk5",
            "Junk6",
            "Junk7",
            "Junk8",
            "Junk9"
        };
    }
}
