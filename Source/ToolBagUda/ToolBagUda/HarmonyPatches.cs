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
	[StaticConstructorOnStartup]
	public static class HarmonyPatches
	{

		static HarmonyPatches()
		{
			Harmony harmony = new Harmony("rimworld.BorgRaceFork");
			harmony.Patch(AccessTools.Method(typeof(CompRefuelable), "ConsumeFuel", null, null), null, new HarmonyMethod(typeof(HarmonyPatches), "CompRefuelable_ConsumeFuel_PostFix", null), null, null);
	
			harmony.Patch(AccessTools.Method(typeof(Pawn), "PostApplyDamage", null, null), null, new HarmonyMethod(typeof(HarmonyPatches), "Pawn_PostApplyDamage_PostFix", null), null, null);
		}




		private static void Pawn_PostApplyDamage_PostFix(Pawn __instance, DamageInfo dinfo, float totalDamageDealt)
		{
			Pawn pawn = null; // init pawn as instigator of damage
			try
			{
				pawn = (dinfo.Instigator as Pawn);
			}
			catch
			{
				ModCore.Log("HarmonyPatches pawn = (dinfo.Instigator as Pawn); failed");
			}
			

			if (pawn != null && __instance != null) //if valid 
			{
				//ModCore.Log("valid instance and pawn");
				if (__instance.Dead)
				{
					//ModCore.Log("instance is dead");
					bool flag = __instance.RaceProps.Humanlike;
					if (flag)
                    {
						//ModCore.Log("instance is humalike");
						//ModCore.Log(pawn.def.race.body.defName);
						if (pawn.def.race.body.defName.Equals("Borg"))
						{
							//ModCore.Log("pawn is borg..."); ModCore.Log(pawn.equipment.Primary.def.defName.ToString());
							if (pawn.equipment.Primary.def.defName.Equals("Borg_InjectorWhipPlayer") || pawn.equipment.Primary.def.defName.Equals("Borg_InjectorWhip"))				//dinfo.Def.defName.Equals("BorgNaniteProbePlayer") || dinfo.Def.defName.Equals("BorgNaniteProbe"))
							{
								SpawnUtilityBorg utility = new SpawnUtilityBorg();
								utility.spawnBorgPawn(__instance, pawn.Faction.def.defName, pawn.Map);
							}
						}
                    						
					}
				}
			}
		}








		private static void CompRefuelable_ConsumeFuel_PostFix(CompRefuelable __instance, float amount)
		{
			/*int count = (int)__instance.Fuel;
			//ModCore.Log(count.ToString());
			if (__instance.Fuel == 0)
			{
				
				if (__instance.parent.def.defName.Equals("BorgSpawner"))
				{
					IntVec3 intVec = new IntVec3(__instance.parent.Map.Size.x / 2, 0, __instance.parent.Map.Size.z / 2);
					Lord defendShip = LordMaker.MakeNewLord(Faction.OfAncientsHostile, new LordJob_DefendShip(Faction.OfAncientsHostile, intVec), __instance.parent.Map, null);

					Pawn pawn2 = PawnGenerator.GeneratePawn(PawnKindDef.Named("BorgDrone3"), Faction.OfAncientsHostile);
					pawn2.Position = __instance.parent.Position;
					pawn2.SpawnSetup(__instance.parent.Map, false);
					defendShip.AddPawn(pawn2);
					__instance.parent.Destroy();

				}
			}*/
		}
	}


		

	}





