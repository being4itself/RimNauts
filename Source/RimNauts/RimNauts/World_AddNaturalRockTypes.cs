using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld.Planet;
using Verse;

namespace ThatsAMoon
{
    // Token: 0x02000004 RID: 4
    [HarmonyPatch(typeof(World), "NaturalRockTypesIn")]
    internal static class World_AddNaturalRockTypes
    {
        // Token: 0x06000010 RID: 16 RVA: 0x0000258C File Offset: 0x0000078C
        internal static void Postfix(int tile, ref IEnumerable<ThingDef> __result, ref World __instance)
        {
            Traverse traverse = Traverse.Create(__instance);
            WorldGrid value = traverse.Field("grid").GetValue<WorldGrid>();
            bool flag = value[tile].biome.defName.Equals("RockMoonBiome");
            bool flag2 = flag;
            if (flag2)
            {
                __result = new List<ThingDef>
                {
                    DefDatabase<ThingDef>.GetNamed("BiomesNEO_MoonstoneRock", true)
                };
            }
            else
            {
                bool flag3 = Enumerable.Contains<ThingDef>(__result, DefDatabase<ThingDef>.GetNamed("BiomesNEO_MoonstoneRock", true));
                bool flag4 = Enumerable.Contains<ThingDef>(__result, DefDatabase<ThingDef>.GetNamed("BiomesNEO_BasaltRock", true));
                bool flag5 = flag3 || flag4;
                if (flag5)
                {
                    Rand.PushState();
                    Rand.Seed = tile;
                    List<ThingDef> rocks = Enumerable.ToList<ThingDef>(__result);
                    bool flag6 = flag3;
                    if (flag6)
                    {
                        rocks.Remove(DefDatabase<ThingDef>.GetNamed("BiomesNEO_MoonstoneRock", true));
                    }
                    bool flag7 = flag4;
                    if (flag7)
                    {
                        rocks.Remove(DefDatabase<ThingDef>.GetNamed("BiomesNEO_BasaltRock", true));
                    }
                    List<ThingDef> list = Enumerable.ToList<ThingDef>(Enumerable.Where<ThingDef>(DefDatabase<ThingDef>.AllDefs, (ThingDef d) => d.category.Equals(ThingCategory.Building) && d.building.isNaturalRock && !d.building.isResourceRock && !d.IsSmoothed && !rocks.Contains(d) && d.defName != "BiomesNEO_MoonstoneRock" && d.defName != "BiomesNEO_BasaltRock"));
                    bool flag8 = !list.NullOrEmpty<ThingDef>();
                    bool flag9 = flag8;
                    if (flag9)
                    {
                        rocks.Add(list.RandomElement<ThingDef>());
                    }
                    __result = rocks;
                    Rand.PopState();
                }
            }
        }
    }
}
