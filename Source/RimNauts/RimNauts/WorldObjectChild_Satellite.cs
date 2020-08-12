using System;
using System.Linq;
using System.Reflection;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace ThatsAMoon
{
    // Token: 0x02000003 RID: 3
    [StaticConstructorOnStartup]
    public class WorldObjectChild_Satellite : MapParent
    {
        // Token: 0x17000001 RID: 1
        // (get) Token: 0x06000002 RID: 2 RVA: 0x000020F0 File Offset: 0x000002F0
        public override Vector3 DrawPos
        {
            get
            {
                return this.calcParametricEllipse(this.maxOrbits, this.shiftOrbits, this.period, this.timeOffset);
            }
        }

        // Token: 0x06000003 RID: 3 RVA: 0x00002120 File Offset: 0x00000320
        public override void PostAdd()
        {
            this.maxOrbits = this.randomizeVect(this.satDef.getMaxOrbits);
            this.shiftOrbits = this.randomizeVect(this.satDef.getShiftOrbits);
            this.period = this.satDef.getOrbitPeriod + (float)((double)(Rand.Value - 0.5f) * ((double)this.satDef.getOrbitPeriod * 0.25));
            this.timeOffset = Rand.Range(0, (int)(this.period / 2f));
            base.PostAdd();
        }

        // Token: 0x06000004 RID: 4 RVA: 0x000021B4 File Offset: 0x000003B4
        public Vector3 randomizeVect(Vector3 oldVector)
        {
            return new Vector3
            {
                x = (float)(Rand.Bool ? 1 : -1) * oldVector.x + (float)((double)(Rand.Value - 0.5f) * ((double)oldVector.x * 0.25)),
                y = (float)(Rand.Bool ? 1 : -1) * oldVector.y + (float)((double)(Rand.Value - 0.5f) * ((double)oldVector.y * 0.25)),
                z = (float)(Rand.Bool ? 1 : -1) * oldVector.z + (float)((double)(Rand.Value - 0.5f) * ((double)oldVector.z * 0.25))
            };
        }

        // Token: 0x06000005 RID: 5 RVA: 0x00002280 File Offset: 0x00000480
        public Vector3 calcParametricEllipse(Vector3 max, Vector3 shift, float Period, int timeOffset)
        {
            Vector3 result = default(Vector3);
            int ticksGame = Find.TickManager.TicksGame;
            result.x = max.x * (float)Math.Cos((double)(6.28f / Period * (float)(ticksGame + timeOffset))) + shift.x;
            result.z = max.z * (float)Math.Sin((double)(6.28f / Period * (float)(ticksGame + timeOffset))) + shift.z;
            result.y = max.y * (float)Math.Cos((double)(6.28f / Period * (float)(ticksGame + timeOffset))) + shift.y;
            return result;
        }

        // Token: 0x06000006 RID: 6 RVA: 0x00002324 File Offset: 0x00000524
        internal static object GetInstanceField(Type type, object instance, string fieldName)
        {
            BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            return type.GetField(fieldName, bindingAttr).GetValue(instance);
        }

        // Token: 0x06000007 RID: 7 RVA: 0x00002348 File Offset: 0x00000548
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<float>(ref this.period, "period", 0f, false);
            Scribe_Values.Look<int>(ref this.timeOffset, "timeOffset", 0, false);
            Scribe_Values.Look<Vector3>(ref this.maxOrbits, "maxOrbits", default(Vector3), false);
            Scribe_Values.Look<Vector3>(ref this.shiftOrbits, "shiftOrbits", default(Vector3), false);
            WorldObjectChild_Satellite.GetInstanceField(typeof(WorldObject), this, "BaseDrawSize");
        }

        // Token: 0x06000008 RID: 8 RVA: 0x000023D3 File Offset: 0x000005D3
        public override void Tick()
        {
            base.Tick();
        }

        // Token: 0x06000009 RID: 9 RVA: 0x000023E0 File Offset: 0x000005E0
        public override void Print(LayerSubMesh subMesh)
        {
            float averageTileSize = Find.WorldGrid.averageTileSize;
            WorldRendererUtility.PrintQuadTangentialToPlanet(this.DrawPos, 10.7f * averageTileSize, 0.008f, subMesh, false, false, true);
        }

        // Token: 0x0600000A RID: 10 RVA: 0x00002418 File Offset: 0x00000618
        public override void Draw()
        {
            float averageTileSize = Find.WorldGrid.averageTileSize;
            float transitionPct = ExpandableWorldObjectsUtility.TransitionPct;
            bool flag = this.def.expandingIcon && transitionPct > 0f;
            if (flag)
            {
                MaterialPropertyBlock materialPropertyBlock = WorldObjectChild_Satellite.propertyBlock;
                WorldRendererUtility.DrawQuadTangentialToPlanet(this.DrawPos, 10f * averageTileSize, 0.008f, this.Material, false, false, null);
            }
            else
            {
                WorldRendererUtility.DrawQuadTangentialToPlanet(this.DrawPos, 10f * averageTileSize, 0.008f, this.Material, false, false, null);
            }
        }

        // Token: 0x0600000B RID: 11 RVA: 0x000024A0 File Offset: 0x000006A0
        public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
        {
            alsoRemoveWorldObject = true;
            bool flag = Enumerable.Count<WorldObject>(Enumerable.Where<WorldObject>(Find.World.worldObjects.AllWorldObjects, (WorldObject ob) => ob is TravelingTransportPods && ((int)typeof(TravelingTransportPods).GetField("initialTile", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(ob) == base.Tile || ((TravelingTransportPods)ob).destinationTile == base.Tile))) > 0;
            return !flag && base.ShouldRemoveMapNow(out alsoRemoveWorldObject);
        }

        // Token: 0x0600000C RID: 12 RVA: 0x000024ED File Offset: 0x000006ED
        public override void PostRemove()
        {
            Current.Game.GetComponent<Gamecomp_SatellitesInOrbit>().resetSatellite();
            base.PostRemove();
        }

        // Token: 0x04000001 RID: 1
        private SatelliteDef satDef = DefDatabase<SatelliteDef>.GetNamed("SatelliteCore", true);

        // Token: 0x04000002 RID: 2
        public Vector3 maxOrbits;

        // Token: 0x04000003 RID: 3
        public Vector3 shiftOrbits;

        // Token: 0x04000004 RID: 4
        public float period;

        // Token: 0x04000005 RID: 5
        private static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

        // Token: 0x04000006 RID: 6
        public int timeOffset = 0;
    }
}
