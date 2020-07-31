using System;
using Verse;
using Verse.AI.Group;

namespace ToolBagUda
{
	public class CompProperties_PawnSpawner : CompProperties
	{
		public CompProperties_PawnSpawner()
		{
			this.compClass = typeof(Comp_PawnSpawner);
		}


		public bool isPlayer = false;

		public int secondsBetweenSpawns = 1;

		public int numberToSpawn = 1;

		public PawnKindDef pawnKind;

		public bool newborn = false;

		public bool selfDestruct = true;
		public bool isProxy = false;
		public bool getDrops = false;
		public float proxyRadius = 3.0f;

		public string lordJob;

		//public Color glowColor ;// new Color(255, 255, 255, 0) * 1.45f;
	}
}
