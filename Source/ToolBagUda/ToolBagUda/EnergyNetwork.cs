using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace BorgAssimilate
{
    public class PlasmaEnergy : WorldComponent
    {

        public PlasmaEnergy(World world) : base(world)
        {

        }
        public override void FinalizeInit()
        {
            this.nextTick = getNextTick(Find.TickManager.TicksGame);
            base.FinalizeInit();

            

        }
        public override void WorldComponentTick()
        {
            base.WorldComponentTick();

            if (Find.TickManager.TicksGame >= this.nextTick)
            {
                if (this.EMPlasmaAmount > this.EMPlasmaAmountFinal)
                {
                    Log.Message("discharging" + this.EMPlasmaAmountFinal.ToString());
                    this.EMPlasmaAmount = dischargingCurve(this.EMPlasmaAmount, this.EMPlasmaAmountFinal, this.rareInt, this.EMPlasmaChangeRate);
                    this.WarpPlasmaAmount = dischargingCurve(this.WarpPlasmaAmount, this.EMPlasmaAmountFinal, this.rareInt, this.WarpPlasmaChangeRate);
                }
                else if (this.EMPlasmaAmountFinal > this.EMPlasmaAmount)
                {

                    Log.Message("charging... " + this.EMPlasmaAmountFinal.ToString());
                    this.EMPlasmaAmount = chargingCurve(this.EMPlasmaAmount, this.EMPlasmaAmountFinal, this.rareInt, this.EMPlasmaChangeRate);

                    this.WarpPlasmaAmount = chargingCurve(this.EMPlasmaAmount, this.WarpPlasmaAmountFinal, this.rareInt, this.WarpPlasmaChangeRate);
                }

                Log.Message("Net tick energy:" + this.EMPlasmaAmount.ToString() + "   "  + this.EMPlasmaAmountFinal.ToString());
                this.nextTick = getNextTick(Find.TickManager.TicksGame);
            }
           
        }

        public bool tryDrainAmount(float amount)
        {
            if (amount > this.EMPlasmaAmountFinal)
            {
                return false;
            }
            this.EMPlasmaAmountFinal -= amount;
            return true;
           

        }
        public void pushChargeAmount(float amount)
        {
            if (this.EMPlasmaAmountFinal + amount >= this.EMPlasmaCapacity)
            {
                this.EMPlasmaAmountFinal = this.EMPlasmaCapacity; Log.Message("Net is full:  " + this.EMPlasmaAmountFinal.ToString());
            }
            this.EMPlasmaAmountFinal  += amount;
            


        }

        public float chargingCurve(float amountStart, float amountEnd, int Dt, float rate)
        {
            float dt = Dt;
            double result = amountStart + (amountEnd * (1-Math.Exp(-1 * dt / rate)));
            return (float)result;
        }
        public float dischargingCurve(float amountStart, float amountEnd, int Dt, float rate)
        {
            float dt = Dt;
            double result = (amountStart * (Math.Exp(-1*dt / rate))) + amountEnd;
            return (float)result;
        }

        public float dischargingCurve(float amountStart, int to, int tf, float rate)
        {
            float dt = tf-to;
            double result = amountStart * (Math.Exp(-dt / rate));
            return (float)result;
        }

        public float stepAmount(float naught, float rate , int timestep)
        {
            float result =  naught + (rate * (float)timestep);
            return result;
        }

        public int getNextTick(int currentTime)
        {
            int result = currentTime +this.rareInt;
            return result;
        }


        public float EMPlasmaCapacity = 1000000;
        public float WarpPlasmaCapacity = 1000000;

        public float EMPlasmaChangeRate = 1;
        public float WarpPlasmaChangeRate = 1;


        public float EMPlasmaAmount=0;
        public float WarpPlasmaAmount=0;      // in TJ

        public float EMPlasmaAmountFinal;   // in TJ
        public float WarpPlasmaAmountFinal;

        public int rareInt = 60; // in tick = third min  1/60 of second min
        public int nextTick; 

        public ThingWithComps Reactor;
        public ThingWithComps PDS;



    }
}
