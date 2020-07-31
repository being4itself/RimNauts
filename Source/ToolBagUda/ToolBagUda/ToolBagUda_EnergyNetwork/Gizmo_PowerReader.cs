using System;
using UnityEngine;
using Verse;

namespace ToolBagUda
{
	[StaticConstructorOnStartup]
	public class Gizmo_PowerReader : Gizmo
	{
		public Gizmo_PowerReader()
		{
			this.order = -100f;
		}

		public override float GetWidth(float maxWidth)
		{
			return 140f;
		}
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth)
		{
			Rect overRect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			Find.WindowStack.ImmediateWindow(1523289473, overRect, WindowLayer.GameUI, delegate
			{
				Rect rect2;
				Rect rect = rect2 = overRect.AtZero().ContractedBy(6f);
				rect2.height = overRect.height * (5f / 12f);
				Text.Font = GameFont.Tiny;
				Widgets.Label(rect2, this.engine.Props.gizmoLabel);

				Rect rect3 = rect;
				rect3.yMin = overRect.height * (8f / 12f);
				rect3.yMax = overRect.height * (6f / 12f);
				float fillPercent = engine.getPowerNetAmount(false) / engine.EMPlasmaCapacity;
				Widgets.FillableBar(rect3, fillPercent, Gizmo_PowerReader.FullBarTex, Gizmo_PowerReader.EmptyBarTex, false);

				Rect rect4 = rect;
				rect4.yMax = overRect.height *(9f/ 12f);
				rect4.yMin = overRect.height * (11f/ 12f);
				float fillPercent2 = engine.getPowerNetAmount(true) / engine.WarpPlasmaCapacity;
				Widgets.FillableBar(rect4, fillPercent2, Gizmo_PowerReader.FullBarTexW, Gizmo_PowerReader.EmptyBarTex, false);

				if (false)
				{
					float num = fillPercent;
					float x = rect3.x + num * rect3.width - (float)Gizmo_PowerReader.TargetLevelArrow.width * 0.5f / 2f;
					float y = rect3.y - (float)Gizmo_PowerReader.TargetLevelArrow.height * 0.5f;
					GUI.DrawTexture(new Rect(x, y, (float)Gizmo_PowerReader.TargetLevelArrow.width * 0.5f, (float)Gizmo_PowerReader.TargetLevelArrow.height * 0.5f), Gizmo_PowerReader.TargetLevelArrow);
				}
				Text.Font = GameFont.Small;
				Text.Anchor = TextAnchor.MiddleCenter;
				Widgets.Label(rect3, engine.getPowerNetAmount(false).ToString() );
				Text.Anchor = TextAnchor.UpperLeft;
			}, true, false, 1f);
			return new GizmoResult(GizmoState.Clear);
		}

		public Comp_PDS engine;

		public ElectroPlasmaNet energyNet;
		private static readonly Texture2D FullBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.35f, 0.55f, 0.2f));
		private static readonly Texture2D FullBarTexW = SolidColorMaterials.NewSolidColorTexture(new Color(0.55f, 0.35f, 0.2f));
		private static readonly Texture2D EmptyBarTex = SolidColorMaterials.NewSolidColorTexture(Color.black);
		private static readonly Texture2D TargetLevelArrow = ContentFinder<Texture2D>.Get("UI/Misc/BarInstantMarkerRotated", true);
		private const float ArrowScale = 0.5f;
	}
}
