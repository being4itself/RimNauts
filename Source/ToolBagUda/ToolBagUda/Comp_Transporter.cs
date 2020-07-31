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
	public class Comp_Transporter : ThingComp
	{
		public CompProperties_Transporter Props
		{
			get
			{
				return this.props as CompProperties_Transporter;
			}
		}



		// RimWorld.CompRefuelable
		// Token: 0x06005292 RID: 21138 RVA: 0x001B9827 File Offset: 0x001B7A27
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			/*return null;
			if (this.Props.targetFuelLevelConfigurable)
			{
				yield return new Command_SetTargetFuelLevel
				{
					transporterConsole = this,
					defaultLabel = "CommandSetTargetFuelLevel".Translate(),
					defaultDesc = "CommandSetTargetFuelLevelDesc".Translate(),
					icon = CompRefuelable.SetTargetFuelLevelCommand
				};
			}
			if (this.Props.showGizmo && Find.Selector.SingleSelectedThing == this.parent)
			{
				yield return new Gizmo_TransporterReader
				{
					transporterConsole = this
				};
			}
			/*if (this.Props.showAllowAutoRefuelToggle)
			{
				yield return new Command_Toggle
				{
					defaultLabel = "CommandToggleAllowAutoRefuel".Translate(),
					defaultDesc = "CommandToggleAllowAutoRefuelDesc".Translate(),
					hotKey = KeyBindingDefOf.Command_ItemForbid,
					icon = (this.allowAutoRefuel ? TexCommand.ForbidOff : TexCommand.ForbidOn),
					isActive = (() => this.allowAutoRefuel),
					toggleAction = delegate ()
					{
						this.allowAutoRefuel = !this.allowAutoRefuel;
					}
				};
			}*/
			if (Prefs.DevMode)
			{
				yield return new Command_Action
				{
					defaultLabel = "Debug: Set fuel to 0",
					action = delegate ()
					{
						//this.fuel = 0f;
						this.parent.BroadcastCompSignal("Refueled");
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "Debug: Set fuel to 0.1",
					action = delegate ()
					{
						//this.fuel = 0.1f;
						this.parent.BroadcastCompSignal("Refueled");
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "Debug: Set fuel to max",
					action = delegate ()
					{
						//this.fuel = this.Props.fuelCapacity;
						this.parent.BroadcastCompSignal("Refueled");
					}
				};
			}
			yield break;
		}




		public override void CompTick()
		{

			//this.active = proxiTrigger(this.active);


			if (this.activeLock)
			{
				MoteMaker.ThrowDustPuffThick(new Vector3((float)this.parent.Position.x + this.warpOffSet, (float)this.parent.Position.y + this.warpOffSet, (float)this.parent.Position.z + this.warpOffSet), this.parent.Map, 1, new Color(0, 1, 0));

				if (Find.TickManager.TicksGame >= this.nextTick)
				{
					PlasmaEnergy energyNet = Find.World.GetComponent<PlasmaEnergy>();
					
					TransporterNetwork transNet = Find.World.GetComponent<TransporterNetwork>();
					foreach (Pawn pattern in transNet.BufferedPatterns)
					{
						GenSpawn.Spawn(pattern, transNet.lockPoint.Position, transNet.lockPoint.Map, WipeMode.Vanish);
						energyNet.tryDrainAmount(100000f);
					}
					this.parent.Destroy(DestroyMode.Vanish);
					transNet.BufferedPatterns.Clear();
				}


				//

			}
			else if (this.activeSite)
			{
				MoteMaker.ThrowDustPuffThick(new Vector3((float)this.parent.Position.x + this.warpOffSet, (float)this.parent.Position.y + this.warpOffSet, (float)this.parent.Position.z + this.warpOffSet), this.parent.Map, this.Props.proxyRadius, new Color(0, 1, 0));

				if (Find.TickManager.TicksGame >= this.nextTick)
				{
					TransporterPadToBuffer();
					this.parent.Destroy(DestroyMode.Vanish);
				}
			}

		}
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			TransporterNetwork transNet = Find.World.GetComponent<TransporterNetwork>();
			if (this.Props.TransporterLock)
			{
				
				if (transNet.transporter != null)
				{
					transNet.lockPoint = this.parent;
					transNet.LockPosition = this.parent.Position; Log.Message("pos:" + transNet.LockPosition.x.ToString());
					transNet.LockMap = this.parent.Map;

					


					this.lockTimeStart = Find.TickManager.TicksGame;
					this.nextTick = calcNextTick(this.lockTimeStart);
					this.activeLock = true;

				}

			}
			else
			{
				

				transNet.transporter = this.parent; Log.Message("transporter:" + transNet.transporter.def.defName.ToString());
				transNet.TransporterPostiion = this.parent.Position; Log.Message("pos:" + transNet.TransporterPostiion.x.ToString());
				transNet.TransporterMap = this.parent.Map;

				this.activeSite = true;

				this.lockTimeStart = Find.TickManager.TicksGame;
				this.nextTick = calcNextTick(this.lockTimeStart);



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


		public void TransporterPadToBuffer()
        {
			Log.Message("init buffering");
			TransporterNetwork transNet = Find.World.GetComponent<TransporterNetwork>();
			try
			{
				Log.Message("scanning pad.." + transNet.TransporterPostiion.x.ToString() + transNet.TransporterPostiion.z.ToString());
			}
            catch
            {
				Log.Message("scan failed");

			}
			Log.Message("transporterXX:" + transNet.transporter.def.defName.ToString());
			Log.Message("scanning pad.." + transNet.transporter.Position.x.ToString() + transNet.transporter.Position.z.ToString());
			if (transNet.transporter != null && transNet.transporter.Map != null)
            {
				List<Thing> list = GenRadial.RadialDistinctThingsAround(transNet.transporter.Position, transNet.transporter.Map, this.Props.proxyRadius, true).ToList<Thing>();
				bool flag3 = list.Count > 1;
				if (flag3)
				{
					Log.Message("active check");
					foreach (Thing thingInd in list)

					{
						if (thingInd.def.race != null)
						{


							transNet.BufferedPatterns.Add(thingInd as Pawn);
							transToBuffer(thingInd);


						}

					}

				}
			}
            else
            {
				Log.Message("Scanner failed");

			}
			
		}

		public void transToBuffer(Thing pawn)
        {
			pawn.DeSpawn(DestroyMode.Vanish);
        }

		public void setTransporterLock(IntVec3 pos, Map map)
        {


			this.lockPos = pos;
			this.lockMap = map;
        }


		public void pullFromBuffer()
        {

			

		}


		/*public void SpawnPawn()
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
		*/
		public int calcNextTick(int curTick)
		{
			return curTick + calcTickInterval();
		}

		public int calcTickInterval()
		{
			return this.Props.secondsBetweenSpawns * 60;
		}


		public int getBufferAmount()
        {
			return this.patternBuffer.Count();
        }
		public int timesTriggered = 0;
		public int nextTick;
		public int lockTimeStart;
		public bool activeLock = false;

		public bool activeSite = false;
		public float warpOffSet = 0.5f;

		public IntVec3 lockPos;
		public Map lockMap;

		public List<Pawn> patternBuffer;
		
	}


}
