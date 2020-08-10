using System;
using System.Reflection;
using HarmonyLib;
using Verse;

namespace ThatsAMoon
{
    // Token: 0x02000009 RID: 9
    [StaticConstructorOnStartup]
    internal class ThatsAMoon : Mod
    {
        // Token: 0x06000025 RID: 37 RVA: 0x00003390 File Offset: 0x00001590
        public ThatsAMoon(ModContentPack content) : base(content)
        {
            new Harmony("nolabritt.ThatsAMoon").PatchAll(Assembly.GetExecutingAssembly());
        }

        // Token: 0x06000026 RID: 38 RVA: 0x000033B0 File Offset: 0x000015B0
        public void Save()
        {
        }
    }
}
