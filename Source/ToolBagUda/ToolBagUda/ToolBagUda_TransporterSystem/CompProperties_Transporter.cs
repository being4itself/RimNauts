using System;
using Verse;
using Verse.AI.Group;

namespace ToolBagUda
{
	public class CompProperties_Transporter : CompProperties
	{
		public CompProperties_Transporter()
		{
			this.compClass = typeof(Comp_Transporter);
		}


		public bool TransporterLock = false;
		public bool TransporterPad = false;

		public int secondsBetweenSpawns = 3;
		public float proxyRadius = 3.0f;
		public bool showGizmo = true;
		public string gizmoLabel = "Transporter";

	}
}
