using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200007E RID: 126
	internal class LordJob_DefendShip : LordJob
	{
		// Token: 0x0600025A RID: 602 RVA: 0x000124EF File Offset: 0x000106EF
		public LordJob_DefendShip()
		{
		}

		// Token: 0x0600025B RID: 603 RVA: 0x000124F7 File Offset: 0x000106F7
		public LordJob_DefendShip(Faction faction, IntVec3 baseCenter)
		{
			this.faction = faction;
			this.baseCenter = baseCenter;
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0001250D File Offset: 0x0001070D
		public override StateGraph CreateGraph()
		{
			return new StateGraph
			{
				StartingToil = new LordToil_DefendShip(this.baseCenter)
			};
		}

		// Token: 0x0600025D RID: 605 RVA: 0x00012528 File Offset: 0x00010728
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<IntVec3>(ref this.baseCenter, "baseCenter", default(IntVec3), false);
		}

		// Token: 0x04000121 RID: 289
		private Faction faction;

		// Token: 0x04000122 RID: 290
		public IntVec3 baseCenter;
	}
}
