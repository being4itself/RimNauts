using System;
using RimWorld;
using RimWorld.Planet;

namespace ThatsAMoon
{
    // Token: 0x02000008 RID: 8
    public class BiomeWorker_Satellite : BiomeWorker
    {
        // Token: 0x06000023 RID: 35 RVA: 0x00003370 File Offset: 0x00001570
        public override float GetScore(Tile tile, int tileID)
        {
            return -999f;
        }
    }
}