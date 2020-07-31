using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;
using RimWorld;

namespace BorgAssimilate
{
	// Token: 0x02000C80 RID: 3200
	public class Building_OrganicSubDispenser : Building
	{
		// Token: 0x17000D9E RID: 3486
		// (get) Token: 0x06004CF8 RID: 19704 RVA: 0x0019CCB9 File Offset: 0x0019AEB9
		public bool CanDispenseNow
		{
			get
			{
				return this.powerComp.PowerOn && this.HasEnoughFeedstockInHoppers();
			}
		}

		// Token: 0x17000D9F RID: 3487
		// (get) Token: 0x06004CF9 RID: 19705 RVA: 0x0019CCD0 File Offset: 0x0019AED0
		public List<IntVec3> AdjCellsCardinalInBounds
		{
			get
			{
				if (this.cachedAdjCellsCardinal == null)
				{
					this.cachedAdjCellsCardinal = (from c in GenAdj.CellsAdjacentCardinal(this)
												   where c.InBounds(base.Map)
												   select c).ToList<IntVec3>();
				}
				return this.cachedAdjCellsCardinal;
			}
		}

		// Token: 0x17000DA0 RID: 3488
		// (get) Token: 0x06004CFA RID: 19706 RVA: 0x0019CD02 File Offset: 0x0019AF02
		public virtual ThingDef DispensableDef
		{
			get
			{
				return ThingDefOf.MealNutrientPaste;
			}
		}

		// Token: 0x17000DA1 RID: 3489
		// (get) Token: 0x06004CFB RID: 19707 RVA: 0x0019CD09 File Offset: 0x0019AF09
		public override Color DrawColor
		{
			get
			{
				if (!this.IsSociallyProper(null, false, false))
				{
					return Building_Bed.SheetColorForPrisoner;
				}
				return base.DrawColor;
			}
		}

		// Token: 0x06004CFC RID: 19708 RVA: 0x0019CD22 File Offset: 0x0019AF22
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.powerComp = base.GetComp<CompPowerTrader>();
		}

		// Token: 0x06004CFD RID: 19709 RVA: 0x0019CD38 File Offset: 0x0019AF38
		public virtual Building AdjacentReachableHopper(Pawn reacher)
		{
			for (int i = 0; i < this.AdjCellsCardinalInBounds.Count; i++)
			{
				Building edifice = this.AdjCellsCardinalInBounds[i].GetEdifice(base.Map);
				if (edifice != null && edifice.def == ThingDefOf.Hopper && reacher.CanReach(edifice, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					return edifice;
				}
			}
			return null;
		}

		// Token: 0x06004CFE RID: 19710 RVA: 0x0019CD98 File Offset: 0x0019AF98
		public virtual Thing TryDispenseFood()
		{
			if (!this.CanDispenseNow)
			{
				return null;
			}
			float num = this.def.building.nutritionCostPerDispense - 0.0001f;
			List<ThingDef> list = new List<ThingDef>();
			for (; ; )
			{
				Thing thing = this.FindFeedInAnyHopper();
				if (thing == null)
				{
					break;
				}
				int num2 = Mathf.Min(thing.stackCount, Mathf.CeilToInt(num / thing.GetStatValue(StatDefOf.Nutrition, true)));
				num -= (float)num2 * thing.GetStatValue(StatDefOf.Nutrition, true);
				list.Add(thing.def);
				thing.SplitOff(num2);
				if (num <= 0f)
				{
					goto Block_3;
				}
			}
			Log.Error("Did not find enough food in hoppers while trying to dispense.", false);
			return null;
		Block_3:
			this.def.building.soundDispense.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
			Thing thing2 = ThingMaker.MakeThing(ThingDefOf.MealNutrientPaste, null);
			CompIngredients compIngredients = thing2.TryGetComp<CompIngredients>();
			for (int i = 0; i < list.Count; i++)
			{
				compIngredients.RegisterIngredient(list[i]);
			}
			return thing2;
		}

		// Token: 0x06004CFF RID: 19711 RVA: 0x0019CE98 File Offset: 0x0019B098
		public virtual Thing FindFeedInAnyHopper()
		{
			for (int i = 0; i < this.AdjCellsCardinalInBounds.Count; i++)
			{
				Thing thing = null;
				Thing thing2 = null;
				List<Thing> thingList = this.AdjCellsCardinalInBounds[i].GetThingList(base.Map);
				for (int j = 0; j < thingList.Count; j++)
				{
					Thing thing3 = thingList[j];
					if (Building_OrganicSubDispenser.IsAcceptableFeedstock(thing3.def))
					{
						thing = thing3;
					}
					if (thing3.def == ThingDefOf.Hopper)
					{
						thing2 = thing3;
					}
				}
				if (thing != null && thing2 != null)
				{
					return thing;
				}
			}
			return null;
		}

		// Token: 0x06004D00 RID: 19712 RVA: 0x0019CF24 File Offset: 0x0019B124
		public virtual bool HasEnoughFeedstockInHoppers()
		{
			float num = 0f;
			for (int i = 0; i < this.AdjCellsCardinalInBounds.Count; i++)
			{
				IntVec3 c = this.AdjCellsCardinalInBounds[i];
				Thing thing = null;
				Thing thing2 = null;
				List<Thing> thingList = c.GetThingList(base.Map);
				for (int j = 0; j < thingList.Count; j++)
				{
					Thing thing3 = thingList[j];
					if (Building_OrganicSubDispenser.IsAcceptableFeedstock(thing3.def))
					{
						thing = thing3;
					}
					if (thing3.def == ThingDefOf.Hopper)
					{
						thing2 = thing3;
					}
				}
				if (thing != null && thing2 != null)
				{
					num += (float)thing.stackCount * thing.GetStatValue(StatDefOf.Nutrition, true);
				}
				if (num >= this.def.building.nutritionCostPerDispense)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004D01 RID: 19713 RVA: 0x0019CFE8 File Offset: 0x0019B1E8
		public static bool IsAcceptableFeedstock(ThingDef def)
		{
			return def.IsNutritionGivingIngestible && def.ingestible.preferability != FoodPreferability.Undefined && (def.ingestible.foodType & FoodTypeFlags.Plant) != FoodTypeFlags.Plant && (def.ingestible.foodType & FoodTypeFlags.Tree) != FoodTypeFlags.Tree;
		}

		// Token: 0x06004D02 RID: 19714 RVA: 0x0019D03C File Offset: 0x0019B23C
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(base.GetInspectString());
			if (!this.IsSociallyProper(null, false, false))
			{
				stringBuilder.AppendLine("InPrisonCell".Translate());
			}
			return stringBuilder.ToString().Trim();
		}

		// Token: 0x06004D03 RID: 19715 RVA: 0x00194D04 File Offset: 0x00192F04
		public Building_OrganicSubDispenser()
		{
		}

		// Token: 0x06004D04 RID: 19716 RVA: 0x0019D088 File Offset: 0x0019B288
		// Note: this type is marked as 'beforefieldinit'.
		static Building_OrganicSubDispenser()
		{
		}

		// Token: 0x04002B21 RID: 11041
		public CompPowerTrader powerComp;

		// Token: 0x04002B22 RID: 11042
		private List<IntVec3> cachedAdjCellsCardinal;

		// Token: 0x04002B23 RID: 11043
		public static int CollectDuration = 50;
	}
}
