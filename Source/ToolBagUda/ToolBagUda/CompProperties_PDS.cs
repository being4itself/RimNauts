using System;
using Verse;
using Verse.AI.Group;

namespace BorgAssimilate
{
	public class CompProperties_PDS : CompProperties
	{
		public CompProperties_PDS()
		{
			this.compClass = typeof(Comp_PDS);
		}


		public bool TransporterLock = false;

		public int secondsBetweenSpawns = 10;
		public float proxyRadius = 3.0f;
		public bool showGizmo = true;
		public string gizmoLabel = "Electro Plasma";

	}
}
