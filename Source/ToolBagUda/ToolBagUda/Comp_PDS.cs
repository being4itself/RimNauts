using System;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using System.Collections.Generic;

namespace BorgAssimilate
{
	// Token: 0x02000007 RID: 7
	public class Comp_PDS : ThingComp
	{
		public CompProperties_PDS Props
		{
			get
			{
				return this.props as CompProperties_PDS;
			}
		}


		public float getPowerNetAmount()
        {
			PlasmaEnergy energyNet = Find.World.GetComponent<PlasmaEnergy>();
			return energyNet.EMPlasmaAmount;
		}
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{

			if (this.Props.showGizmo && Find.Selector.SingleSelectedThing == this.parent)
			{
				yield return new Gizmo_PowerReader
				{
					engine = this
				};
			}
	
			if (Prefs.DevMode)
			{
				yield return new Command_Action
				{
					defaultLabel = "Debug: Set output to 0",
					action = delegate ()
					{
						this.EMProduced = 0f;
						this.parent.BroadcastCompSignal("Refueled");
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "Debug: Set output to 1000",
					action = delegate ()
					{
						this.EMProduced = 1000f;
						this.parent.BroadcastCompSignal("Refueled");
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "Debug: Set output to 100000",
					action = delegate ()
					{
						this.EMProduced = 100000f;
						this.parent.BroadcastCompSignal("Refueled");
					}
				};
			}
			yield break;
		}




		public override void CompTick()
		{
            if (this.activeLock)
            {
				PlasmaEnergy energyNet = Find.World.GetComponent<PlasmaEnergy>();
				if (Find.TickManager.TicksGame >= this.nextTick)
                {
					energyNet.pushChargeAmount(this.EMProduced);
					this.nextTick = calcNextTick(Find.TickManager.TicksGame);
					//Log.Message("energy pushed to net: " + this.EMProduced.ToString());
				}
            }

		}
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);

			Log.Message("Starting Reactor");
			this.activeLock = true;
			this.nextTick = calcNextTick(Find.TickManager.TicksGame);

			PlasmaEnergy energyNet = Find.World.GetComponent<PlasmaEnergy>();
			energyNet.PDS = this.parent; Log.Message("linked to Power Net: " + energyNet.PDS.def.defName.ToString());
			energyNet.EMPlasmaCapacity = this.EMPlasmaCapacity;
			energyNet.WarpPlasmaCapacity = this.WarpPlasmaCapacity;
			energyNet.EMPlasmaChangeRate = this.EMPlasmaChangeRate;
			energyNet.WarpPlasmaChangeRate = this.WarpPlasmaChangeRate;
		}

		public int calcNextTick(int curTick)
		{
			return curTick + calcTickInterval();
		}

		public int calcTickInterval()
		{
			return this.Props.secondsBetweenSpawns * 60;
		}

		public float EMProduced = 10000f;
		public float WarpProduced = 1000f;

		public float EMPlasmaCapacity = 1000000;
		public float WarpPlasmaCapacity = 1000000;

		public float EMPlasmaChangeRate = 1;   
		public float WarpPlasmaChangeRate = 1;

		public int nextTick;
		public int lockTimeStart;
		public bool activeLock = false;

		public float warpOffSet = 0.5f;

		public IntVec3 lockPos;
		public Map lockMap;

		public List<Pawn> patternBuffer;

	}


}
