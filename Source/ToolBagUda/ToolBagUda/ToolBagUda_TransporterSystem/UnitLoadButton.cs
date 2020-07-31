using System;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using Verse;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using RimWorld;

namespace ToolBagUda
{

	[StaticConstructorOnStartup]
	internal static class UnitLoadButton
	{

		static UnitLoadButton()
		{
			Harmony harmony = new Harmony("Nolabritt.SaveOurCrewToo");
			MethodInfo original = AccessTools.Method(typeof(DebugWindowsOpener), "DevToolStarterOnGUI", null, null);
			MethodInfo method = AccessTools.Method(typeof(UnitLoadButton), "Draw", null, null);
			harmony.Patch(original, null, new HarmonyMethod(method), null, null);
		}

		public static void Draw()
		{
			if (Prefs.DevMode)
			{
				Vector2 vector = new Vector2((float)UI.screenWidth * 0.5f + 325f, 3f);
				Find.WindowStack.ImmediateWindow(typeof(UnitLoadButton).GetHashCode(), new Rect(vector.x, vector.y, 24f, 24f).Rounded(), WindowLayer.GameUI, delegate
				{
					if (new WidgetRow(24f, 0f, UIDirection.LeftThenDown, 99999f, 4f).ButtonIcon(UnitLoadButton.UnitTextIcon, "Load Patterns", null, true))
					{

						char sp = Path.DirectorySeparatorChar;
						string pather = GenFilePaths.ModsFolderPath; Log.Message("Mods:  " + pather);
						string capper = "workshop" + sp + "content" + sp + "294100" + sp + "2180058143" + sp + "Defs" + sp + "Misc" + sp + "TrekData" + sp;
						string[] patherParts = pather.Split(Path.DirectorySeparatorChar);
						string cap = patherParts[0];
						cap = cap + '/';
						patherParts[0] = cap;

						int numParts = patherParts.Count() - 1;
						Array.Resize(ref patherParts, numParts - 2);
						string file2 = Path.Combine(Path.Combine(patherParts), capper + "Saved_Crew" + ".rwship");

						Log.Message("Signal path set:  " + file2);

						TransporterNetwork transNet = Find.World.GetComponent<TransporterNetwork>();
						List<Pawn> loadedPawns = new List<Pawn>();
						Log.Message("Transporter network link established. Patterns:  " + loadedPawns.Count.ToString());
						Scribe.loader.InitLoading(file2);

						Scribe_Collections.Look<Pawn>(ref loadedPawns, "patterns", LookMode.Deep, Array.Empty<object>());

						Scribe.loader.FinalizeLoading();

						Log.Message("Pattern recovery finished. Patterns:  " + loadedPawns.Count.ToString());


						foreach(Pawn pattern in loadedPawns)
                        {
							transNet.BufferedPatterns.Add(pattern);
						}


						Log.Message("Pattern sync to buffer. Patterns:  " + transNet.BufferedPatterns.Count.ToString());
					}
				}, false, false, 0f);
			}
		}


		public static Texture2D UnitTextIcon = ContentFinder<Texture2D>.Get("UI/Icons/STicon", true);
	}
}
