using System;
using RimWorld;
using Verse;

namespace BorgAssimilate
{
	// Token: 0x02000003 RID: 3
	public class Hediff_BorgInfectionPlayer : Hediff_Injury
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002124 File Offset: 0x00000324
		public override void Notify_PawnDied()
		{
			base.Notify_PawnDied();
			bool flag = !this.pawn.def.race.Animal || !this.pawn.def.race.IsMechanoid;
			
			if (flag)
			{
				Corpse corpse = this.pawn.Corpse;
				Pawn pawn = PawnGenerator.GeneratePawn(PawnKindDef.Named("PlayerBorgDrone"), FactionUtility.DefaultFactionFrom(FactionDef.Named("BorgCollective")));
				pawn.SetFaction(Faction.OfPlayer, null);
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
				bool mech = this.pawn.def.race.IsMechanoid;
				if (animal)
				{
					Messages.Message("an animal has succumbed to the nanite infection, and have been deemed inappropriate for assimilation. The nanites have consumed and destroyed the corpse.", MessageTypeDefOf.NeutralEvent, true);
					this.pawn.Corpse.Destroy(DestroyMode.Vanish);
				}
				if (mech)
				{
					Messages.Message("an Mechinoid has succumbed to the nanite infection, and have been deemed inappropriate for assimilation. The nanites have consumed and destroyed the corpse.", MessageTypeDefOf.NeutralEvent, true);
					this.pawn.Corpse.Destroy(DestroyMode.Vanish);
				}
			}
		}
	}
}
