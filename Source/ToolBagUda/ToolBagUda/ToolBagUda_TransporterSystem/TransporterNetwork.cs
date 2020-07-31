using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ToolBagUda
{
    public class TransporterNetwork : WorldComponent
    {

        public TransporterNetwork(World world) : base(world)
        {

        }

        public override void WorldComponentTick()
        {
            base.WorldComponentTick();

        }

        public Map TransporterMap;
        public IntVec3 TransporterPostiion;

        public Map LockMap;
        public IntVec3 LockPosition;

        public List<Pawn> BufferedPatterns = new List<Pawn>();

        public float patternStrength;

        public ThingWithComps transporter;
        public ThingWithComps lockPoint;
        public ThingWithComps transporterPad;
    

    }
}
