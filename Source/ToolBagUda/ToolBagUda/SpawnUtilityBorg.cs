using System;
using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;


namespace BorgAssimilate
{
    class SpawnUtilityBorg  
    {
        public SpawnUtilityBorg()
        {

        }
        ~SpawnUtilityBorg()
        {

        }




        public void spawnBorgPawn(Pawn deadPawn, String factionName,Map map)
        {
            try
            {
                bool flag = factionName.Equals(Faction.OfPlayer.def.defName);
                //ModCore.Log(flag.ToString());
                Faction faction = FactionUtility.DefaultFactionFrom(FactionDef.Named(factionName));
                // Map map = deadPawn.Map;
                Pawn spawn = PawnGenerator.GeneratePawn(flag ? this.playerPawnKind : this.hostilePawnKind, faction);
                spawn.Position = deadPawn.Position;
                if (map != null)
                {
                    
                    ModCore.Log("valid map");
                    spawn.SpawnSetup(map, false);
                }
                else
                {
                    ModCore.Log("invalid map");
                }

                if (flag)
                {
                    //ModCore.Log("setting faction to player");
                    spawn.SetFaction(Faction.OfPlayer, null);
                }
                else
                {

                    //ModCore.Log("applying duty");
                    IntVec3 intVec = new IntVec3(map.Size.x / 2, 0, map.Size.z / 2);// ModCore.Log(faction.ToString()); ModCore.Log(intVec.ToString());
                    LordJob_DefendShip defendShipJob = new LordJob_DefendShip(faction, intVec);
                    Lord defendShip = LordMaker.MakeNewLord(faction, defendShipJob, map, null);
                    defendShip.AddPawn(spawn);
                }
                if (!deadPawn.Destroyed)
                {
                    deadPawn.Destroy(DestroyMode.Vanish);
                }
                Corpse corpse = deadPawn.Corpse;
                bool flag2 = corpse != null;
                if (flag2)
                {
                    corpse.Destroy(DestroyMode.Vanish);
                }

            }
            catch
            {
                ModCore.Log("Spawn Error Handled");
            }
}

        public void spawnBorgPawn2(bool isPlayer, PawnKindDef pawnKind, IntVec3 location, Map map)
        {
            try
            {
                Pawn spawn = PawnGenerator.GeneratePawn((pawnKind != null ? pawnKind : hostilePawnKind), (isPlayer ? Faction.OfPlayer : Faction.OfAncientsHostile));
                spawn.Position = location;
                if (map != null)
                {
                    ModCore.Log("valid map");
                    spawn.SpawnSetup(map, false);
                }
                else
                {
                    ModCore.Log("invalid map");
                    spawn.SpawnSetup(spawn.Map, false);
                }

                if (isPlayer)
                {
                    //ModCore.Log("setting faction to player");
                    spawn.SetFaction(Faction.OfPlayer, null);
                }
                else
                {

                    //ModCore.Log("applying duty");
                    IntVec3 intVec = new IntVec3(map.Size.x / 2, 0, map.Size.z / 2);// ModCore.Log(faction.ToString()); ModCore.Log(intVec.ToString());
                    LordJob defendShipJob = new LordJob_AssaultColony(Faction.OfAncientsHostile); //, intVec, (int)(Rand.Value * 100));
                    Lord defendShip = LordMaker.MakeNewLord(Faction.OfAncientsHostile, defendShipJob, map, null);
                    defendShip.AddPawn(spawn);
                }
            }
            catch
            {
                ModCore.Log("Spawn Error Handled");
            }


        }

        public void spawnBorgPawn3(bool isPlayer, PawnKindDef pawnKind, IntVec3 location, Map map)
        {
            try
            {

                Pawn spawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(DefDatabase<PawnKindDef>.GetNamed("BorgDrone3", true), Faction.OfAncientsHostile, 
                    PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, true, false, false, false, false, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null));

                GenSpawn.Spawn(spawn, location, map, WipeMode.Vanish);

                //Pawn spawn = PawnGenerator.GeneratePawn((pawnKind != null ? pawnKind : hostilePawnKind), (isPlayer ? Faction.OfPlayer : Faction.OfAncientsHostile));
                spawn.Position = location;
                if (map != null)
                {
                    ModCore.Log("valid map");
                    spawn.SpawnSetup(map, false);
                }
                else
                {
                    ModCore.Log("invalid map");
                    spawn.SpawnSetup(spawn.Map, false);
                }

                if (isPlayer)
                {
                    //ModCore.Log("setting faction to player");
                    spawn.SetFaction(Faction.OfPlayer, null);
                }
                else
                {

                    //ModCore.Log("applying duty");
                    IntVec3 intVec = new IntVec3(map.Size.x / 2, 0, map.Size.z / 2);// ModCore.Log(faction.ToString()); ModCore.Log(intVec.ToString());
                    LordJob defendShipJob = new LordJob_AssaultColony(Faction.OfAncientsHostile); //, intVec, (int)(Rand.Value * 100));
                    Lord defendShip = LordMaker.MakeNewLord(Faction.OfAncientsHostile, defendShipJob, map, null);
                    defendShip.AddPawn(spawn);
                }
            }
            catch
            {
                ModCore.Log("Spawn Error Handled");
            }


        }


        public bool spawnBorgPawnX(bool isPlayer, PawnKindDef pawnKind, IntVec3 location, Map map)//,out Pawn pawn)
        {
            Pawn pawn = null; ModCore.Log("spawn start");
            if (this.hostilePawnKind == null)
            {
                pawn = null;
                return false;
            }
            int index = this.hostilePawnKind.lifeStages.Count - 0; ModCore.Log("spawn cont");
            pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(this.hostilePawnKind, Faction.OfAncientsHostile, PawnGenerationContext.NonPlayer, -1, false, false, false, false, 
                true, false, 1f, false, true, true, true, false, false, false, false, 0f, null, 1f, null, null, null, null, null, 
                new float?(this.hostilePawnKind.race.race.lifeStageAges[index].minAge), null, null, null, null, null, null));
            ModCore.Log("spawn made");
            //this.spawnedPawns.Add(pawn);
            GenSpawn.Spawn(pawn, CellFinder.RandomClosewalkCellNear(location, map, 5, null), map, WipeMode.Vanish);
            ModCore.Log("spawn init");
            LordJob defendShipJob = new LordJob_AssaultColony(Faction.OfAncientsHostile); //, intVec, (int)(Rand.Value * 100));
            Lord defendShip = LordMaker.MakeNewLord(Faction.OfAncientsHostile, defendShipJob, map, null);
            defendShip.AddPawn(pawn);
            ModCore.Log("spawn assigned");

            return true;
        }



        public static bool isBorg(Pawn pawn)
        {
            return pawn.def.race.body.defName.Equals("borg");
        }

        private PawnKindDef getPlayerKind()
        {
            return this.playerPawnKind;
        }
        private PawnKindDef getHostileKind()
        {
            return this.hostilePawnKind;
        }
        public float spawnChanceAI = 1.0f;
        public float spawnChancePlayer = 1.0f;

        public PawnKindDef playerPawnKind = PawnKindDef.Named("PlayerBorgDrone");
        public PawnKindDef hostilePawnKind=PawnKindDef.Named("BorgDrone3");
        private Faction factionHostile = Faction.OfAncientsHostile;
        public List<Pawn> spawnedPawns;
    }
}
