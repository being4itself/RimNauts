using InGameWiki;
using Verse;

namespace BorgAssimilate
{
    [StaticConstructorOnStartup]
    internal static class Wiki
    {
        static Wiki()
        {
            // Get a reference to your mod instance.
            Mod myMod = ModCore.Instance;

            // Create and register a new wiki.
            var wiki = ModWiki.Create(myMod);

            // Change some wiki properties.
            wiki.WikiTitle = "Borg Race Revisited";
        }
    }
}