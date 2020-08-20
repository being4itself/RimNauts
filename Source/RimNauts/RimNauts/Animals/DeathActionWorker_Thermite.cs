using RimWorld;
using Verse;

namespace RimNauts
{
    public class DeathActionWorker_Thermite : DeathActionWorker
    {
        public override void PawnDied(Corpse corpse)
        {
            Faction newFaction = corpse.InnerPawn.Faction;
            if (corpse.def.defName== "Corpse_Thermite") {
                PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDef.Named("Worm"), newFaction, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, false, false);
                Pawn pawn = PawnGenerator.GeneratePawn(request);
                Pawn pawn2 = PawnGenerator.GeneratePawn(request);
                Pawn pawn3 = PawnGenerator.GeneratePawn(request);
                PawnUtility.TrySpawnHatchedOrBornPawn(pawn, corpse.InnerPawn);
                PawnUtility.TrySpawnHatchedOrBornPawn(pawn2, corpse.InnerPawn);
                PawnUtility.TrySpawnHatchedOrBornPawn(pawn3, corpse.InnerPawn);
            }
        }
    }
}