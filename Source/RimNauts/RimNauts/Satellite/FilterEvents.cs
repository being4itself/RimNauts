using HarmonyLib;
using RimWorld;
using Verse;

namespace RimNauts 
{
    [HarmonyPatch(typeof(PawnsArrivalModeWorker), "CanUseWith")]
    public class PawnsArrivalModeWorker_CanUseWith_Patch
    {
        public static void Postfix(bool __result, IncidentParms parms)
        {
            if (__result && Find.WorldGrid[parms.target.Tile].biome == MoonDefOf.RockMoonBiome)
            {
                if (!isDropType(parms.raidArrivalMode))
                {
                    __result = false;
                }
            }
        }
        private static bool isDropType(PawnsArrivalModeDef def)
        {
            // Any arrival mode requiring Industrial tech level is a drop pod type
            // May not be true for mods
            return def.minTechLevel >= TechLevel.Industrial;
        }
    }
}