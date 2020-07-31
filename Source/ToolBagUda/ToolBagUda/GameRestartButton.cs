using System;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace BorgAssimilate
{

	[StaticConstructorOnStartup]
	public static class GameRestartButton
	{

		static GameRestartButton()
		{
			Harmony harmony = new Harmony("Nolabritt.SaveOurCrewToo");
			MethodInfo original = AccessTools.Method(typeof(DebugWindowsOpener), "DevToolStarterOnGUI", null, null);
			MethodInfo method = AccessTools.Method(typeof(GameRestartButton), "Draw", null, null);
			harmony.Patch(original, null, new HarmonyMethod(method), null, null);
		}

		public static void Draw()
		{
			if (Prefs.DevMode)
			{
				Vector2 vector = new Vector2((float)UI.screenWidth * 0.5f + 250f, 3f);
				Find.WindowStack.ImmediateWindow(typeof(GameRestartButton).GetHashCode(), new Rect(vector.x, vector.y, 24f, 24f).Rounded(), WindowLayer.GameUI, delegate
				{
					if (new WidgetRow(24f, 0f, UIDirection.LeftThenDown, 99999f, 4f).ButtonIcon(GameRestartButton.UnitTextIcon, "Save Pawn to PCC", null, true))
					{
						GenCommandLine.Restart();
					}
				}, false, false, 0f);
			}
		}


		public static Texture2D UnitTextIcon = ContentFinder<Texture2D>.Get("UI/Icons/SCicon", true);
	}
}
