using System;
using UnityEngine;
using Verse;
using RimWorld;

namespace BorgAssimilate
{
	// Token: 0x02000CB6 RID: 3254
	[StaticConstructorOnStartup]
	public class Gizmo_EnergyShieldStatus_Borg : Gizmo
	{
		// Token: 0x06004EE2 RID: 20194 RVA: 0x001A8E38 File Offset: 0x001A7038
		public Gizmo_EnergyShieldStatus_Borg()
		{
			this.order = -100f;
		}

		// Token: 0x06004EE3 RID: 20195 RVA: 0x001A8E4B File Offset: 0x001A704B
		public override float GetWidth(float maxWidth)
		{
			return 140f;
		}

		// Token: 0x06004EE4 RID: 20196 RVA: 0x001A8E54 File Offset: 0x001A7054
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth)
		{
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			Rect rect2 = rect.ContractedBy(6f);
			Widgets.DrawWindowBackground(rect);
			Rect rect3 = rect2;
			rect3.height = rect.height / 2f;
			Text.Font = GameFont.Tiny;
			Widgets.Label(rect3, this.shield.LabelCap);
			Rect rect4 = rect2;
			rect4.yMin = rect2.y + rect2.height / 2f;
			float fillPercent = this.shield.Energy / Mathf.Max(1f, this.shield.GetStatValue(StatDefOf.EnergyShieldEnergyMax, true));
			Widgets.FillableBar(rect4, fillPercent, Gizmo_EnergyShieldStatus_Borg.FullShieldBarTex, Gizmo_EnergyShieldStatus_Borg.EmptyShieldBarTex, false);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect4, (this.shield.Energy * 100f).ToString("F0") + " / " + (this.shield.GetStatValue(StatDefOf.EnergyShieldEnergyMax, true) * 100f).ToString("F0"));
			Text.Anchor = TextAnchor.UpperLeft;
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x04002C43 RID: 11331
		public ShieldBelt_Borg shield;

		// Token: 0x04002C44 RID: 11332
		private static readonly Texture2D FullShieldBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.2f, 0.2f, 0.24f));

		// Token: 0x04002C45 RID: 11333
		private static readonly Texture2D EmptyShieldBarTex = SolidColorMaterials.NewSolidColorTexture(Color.clear);
	}
}
