using System;
using System.Linq;
using RimWorld;
using Verse;

namespace ThatsAMoon
{
    // Token: 0x02000005 RID: 5
    public class GenStep_Crater : GenStep
    {
        // Token: 0x17000002 RID: 2
        // (get) Token: 0x06000011 RID: 17 RVA: 0x000026EC File Offset: 0x000008EC
        public override int SeedPart
        {
            get
            {
                return 1148858716;
            }
        }

        // Token: 0x06000012 RID: 18 RVA: 0x00002704 File Offset: 0x00000904
        public void wideBrush(IntVec3 pos, string defName, Map map, int halfWidth)
        {
            for (int i = -halfWidth; i <= halfWidth; i++)
            {
                map.terrainGrid.SetTerrain(new IntVec3(pos.x, 0, pos.z + i), DefDatabase<TerrainDef>.GetNamed("lunarSand", true));
            }
        }

        // Token: 0x06000013 RID: 19 RVA: 0x00002758 File Offset: 0x00000958
        public override void Generate(Map map, GenStepParams parms)
        {
            bool devMode = Prefs.DevMode;
            if (devMode)
            {
                //Log.Message("start crater gen", false);
            }
            int num = map.Size.x / 2 - 3;
            for (int i = 1; i <= Rand.RangeInclusive(4, 10); i++)
            {
                int num2 = (int)(((double)Rand.Value - 0.5) * (double)map.Size.z);
                for (int j = 1; j <= map.Size.x - 1; j++)
                {
                    int newZ = (int)(-22.0 * Math.Sin((double)(3.14f / (float)map.Size.x * (float)j - 0f)) + (double)map.Center.z + (double)num2 * 0.75);
                    //Log.Message("y= " + newZ.ToString(), false);
                    this.wideBrush(new IntVec3(j, 0, newZ), "lunarSand", map, Rand.RangeInclusive(1, 4));
                }
            }
            for (int k = 1; k <= map.Size.x - 1; k++)
            {
                int newZ2 = (int)(-6.0 * Math.Sin((double)(3.14f / (float)map.Size.x * (float)k - 0f)) + (double)map.Center.z + (double)(map.Size.z / 2 - 5) * 0.75);
                //Log.Message("y= " + newZ2.ToString(), false);
                this.wideBrush(new IntVec3(k, 0, newZ2), "astroidIceHi", map, Rand.RangeInclusive(1, 4));
            }
            for (int l = 1; l <= Rand.RangeInclusive(1, 1); l++)
            {
                IntVec3 position = new IntVec3(map.Center.x + (int)(((double)Rand.Value - 0.5) * (double)map.Size.x * 0.75), map.Center.y + (int)(((double)Rand.Value - 0.5) * (double)map.Size.y * 0.75), map.Center.z + (int)(((double)Rand.Value - 0.5) * (double)map.Size.z * 0.75));
                /*Log.Message(string.Concat(new string[]
                {
                    position.x.ToString(),
                    ",",
                    position.y.ToString(),
                    ",",
                    position.z.ToString()
                }), false);*/
                this.genCrater(position, new IntVec3(Rand.RangeInclusive(20, 30), 0, Rand.RangeInclusive(20, 30)), map);
            }
            for (int m = 1; m <= Rand.RangeInclusive(1, 1); m++)
            {
                IntVec3 position2 = new IntVec3(map.Center.x + (int)(((double)Rand.Value - 0.5) * (double)map.Size.x * 0.75), map.Center.y + (int)(((double)Rand.Value - 0.5) * (double)map.Size.y * 0.75), map.Center.z + (int)(((double)Rand.Value - 0.5) * (double)map.Size.z * 0.75));
                this.genCrater(position2, new IntVec3(Rand.RangeInclusive(10, 25), 0, Rand.RangeInclusive(10, 25)), map);
            }
            for (int n = 1; n <= Rand.RangeInclusive(1, 1); n++)
            {
                IntVec3 position3 = new IntVec3(map.Center.x + (int)(((double)Rand.Value - 0.5) * (double)map.Size.x * 0.75), map.Center.y + (int)(((double)Rand.Value - 0.5) * (double)map.Size.y * 0.75), map.Center.z + (int)(((double)Rand.Value - 0.5) * (double)map.Size.z * 0.75));
                this.genCrater(position3, new IntVec3(Rand.RangeInclusive(5, 15), 0, Rand.RangeInclusive(5, 15)), map);
            }
            foreach (IntVec3 intVec in map.AllCells)
            {
                bool flag = (intVec.x - map.Center.x) * (intVec.x - map.Center.x) + (intVec.z - map.Center.z) * (intVec.z - map.Center.z) >= num * num;
                if (flag)
                {
                    bool devMode2 = Prefs.DevMode;
                    if (devMode2)
                    {
                        //Log.Message("start carve", false);
                    }
                    Thing thing = GenSpawn.Spawn(ThingDefOf.Sandstone, intVec, map, WipeMode.Vanish);
                    thing.Destroy(DestroyMode.Vanish);
                    bool flag2 = Enumerable.Contains<TerrainDef>(DefDatabase<TerrainDef>.AllDefs, TerrainDef.Named("OpenSpace"));
                    if (flag2)
                    {
                        map.terrainGrid.SetTerrain(intVec, DefDatabase<TerrainDef>.GetNamed("OpenSpace", true));
                    }
                    else
                    {
                        //Log.Error("No Space found", false);
                    }
                }
                bool flag3 = map.fertilityGrid.FertilityAt(intVec) > 0f;
                if (flag3)
                {
                    map.terrainGrid.SetTerrain(intVec, DefDatabase<TerrainDef>.GetNamed("lunarSand", true));
                }
            }
            MapGenerator.PlayerStartSpot = new IntVec3(1, 0, 1);
        }

        // Token: 0x06000014 RID: 20 RVA: 0x00002DB4 File Offset: 0x00000FB4
        public void thingSwap(Map map, IntVec3 location, string targetDef, string swapDef)
        {
            try
            {
                bool flag = map.thingGrid.ThingAt<Thing>(location).def.defName.Equals(targetDef);
                if (flag)
                {
                    GenSpawn.Spawn(DefDatabase<ThingDef>.GetNamed(swapDef, true), location, map, WipeMode.Vanish);
                }
            }
            catch
            {
            }
        }

        // Token: 0x06000015 RID: 21 RVA: 0x00002E10 File Offset: 0x00001010
        public void terrainSwap(Map map, IntVec3 location, string targetDef, string swapDef)
        {
            try
            {
                bool flag = map.terrainGrid.TerrainAt(location).defName.Equals(targetDef);
                if (flag)
                {
                    map.terrainGrid.SetTerrain(location, DefDatabase<TerrainDef>.GetNamed(targetDef, true));
                }
            }
            catch
            {
                //Log.Error("error setting terrrain" + targetDef, false);
            }
        }

        // Token: 0x06000016 RID: 22 RVA: 0x00002E7C File Offset: 0x0000107C
        public void genCrater(IntVec3 position, IntVec3 size, Map map)
        {
            float num = 1f / (float)(size.x * size.x);
            //Log.Message("A_coef:  " + num.ToString(), false);
            float num2 = 1f / (float)(size.z * size.z);
            float num3 = 0.75f * (float)size.x;
            float num4 = 0.75f * (float)size.z;
            float num5 = 0.87f * (float)size.x;
            float num6 = 0.87f * (float)size.z;
            float num7 = 1f / (num3 * num3);
            float num8 = 1f / (num4 * num4);
            float num9 = 1f / (num5 * num5);
            float num10 = 1f / (num6 * num6);
            float num11 = (float)size.x * 0.3f * (float)(Rand.Bool ? -1 : 1);
            int num12 = 0;
            for (int i = -size.x; i <= size.x; i++)
            {
                //Log.ResetMessageCount();
                for (int j = -size.z; j <= size.z; j++)
                {
                    float num13 = (float)(position.x + i);
                    float num14 = (float)(position.z + j);
                    bool flag = num * ((num13 - (float)position.x) * (num13 - (float)position.x)) + num2 * ((num14 - (float)position.z) * (num14 - (float)position.z)) <= 1f;
                    if (flag)
                    {
                        bool flag2 = num7 * ((num13 - (float)position.x + num11) * (num13 - (float)position.x + num11)) + num8 * ((num14 - (float)position.z + (float)num12) * (num14 - (float)position.z + (float)(Rand.Bool ? num12 : (-(float)num12)))) <= 1f;
                        if (flag2)
                        {
                            map.terrainGrid.SetTerrain(new IntVec3((int)num13, 1, (int)num14), DefDatabase<TerrainDef>.GetNamed("lunarSand", true));
                        }
                        else
                        {
                            bool flag3 = num9 * ((num13 - (float)position.x + num11) * (num13 - (float)position.x + num11)) + num10 * ((num14 - (float)position.z + (float)num12) * (num14 - (float)position.z + (float)(Rand.Bool ? num12 : (-(float)num12)))) <= 1f;
                            if (flag3)
                            {
                                GenSpawn.Spawn(DefDatabase<ThingDef>.GetNamed(((double)Rand.Value > 0.65) ? "MineableUranium" : "BiomesNEO_BasaltRock", true), new IntVec3((int)num13, 0, (int)num14), map, WipeMode.Vanish);
                            }
                            else
                            {
                                GenSpawn.Spawn(DefDatabase<ThingDef>.GetNamed(((double)Rand.Value > 0.65) ? "MineablePlasteel" : "BiomesNEO_MoonstoneRock", true), new IntVec3((int)num13, 0, (int)num14), map, WipeMode.Vanish);
                            }
                        }
                    }
                }
            }
        }
    }
}
