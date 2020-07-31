using System;
using RimWorld;
using Verse;

namespace BorgAssimilate
{
	// Token: 0x02000002 RID: 2
	public class Hediff_BorgInfection : Hediff_Injury
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public override void Notify_PawnDied()
		{
			base.Notify_PawnDied();
			bool flag = !this.pawn.def.race.Animal;
			if (flag)
			{
				Corpse corpse = this.pawn.Corpse;
				Pawn pawn = PawnGenerator.GeneratePawn(PawnKindDef.Named("BorgDrone3"), FactionUtility.DefaultFactionFrom(FactionDef.Named("BorgCollective")));
				pawn.Position = corpse.Position;
				pawn.SpawnSetup(corpse.Map, false);
				bool flag2 = corpse != null;
				if (flag2)
				{
					corpse.Destroy(DestroyMode.Vanish);
				}
			}
			else
			{
				bool animal = this.pawn.def.race.Animal;
				if (animal)
				{
					Messages.Message("an animal has succumbed to nanite infection, and have been deemed inappropriate for assimilation. The nanites have consumed and destroyed the corpse.", MessageTypeDefOf.NeutralEvent, true);
					this.pawn.Corpse.Destroy(DestroyMode.Vanish);
				}
			}
		}
	}
}
