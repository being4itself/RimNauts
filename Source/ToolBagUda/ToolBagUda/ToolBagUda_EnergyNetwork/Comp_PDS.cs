using System;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using System.Collections.Generic;

namespace ToolBagUda
{
	public class Comp_PDS : ThingComp
	{
		public CompProperties_PDS Props
		{
			get
			{
				return this.props as CompProperties_PDS;
			}
		}


		public float getPowerNetAmount(bool isWarp)
        {
			if(isWarp) return this.warpNet.currentEnergy;
			return this.energyNet.currentEnergy;
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
			yield return new Command_Action
			{
				defaultLabel = "Activate/Deactivate Warp Engine",
				action = delegate ()
				{
					this.activeLock = !this.activeLock;
					this.parent.BroadcastCompSignal("Warp Engine Shutdown");
				}
			};
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
				if (Find.TickManager.TicksGame >= this.nextTick)
                {
					this.energyNet.pushChargeAmount(this.EMProduced);
					this.warpNet.pushChargeAmount(this.WarpProduced);
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
			this.warpNet = Find.World.GetComponent<WarpPlasmaNet>();
			this.energyNet = Find.World.GetComponent<ElectroPlasmaNet>();
			Log.Message("linked to Power Net: ");
			//this.energyNet.InputDevices.Add(this.parent);
			//this.warpNet.InputDevices.Add(this.parent);
			Log.Message("Sync'd to Power Net: ");// + energyNet.InputDevices.Last<ThingWithComps>().def.defName.ToString());
			this.energyNet.energyCapacity = this.EMPlasmaCapacity;
			this.warpNet.energyCapacity = this.WarpPlasmaCapacity;
		}

		public int calcNextTick(int curTick)
		{
			return curTick + calcTickInterval();
		}

		public int calcTickInterval()
		{
			return this.Props.secondsBetweenPush * 60;
		}

		WarpPlasmaNet warpNet;
		ElectroPlasmaNet energyNet;
		public float EMProduced = 10000f;
		public float WarpProduced = 100;
		public float EMPlasmaCapacity = 1000000;
		public float WarpPlasmaCapacity = 10000;
		public int nextTick;
		public int lockTimeStart;
		public bool activeLock = false;
		public float warpOffSet = 0.5f;

		ThingWithComps PylonConduits;

		ThingWithComps WarpCore;
	}


}
