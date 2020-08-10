using System;
using HarmonyLib;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace ThatsAMoon
{
    // Token: 0x02000002 RID: 2
    [HarmonyPatch(typeof(WorldRendererUtility), "HiddenBehindTerrainNow")]
    internal static class WorldRendererUtility_HiddenBehindTerrainNow
    {
        // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        internal static void Postfix(Vector3 pos, ref bool __result)
        {
            Vector3 normalized = pos.normalized;
            Vector3 currentlyLookingAtPointOnSphere = Find.WorldCameraDriver.CurrentlyLookingAtPointOnSphere;
            float magnitude = pos.magnitude;
            float altitudePercent = Find.WorldCameraDriver.AltitudePercent;
            float num = 125f;
            float num2 = 1100f;
            float num3 = altitudePercent * (num2 - num) + num2;
            __result = ((double)Vector3.Angle(normalized, currentlyLookingAtPointOnSphere) > (Math.Acos((double)(115f / num3)) + Math.Acos((double)(115f / magnitude))) * 57.324840764331206);
            bool flag = magnitude < 115f;
            if (flag)
            {
                __result = (Vector3.Angle(normalized, currentlyLookingAtPointOnSphere) > 73f);
            }
        }
    }
}
