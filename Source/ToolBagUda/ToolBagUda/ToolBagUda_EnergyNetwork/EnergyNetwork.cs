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
    public class EnergyNetwork : WorldComponent
    {

        public EnergyNetwork(World world) : base(world)
        {

        }
        public override void FinalizeInit()
        {
            base.FinalizeInit();
        }
        public override void WorldComponentTick()
        {
            base.WorldComponentTick();

        }

        public bool tryDrainAmount(float drainAmount, float currentEnergy, out float outputAmount)
        {
            if (drainAmount > currentEnergy)
            {
                outputAmount = currentEnergy;
                return false;
            }
            outputAmount = currentEnergy -= drainAmount;
            return true;


        }
        public bool pushChargeAmount(float chargeAmount, float currentEnergy, float energyCapacity, out float outputAmount)
        {
            if (currentEnergy + chargeAmount >= energyCapacity)
            {
                outputAmount = energyCapacity;
                return false;
            }
            outputAmount = currentEnergy + chargeAmount;
            return true;
        }

        public float chargingCurve(float amountStart, float amountEnd, int Dt, float rate)
        {
            float dt = Dt;
            double result = amountStart + (amountEnd * (1 - Math.Exp(-1 * dt / rate)));
            return (float)result;
        }
        public float dischargingCurve(float amountStart, float amountEnd, int Dt, float rate)
        {
            float dt = Dt;
            double result = (amountStart * (Math.Exp(-1 * dt / rate))) + amountEnd;
            return (float)result;
        }

        public float dischargingCurve(float amountStart, int to, int tf, float rate)
        {
            float dt = tf - to;
            double result = amountStart * (Math.Exp(-dt / rate));
            return (float)result;
        }

        public float stepAmount(float naught, float rate, int timestep)
        {
            float result = naught + (rate * (float)timestep);
            return result;
        }

        public int getNextTick(int currentTime, int rareTickInterval)
        {
            int result = currentTime + rareTickInterval;
            return result;
        }


        public float energyCapacity;
        public string energyName;
        public float currentEnergy;

        public List<ThingWithComps> InputDevices;
        public List<ThingWithComps> OutputDevices;

    }
}
