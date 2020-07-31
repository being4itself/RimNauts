using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ToolBagUda
{
    public class ElectroPlasmaNet : EnergyNetwork
    {

        public ElectroPlasmaNet(World world) : base(world)
        {

        }
        public override void FinalizeInit()
        {
            this.nextTick = base.getNextTick(Find.TickManager.TicksGame, rareInt);
            base.FinalizeInit();

            this.energyName = "Electro-Plasma";
            this.energyCapacity = 100;
        }
        public override void WorldComponentTick()
        {
            base.WorldComponentTick();
        }

        public bool pushChargeAmount(float chargeAmount)
        {
            return this.pushChargeAmount(chargeAmount, this.currentEnergy, this.energyCapacity, out this.currentEnergy);
        }
        public bool tryDrainAmount(float drainAmount)
        {
            return  this.tryDrainAmount(drainAmount, this.currentEnergy, out this.currentEnergy);
        }

        public int rareInt = 60; // in tick = third min  1/60 of second min
        public int nextTick;
    }
}
