using System.Linq;
using HarmonyLib;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace ThatsAMoon
{
    // Token: 0x0200000D RID: 13
    [HarmonyPatch(typeof(TravelingTransportPods))]
    [HarmonyPatch("End", MethodType.Getter)]
    public static class ToSatellite
    {
        // Token: 0x06000031 RID: 49 RVA: 0x000036C4 File Offset: 0x000018C4
        [HarmonyPostfix]
        public static void EndAtShip(TravelingTransportPods __instance, ref Vector3 __result)
        {
            int destinationTile = __instance.destinationTile;
            foreach (WorldObject worldObject in Enumerable.Where<WorldObject>(Find.World.worldObjects.AllWorldObjects, (WorldObject o) => o is WorldObjectChild_Satellite))
            {
                bool flag = worldObject.Tile == destinationTile;
                if (flag)
                {
                    __result = worldObject.DrawPos;
                }
            }
        }
    }
}
