using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace ThatsAMoon
{
    // Token: 0x02000006 RID: 6
    internal class SatelliteDef : Def
    {
        // Token: 0x17000003 RID: 3
        // (get) Token: 0x06000018 RID: 24 RVA: 0x00003174 File Offset: 0x00001374
        public List<string> getWorldObjectDefNames
        {
            get
            {
                return this.WorldObjectDefNames;
            }
        }

        // Token: 0x17000004 RID: 4
        // (get) Token: 0x06000019 RID: 25 RVA: 0x0000318C File Offset: 0x0000138C
        public Vector3 getOrbitVectorBase
        {
            get
            {
                return this.orbitVectorBase.ToVector3();
            }
        }

        // Token: 0x17000005 RID: 5
        // (get) Token: 0x0600001A RID: 26 RVA: 0x000031AC File Offset: 0x000013AC
        public Vector3 getOrbitVectorRange
        {
            get
            {
                return this.orbitVectorRange.ToVector3();
            }
        }

        // Token: 0x17000006 RID: 6
        // (get) Token: 0x0600001B RID: 27 RVA: 0x000031CC File Offset: 0x000013CC
        public float getOrbitPeriod
        {
            get
            {
                return this.orbitPeriod;
            }
        }

        // Token: 0x17000007 RID: 7
        // (get) Token: 0x0600001C RID: 28 RVA: 0x000031E4 File Offset: 0x000013E4
        public float getOrbitPeriodVar
        {
            get
            {
                return this.orbitPeriodVar;
            }
        }

        // Token: 0x17000008 RID: 8
        // (get) Token: 0x0600001D RID: 29 RVA: 0x000031FC File Offset: 0x000013FC
        public Vector3 getMaxOrbits
        {
            get
            {
                return this.maxOrbits.ToVector3();
            }
        }

        // Token: 0x17000009 RID: 9
        // (get) Token: 0x0600001E RID: 30 RVA: 0x0000321C File Offset: 0x0000141C
        public Vector3 getShiftOrbits
        {
            get
            {
                return this.shiftOrbits.ToVector3();
            }
        }

        // Token: 0x04000007 RID: 7
        public List<string> WorldObjectDefNames = new List<string>
        {
            "RockMoon",
            "GasMoon",
            "BrokenMoon",
            "AsteroidSatellite"
        };

        // Token: 0x04000008 RID: 8
        public IntVec3 orbitVectorBase = new IntVec3(200, 40, 0);

        // Token: 0x04000009 RID: 9
        public IntVec3 orbitVectorRange = new IntVec3(150, 10, 0);

        // Token: 0x0400000A RID: 10
        public float orbitPeriod = 36000f;

        // Token: 0x0400000B RID: 11
        public float orbitPeriodVar = 6000f;

        // Token: 0x0400000C RID: 12
        public IntVec3 maxOrbits = new IntVec3(400, 50, 200);

        // Token: 0x0400000D RID: 13
        public IntVec3 shiftOrbits = new IntVec3(20, 20, 20);
    }
}
