using System;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using System.Collections.Generic;

namespace ToolBagUda
{
	// Token: 0x02000007 RID: 7
	public class Comp_PawnSpawner : ThingComp
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000011 RID: 17 RVA: 0x000022DB File Offset: 0x000004DB
		public CompProperties_PawnSpawner Props
		{
			get
			{
				return this.props as CompProperties_PawnSpawner;
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000022E8 File Offset: 0x000004E8
		public override void CompTick()
		{
			
			this.active = proxiTrigger(this.active);


			if (this.active)
			{

				{ 
				MoteMaker.ThrowDustPuffThick(new Vector3((float)this.parent.Position.x + this.warpOffSet, (float)this.parent.Position.y + this.warpOffSet, (float)this.parent.Position.z + this.warpOffSet), this.parent.Map, 1, new Color(0, 1, 0));
			}
				if (Find.TickManager.TicksGame >= this.nextTick)
				{
					if (this.numberSpawned < this.Props.numberToSpawn)
					{
						this.SpawnPawn();
						this.numberSpawned += 1;
						this.nextTick = calcNextTick(Find.TickManager.TicksGame);
					}
					else
					{

						this.active = false;
						if (this.Props.selfDestruct)
						{
							if (this.Props.getDrops)
                            {
								this.parent.Destroy(DestroyMode.Refund);
							}
                            else
                            {
								this.parent.Destroy(DestroyMode.Vanish);
							}
							
						}
					}
				}
			}
		}
        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
			
			this.numberSpawned = 0;
			MoteMaker.ThrowSmoke(new Vector3((float)this.parent.Position.x, (float)this.parent.Position.y, (float)this.parent.Position.z),this.parent.Map,1);
            if (this.Props.isProxy)
            {
				this.active = false;
			}
			else
            {
				this.active = true;
				this.nextTick = calcNextTick(Find.TickManager.TicksGame);
			}
			
		}

		public bool proxiTrigger(bool isActive)
        {
			if (isActive) return isActive; //return true if active
			if (this.timesTriggered > 0) return isActive; //retrun false if been triggered 
			List<Thing> list = GenRadial.RadialDistinctThingsAround(this.parent.Position, this.parent.Map, this.Props.proxyRadius, true).ToList<Thing>();
			bool flag3 = list.Count > 1;
			if (flag3)
			{
				Log.Message("active check");
				foreach (Thing thingInd in list)

				{
					if (thingInd.def.race != null)
					{
						Log.Message("thing detected:  " + thingInd.def.race.body.ToString());
						if (thingInd.Faction != null)
						{
							Log.Message("thing detected:  " + thingInd.Faction.ToString());
							if (thingInd.Faction.Equals(Faction.OfPlayer))
							{
								this.timesTriggered += 1;
								this.nextTick = calcNextTick(Find.TickManager.TicksGame);
								return !isActive;
							}
						}
					}

				}
			}



			return false;
		}

		public LordJob getaJob()
        {
			LordJob result =   new LordJob_AssaultColony(Faction.OfAncientsHostile); ;
			if (this.Props.lordJob == "assault") return result;
			//if (this.Props.lordJob == "defendShip") return new LordJob_DefendShip(Faction.OfAncientsHostile, new IntVec3(this.parent.Map.Size.x / 2, 0, this.parent.Map.Size.z / 2));
			if (this.Props.lordJob == "defendBase") return new LordJob_DefendBase(Faction.OfAncientsHostile, new IntVec3(this.parent.Map.Size.x / 2, 0, this.parent.Map.Size.z / 2));
			if (this.Props.lordJob == "manTurrets") return new LordJob_ManTurrets();
			if (this.Props.lordJob == "siege") return new LordJob_Siege(Faction.OfAncientsHostile, new IntVec3(this.parent.Map.Size.x / 2, 0, this.parent.Map.Size.z / 2),3000);
			if (this.Props.lordJob == "stageThenAttack") return new LordJob_StageThenAttack(Faction.OfAncientsHostile, new IntVec3(this.parent.Map.Size.x / 2, 0, this.parent.Map.Size.z / 2),300);


			return result;
		}



        public void SpawnPawn()
		{
			bool newborn = this.Props.newborn;
			PawnGenerationRequest request = new PawnGenerationRequest(this.Props.pawnKind, !this.Props.isPlayer ? Faction.OfAncientsHostile : Faction.OfPlayer, PawnGenerationContext.NonPlayer, -1, true, newborn, false, false, true, false, 1f, false, true, true, true, false, false, false, false, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null);
			Pawn newThing = PawnGenerator.GeneratePawn(request);
			GenSpawn.Spawn(newThing, this.parent.Position, this.parent.Map, WipeMode.Vanish);
			if (!this.Props.isPlayer)
            {
				//IntVec3 intVec = new IntVec3(this.parent.Map.Size.x / 2, 0, this.parent.Map.Size.z / 2);
				//LordJob defendShipJob = new LordJob_AssaultColony(Faction.OfAncientsHostile); //, intVec, (int)(Rand.Value * 100));
				LordJob defendShipJob = getaJob();
				Lord defendShip = LordMaker.MakeNewLord(Faction.OfAncientsHostile, defendShipJob, this.parent.Map, null);
				defendShip.AddPawn(newThing);
			}
		}
		
		public int calcNextTick(int curTick)
        {
			return curTick + calcTickInterval();
        }

		public int calcTickInterval()
        {
			return this.Props.secondsBetweenSpawns * 60;
        }

		public void selfDeconstruct()
        {
			CompProperties thisComp = (CompProperties)this.parent.def.comps.Find((CompProperties x) => x.compClass.Name.Equals("Comp_PawnSpawner"));

			this.parent.def.comps.Remove(thisComp);
		}

		public int timesTriggered = 0;
		public int nextTick;
		public int numberSpawned;
		public bool active = false;
		public float warpOffSet = 0.5f;
	}

	
}
