using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace ThatsAMoon
{
    // Token: 0x0200000C RID: 12
    [HarmonyPatch(typeof(TravelingTransportPods))]
    [HarmonyPatch("Start", MethodType.Getter)]
    public static class FromSatellite
    {
        // Token: 0x06000030 RID: 48 RVA: 0x0000360C File Offset: 0x0000180C
        [HarmonyPostfix]
        public static void StartAtShip(TravelingTransportPods __instance, ref Vector3 __result)
        {
            int num = (int)typeof(TravelingTransportPods).GetField("initialTile", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance);
            foreach (WorldObject worldObject in Enumerable.Where<WorldObject>(Find.World.worldObjects.AllWorldObjects, (WorldObject o) => o is WorldObjectChild_Satellite))
            {
                bool flag = worldObject.Tile == num;
                if (flag)
                {
                    __result = worldObject.DrawPos;
                }
            }
        }
    }
}
