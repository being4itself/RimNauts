using System;
using Verse;
using Verse.AI.Group;

namespace ToolBagUda
{
	public class CompProperties_PDS : CompProperties
	{
		public CompProperties_PDS()
		{
			this.compClass = typeof(Comp_PDS);
		}

		public float EMProduced = 100f;
		public float WarpProduced = 100f;
		public float EMPlasmaCapacity = 10000f;
		public float WarpPlasmaCapacity = 10000f;

		public int secondsBetweenPush = 10;
		public float proxyRadius = 3.0f;
		public bool showGizmo = true;
		public string gizmoLabel = "Plasma Distribution System";

	}
}
