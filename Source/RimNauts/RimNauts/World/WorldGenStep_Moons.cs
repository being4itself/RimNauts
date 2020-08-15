using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimNauts
{
	public class WorldGenStep_Moons : WorldGenStep
	{
		public override int SeedPart
		{
			get
			{
				return 133714088;
			}
		}
		public override void GenerateFresh(string seed)
		{

			for (int i = 0; i <= (int)Rand.Range(40, 80); i++)
			{
				Current.Game.GetComponent<Gamecomp_SatellitesInOrbit>().tryGenSatellite(Find.World.grid.TilesCount);

			}
		}

		public override void GenerateFromScribe(string seed)
		{
			for (int i = 0; i <= (int)Rand.Range(40, 80); i++)
			{
				Current.Game.GetComponent<Gamecomp_SatellitesInOrbit>().tryGenSatellite(Find.World.grid.TilesCount);

			}
		}


		
	} 
}

