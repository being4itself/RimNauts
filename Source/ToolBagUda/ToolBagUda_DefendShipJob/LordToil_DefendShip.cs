using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200007F RID: 127
	[StaticConstructorOnStartup]
	internal class LordToil_DefendShip : LordToil
	{
		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600025E RID: 606 RVA: 0x00012566 File Offset: 0x00010766
		public override IntVec3 FlagLoc
		{
			get
			{
				return this.baseCenter;
			}
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0001256E File Offset: 0x0001076E
		public LordToil_DefendShip(IntVec3 baseCenter)
		{
			this.baseCenter = baseCenter;
		}

		// Token: 0x06000260 RID: 608 RVA: 0x00012580 File Offset: 0x00010780
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(LordToil_DefendShip.defendShip, this.baseCenter, -1f);
			}
		}

		// Token: 0x04000123 RID: 291
		private static DutyDef defendShip = DefDatabase<DutyDef>.GetNamed("SoSDefendShip", true);

		// Token: 0x04000124 RID: 292
		public IntVec3 baseCenter;
	}
}
