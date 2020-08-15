using System;
using System.Reflection;
using HarmonyLib;
using Verse;
using RimWorld;


namespace RimNauts
{
	[StaticConstructorOnStartup]
	internal class RimNauts : Mod
	{
		public RimNauts(ModContentPack content) : base(content)
		{
			new Harmony("nolabritt.RimNauts").PatchAll(Assembly.GetExecutingAssembly());
		}
	}
}
