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
    public class Building_BorgSpawner : Building
    {
       
        public BorgSpawner Def
        {
            get
            {
                return this.def as BorgSpawner;
            }
        }
       
        public override void Tick()
        {
            bool flag = Def.currentSpawnCount <= Def.numberToSpawn;
            if (flag)
            {
                float deltaTick = Find.TickManager.TicksGame - Def.startTick;
                //ModCore.Log("Delta Tick triggered");
                if (deltaTick >= Def.spawnInterval)
                {
                    
                    SpawnUtilityBorg SpawnerUtil = new SpawnUtilityBorg();
                    if(SpawnerUtil.spawnBorgPawnX(Def.isPlayer, Def.pawnToSpawn, base.Position, base.Map)) ModCore.Log("pawn made"); ;
                    Def.currentSpawnCount += 1;
                   // ModCore.Log("pawn made");
                    Def.startTick = Find.TickManager.TicksGame;
                    SpawnerUtil = null;
                }
            }
            else if (!flag && Def.destroyAfter)
            {
                //ModCore.Log("destroying");
                base.Destroy(DestroyMode.Vanish);
            }
            
        }
        public override void PostMake()
        {
            //ModCore.Log("Postmake started");
           
            this.startTick = Find.TickManager.TicksGame; 
            Def.setPawnKind();
            Def.calcTickInterval();
            Def.currentSpawnCount = 1; 
            Def.startTick = this.startTick; 

        }


       
        public int startTick;
        
    }
}

/*        
        public BorgSpawner Def
        {
            get
            {
                return this.def as BorgSpawner;
            }
        }
       
        public override void Tick()
        {
            bool flag = Def.currentSpawnCount <= Def.numberToSpawn;
            if (flag)
            {
                float deltaTick = Find.TickManager.TicksGame - Def.startTick;
                //ModCore.Log("Delta Tick triggered");
                if (deltaTick >= Def.spawnInterval)
                {
                    
                    SpawnUtilityBorg SpawnerUtil = new SpawnUtilityBorg();
                    SpawnerUtil.spawnBorgPawn(Def.isPlayer, Def.pawnToSpawn, base.Position, base.Map);
                    Def.currentSpawnCount += 1;
                   // ModCore.Log("pawn made");
                    Def.startTick = Find.TickManager.TicksGame;
                }
            }
            else if (!flag && Def.destroyAfter)
            {
                //ModCore.Log("destroying");
                base.Destroy(DestroyMode.Vanish);
            }
            
        }
        public override void PostMake()
        {
            //ModCore.Log("Postmake started");
           
            this.startTick = Find.TickManager.TicksGame; 
            Def.setPawnKind();
            Def.calcTickInterval();
            Def.currentSpawnCount = 0; 
            Def.startTick = this.startTick; 

        }


       
        public int startTick;
        
    }*/
