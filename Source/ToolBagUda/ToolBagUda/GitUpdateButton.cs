using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;
using Verse;
using RimWorld;

namespace ToolBagUda
{

	[StaticConstructorOnStartup]
	public static class GitUpdateButton
	{

		static GitUpdateButton()
		{
			Harmony harmony = new Harmony("Nolabritt.SaveOurCrewToo");
			MethodInfo original = AccessTools.Method(typeof(DebugWindowsOpener), "DevToolStarterOnGUI", null, null);
			MethodInfo method = AccessTools.Method(typeof(GitUpdateButton), "Draw", null, null);
			harmony.Patch(original, null, new HarmonyMethod(method), null, null);
		}

		public static void Draw()
		{
			if (Prefs.DevMode)
			{
				Vector2 vector = new Vector2((float)UI.screenWidth * 0.5f + 550f, 3f);
				Find.WindowStack.ImmediateWindow(typeof(GitUpdateButton).GetHashCode(), new Rect(vector.x, vector.y, 24f, 24f).Rounded(), WindowLayer.GameUI, delegate
				{
					if (new WidgetRow(24f, 0f, UIDirection.LeftThenDown, 99999f, 4f).ButtonIcon(GitUpdateButton.UnitTextIcon, "Save Pawn to PCC", null, true))
					{
						char sp = Path.DirectorySeparatorChar;
						string pather = GenFilePaths.ModsFolderPath; Log.Message("Mods:  " + pather);

						string targetDir = Path.Combine(pather, "RimTrek"); Log.Message("target:  " + targetDir);
						string sourceDir = Path.Combine(pather, "Something", "RimTrek"); Log.Message("source:  " + sourceDir);

						FileHandlerUtility util = new FileHandlerUtility();
						util.copyDirToMirror(sourceDir);


						GenCommandLine.Restart();
					}
				}, false, false, 0f);
			}
		}


		


		public static Texture2D UnitTextIcon = ContentFinder<Texture2D>.Get("UI/Icons/SCicon", true);
	}


}
