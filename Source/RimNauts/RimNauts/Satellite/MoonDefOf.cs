using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RimNauts
{
    [DefOf]
    public static class MoonDefOf
    {
        public static BiomeDef RockMoonBiome;

        static MoonDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(MoonDefOf));
        }
    }
}
