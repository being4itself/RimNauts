using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using UnityEngine;

namespace BorgAssimilate
{
    public class BorgSpawner : ThingDef
    {

        public void setPawnKind()
        {
            ModCore.Log(pawnKindName); ModCore.Log(numberToSpawn.ToString()); ModCore.Log(spawnIntervalInSeconds.ToString()); 
            this.pawnToSpawn = PawnKindDef.Named(this.pawnKindName);// ModCore.Log("1.75");// pawnKindName);
        }
         
        public void calcTickInterval()
        {
            this.spawnInterval = this.spawnIntervalInSeconds * 60;
            ModCore.Log(spawnInterval.ToString());
        }

        public String   pawnKindName;
        public int      numberToSpawn;
        public int      spawnIntervalInSeconds;
        public int      spawnInterval;
        public bool     isPlayer;
        public bool     destroyAfter;

        public int currentSpawnCount =0;
        public PawnKindDef pawnToSpawn = null;
        public int startTick = 0;
    }



}

/*        public void setPawnKind()
        {
            ModCore.Log(pawnKindName); ModCore.Log(numberToSpawn.ToString()); ModCore.Log(spawnIntervalInSeconds.ToString()); 
            this.pawnToSpawn = PawnKindDef.Named(this.pawnKindName); ModCore.Log("1.75");// pawnKindName);
        }
         
        public void calcTickInterval()
        {
            this.spawnInterval = this.spawnIntervalInSeconds * 60;
            ModCore.Log(spawnInterval.ToString());
        }

        public String   pawnKindName;
        public int      numberToSpawn;
        public int      spawnIntervalInSeconds;
        public int      spawnInterval;
        public bool     isPlayer;
        public bool     destroyAfter;

        public int currentSpawnCount =0;
        public PawnKindDef pawnToSpawn = null;
        public int startTick = 0; */
