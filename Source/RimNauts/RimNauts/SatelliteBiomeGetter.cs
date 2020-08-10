using HarmonyLib;
using RimWorld;
using Verse;

namespace ThatsAMoon
{
    // Token: 0x02000007 RID: 7
    [HarmonyPatch(typeof(Map))]
    [HarmonyPatch("Biome", MethodType.Getter)]
    public static class SatelliteBiomeGetter
    {
        // Token: 0x06000020 RID: 32 RVA: 0x000032F0 File Offset: 0x000014F0
        [HarmonyPrefix]
        public static bool interceptBiome(Map __instance)
        {
            MapInfo info = __instance.info;
            SatelliteBiomeGetter.testResult = (((info != null) ? info.parent : null) != null && __instance.info.parent is WorldObjectChild_Satellite);
            return !SatelliteBiomeGetter.testResult;
        }

        // Token: 0x06000021 RID: 33 RVA: 0x0000333C File Offset: 0x0000153C
        [HarmonyPostfix]
        public static void getSpaceBiome(Map __instance, ref BiomeDef __result)
        {
            bool flag = SatelliteBiomeGetter.testResult;
            if (flag)
            {
                __result = SatelliteBiomeGetter.RockMoonBiome;
            }
        }

        // Token: 0x0400000E RID: 14
        private static bool testResult;

        // Token: 0x0400000F RID: 15
        public static BiomeDef RockMoonBiome = DefDatabase<BiomeDef>.GetNamed("RockMoonBiome", true);
    }
}
