using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace BorgAssimilate
{
	public class Designator_Transport : Designator_Zone
	{
		public Designator_Transport()
		{
			this.defaultLabel = "KFM_DesignatorRallyLabel".Translate();
			this.defaultDesc = "KFM_DesignatorRallyDesc".Translate();
			this.soundDragSustain = SoundDefOf.Designate_DragAreaDelete;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_ZoneDelete;
			this.useMouseIcon = true;
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Flag", true);
			this.hotKey = KeyBindingDefOf.Misc4;
		}

		public override void SelectedUpdate()
		{
			base.SelectedUpdate();
			this.drawCircle(UI.MouseCell());
			//IntVec3 rallyPoint = Utils.GCKFM.getRallyPoint(Find.CurrentMap.GetUniqueLoadID());
			//bool flag = rallyPoint.x >= 0;
			//if (flag)
			{
				//this.drawCircle(rallyPoint);
			}
		}

		private void drawCircle(IntVec3 pos)
		{
			DrawUtils.DrawRadiusRingEx(pos, 6f);
		}

		public override AcceptanceReport CanDesignateCell(IntVec3 sq)
		{
			bool flag = !sq.InBounds(base.Map);
			AcceptanceReport result;
			if (flag)
			{
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		public override int DraggableDimensions
		{
			get
			{
				return 0;
			}
		}

		public override bool DragDrawMeasurements
		{
			get
			{
				return false;
			}
		}

		public override void DesignateMultiCell(IEnumerable<IntVec3> cells)
		{
			throw new NotImplementedException();
		}

		public override void DesignateSingleCell(IntVec3 c)
		{
			this.pos = c;
			this.cmap = Current.Game.CurrentMap;
		}

		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			//Utils.GCKFM.setRallyPoint(Find.CurrentMap.GetUniqueLoadID(), this.pos);
			//Utils.cachedRallyRect = new CellRect(-1, -1, 0, 0);
		}

		private IntVec3 pos;

		private Map cmap;
	}
}
