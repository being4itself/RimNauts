using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimNauts
{
	/// <summary>
	/// Covers all bases for grabbing Tile's rendering location due to DrawPos of Satellite being overridden and moved to orbit
	/// </summary>
	/// <remarks>
	/// Concerned about compatibility and "applying" this to every single tile / world object? You can do a postfix instead, but you'll take a
	/// performance hit due to looping through all tile vertices as opposed to only performing the bucket lookup operation
	/// </remarks>
	[HarmonyPatch(typeof(WorldGrid), nameof(WorldGrid.GetTileCenter))]
	public static class SatelliteTileCenter
    {
		[HarmonyPrefix]
		public static bool Prefix(int tileID, ref Vector3 __result)
        {
			if (Gamecomp_SatellitesInOrbit.cachedWorldObjectTiles.TryGetValue(tileID, out var satellite))
            {
				__result = satellite.DrawPos;
				return false;
            }
			return true;
        }
    }

	/// <summary>
	/// Register Satellite WorldObject in cache post AddToCache
	/// </summary>
	[HarmonyPatch(typeof(WorldObjectsHolder), "AddToCache")]
	public static class WorldObjectRegister
    {
		[HarmonyPrefix]
		public static void Postfix(WorldObject o)
        {
			if (o is WorldObjectChild_Satellite satellite && !Gamecomp_SatellitesInOrbit.cachedWorldObjectTiles.ContainsKey(o.Tile))
            {
				Gamecomp_SatellitesInOrbit.cachedWorldObjectTiles.Add(o.Tile, satellite);
            }
        }
    }

	/// <summary>
	/// Remove Satellite WorldObject from cache post RemoveFromCache
	/// </summary>
	[HarmonyPatch(typeof(WorldObjectsHolder), "RemoveFromCache")]
	public static class WorldObjectDeregister
    {
		[HarmonyPrefix]
		public static void Postfix(WorldObject o)
        {
			if (o is WorldObjectChild_Satellite)
            {
				Gamecomp_SatellitesInOrbit.cachedWorldObjectTiles.Remove(o.Tile);
            }
        }
    }
}
