using InGameWiki;
using System.Linq;
using UnityEngine;
using Verse;

namespace BorgAssimilate
{
    public class ModCore : Mod
    {
        public static ModCore Instance { get; private set; }

        public ModWiki Wiki { get; internal set; }

        public ModCore(ModContentPack content) : base(content)
        {
            Instance = this;
            Log("We are Borg!");

            Trace("Resistance is futile");


        }




        public static void Log(string msg)
        {
            Verse.Log.Message(msg ?? "<null>");
        }

        public static void Trace(string msg)
        {
            Verse.Log.Message(msg ?? "<null>");
        }

  
    }
}