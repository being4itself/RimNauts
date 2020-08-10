using System;
using UnityEngine;
using Verse;

namespace ThatsAMoon
{
    // Token: 0x0200000F RID: 15
    public class WorldGenStep_Moons : WorldGenStep
    {
        // Token: 0x1700000B RID: 11
        // (get) Token: 0x0600003C RID: 60 RVA: 0x00003C9C File Offset: 0x00001E9C
        public override int SeedPart
        {
            get
            {
                return 13371488;
            }
        }

        // Token: 0x0600003D RID: 61 RVA: 0x00003CB4 File Offset: 0x00001EB4
        public override void GenerateFresh(string seed)
        {
            for (int i = 0; i <= Rand.Range(40, 80); i++)
            {
                Current.Game.GetComponent<Gamecomp_SatellitesInOrbit>().tryGenSatellite(0, new Vector3(this.randomOrbit(-40f, 10f), 0f, this.randomOrbit(-150f, 40f)), new Vector3(this.randomOrbit(40f, 10f), 0f, this.randomOrbit(150f, 40f)), (int)this.randomOrbit(6000f, 600f));
            }
        }

        // Token: 0x0600003E RID: 62 RVA: 0x000033B0 File Offset: 0x000015B0
        public override void GenerateFromScribe(string seed)
        {
        }

        // Token: 0x0600003F RID: 63 RVA: 0x00003D58 File Offset: 0x00001F58
        public float randomOrbit(float min, float range)
        {
            float num = min * (float)(Rand.Bool ? 1 : -1);
            return min + range * (Rand.Value - 0.5f);
        }

        // Token: 0x06000040 RID: 64 RVA: 0x00003D8C File Offset: 0x00001F8C
        public float calculateDistanceAcceleration(float distance, float angleSpeed, float GravConst, float mass)
        {
            return distance * angleSpeed * angleSpeed - GravConst * mass / distance * distance;
        }

        // Token: 0x06000041 RID: 65 RVA: 0x00003DAC File Offset: 0x00001FAC
        public float calculateAngleAcceleration(float distanceSpeed, float angleSpeed, float distance)
        {
            return -2f * distanceSpeed * angleSpeed / distance;
        }

        // Token: 0x06000042 RID: 66 RVA: 0x00003DCC File Offset: 0x00001FCC
        public float newValue(float currentValue, float deltaT, float derivative)
        {
            return currentValue + deltaT * derivative;
        }

        // Token: 0x06000043 RID: 67 RVA: 0x00003DE3 File Offset: 0x00001FE3
        public void convertCart(float radius, float theta, Vector3 cart)
        {
            cart.x = radius * (float)Math.Cos((double)theta);
            cart.y = radius * (float)Math.Sin((double)theta);
        }

        // Token: 0x06000044 RID: 68 RVA: 0x00003E08 File Offset: 0x00002008
        public void orbital()
        {
            float num = 1.496E+11f;
            float num2 = 1.9909866E-07f;
            float gravConst = 6.674E-11f;
            float mass = 1000f;
            float num3 = 0f;
            float deltaT = 0.015f;
            double num4 = 0.5235987755982988;
            float derivative = this.calculateDistanceAcceleration(num, num2, gravConst, mass);
            num3 = this.newValue(num3, deltaT, derivative);
            num = this.newValue(num, deltaT, num3);
            float derivative2 = this.calculateAngleAcceleration(num3, num2, num);
            num2 = this.newValue(num2, deltaT, derivative2);
            num4 = (double)this.newValue((float)num4, deltaT, num2);
        }
    }
}
