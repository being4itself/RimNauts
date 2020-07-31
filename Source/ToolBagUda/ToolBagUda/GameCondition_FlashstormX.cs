using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020009B7 RID: 2487
	public class GameCondition_FlashstormX : GameCondition
	{
		// Token: 0x17000AAD RID: 2733
		// (get) Token: 0x06003B50 RID: 15184 RVA: 0x0013994E File Offset: 0x00137B4E
		public int AreaRadius
		{
			get
			{
				return this.areaRadius;
			}
		}

		// Token: 0x06003B51 RID: 15185 RVA: 0x00139958 File Offset: 0x00137B58
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec2>(ref this.centerLocation, "centerLocation", default(IntVec2), false);
			Scribe_Values.Look<int>(ref this.areaRadius, "areaRadius", 0, false);
			Scribe_Values.Look<IntRange>(ref this.areaRadiusOverride, "areaRadiusOverride", default(IntRange), false);
			Scribe_Values.Look<int>(ref this.nextLightningTicks, "nextLightningTicks", 0, false);
			Scribe_Values.Look<IntRange>(ref this.initialStrikeDelay, "initialStrikeDelay", default(IntRange), false);
			Scribe_Values.Look<bool>(ref this.ambientSound, "ambientSound", false, false);
		}

		// Token: 0x06003B52 RID: 15186 RVA: 0x001399F0 File Offset: 0x00137BF0
		public override void Init()
		{
			base.Init();
			this.areaRadius = ((this.areaRadiusOverride == IntRange.zero) ? GameCondition_Flashstorm.AreaRadiusRange.RandomInRange : this.areaRadiusOverride.RandomInRange);
			this.nextLightningTicks = Find.TickManager.TicksGame + this.initialStrikeDelay.RandomInRange;
			if (this.centerLocation.IsInvalid)
			{
				this.FindGoodCenterLocation();
			}
		}

		// Token: 0x06003B53 RID: 15187 RVA: 0x00139A64 File Offset: 0x00137C64
		public override void GameConditionTick()
		{
			if (Find.TickManager.TicksGame > this.nextLightningTicks)
			{
				Vector2 vector = Rand.UnitVector2 * Rand.Range(0f, (float)this.areaRadius);
				IntVec3 intVec = new IntVec3((int)Math.Round((double)vector.x) + this.centerLocation.x, 0, (int)Math.Round((double)vector.y) + this.centerLocation.z);
				if (this.IsGoodLocationForStrike(intVec))
				{
					//base.SingleMap.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrike(base.SingleMap, intVec));
					//base.SingleMap.listerBuildings.Add()
					this.nextLightningTicks = Find.TickManager.TicksGame + GameCondition_Flashstorm.TicksBetweenStrikes.RandomInRange;
				}
			}
			if (this.ambientSound)
			{
				if (this.soundSustainer == null || this.soundSustainer.Ended)
				{
					this.soundSustainer = SoundDefOf.FlashstormAmbience.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(this.centerLocation.ToIntVec3, base.SingleMap, false), MaintenanceType.PerTick));
					return;
				}
				this.soundSustainer.Maintain();
			}
		}

		// Token: 0x06003B54 RID: 15188 RVA: 0x00139B7A File Offset: 0x00137D7A
		public override void End()
		{
			base.SingleMap.weatherDecider.DisableRainFor(30000);
			base.End();
		}

		// Token: 0x06003B55 RID: 15189 RVA: 0x00139B98 File Offset: 0x00137D98
		private void FindGoodCenterLocation()
		{
			if (base.SingleMap.Size.x <= 16 || base.SingleMap.Size.z <= 16)
			{
				throw new Exception("Map too small for flashstorm.");
			}
			for (int i = 0; i < 10; i++)
			{
				this.centerLocation = new IntVec2(Rand.Range(8, base.SingleMap.Size.x - 8), Rand.Range(8, base.SingleMap.Size.z - 8));
				if (this.IsGoodCenterLocation(this.centerLocation))
				{
					break;
				}
			}
		}

		// Token: 0x06003B56 RID: 15190 RVA: 0x00139C2E File Offset: 0x00137E2E
		private bool IsGoodLocationForStrike(IntVec3 loc)
		{
			return loc.InBounds(base.SingleMap) && !loc.Roofed(base.SingleMap) && loc.Standable(base.SingleMap);
		}

		// Token: 0x06003B57 RID: 15191 RVA: 0x00139C5C File Offset: 0x00137E5C
		private bool IsGoodCenterLocation(IntVec2 loc)
		{
			int num = 0;
			int num2 = (int)(3.14159274f * (float)this.areaRadius * (float)this.areaRadius / 2f);
			foreach (IntVec3 loc2 in this.GetPotentiallyAffectedCells(loc))
			{
				if (this.IsGoodLocationForStrike(loc2))
				{
					num++;
				}
				if (num >= num2)
				{
					break;
				}
			}
			return num >= num2;
		}

		// Token: 0x06003B58 RID: 15192 RVA: 0x00139CDC File Offset: 0x00137EDC
		private IEnumerable<IntVec3> GetPotentiallyAffectedCells(IntVec2 center)
		{
			int num;
			for (int x = center.x - this.areaRadius; x <= center.x + this.areaRadius; x = num)
			{
				for (int z = center.z - this.areaRadius; z <= center.z + this.areaRadius; z = num)
				{
					if ((center.x - x) * (center.x - x) + (center.z - z) * (center.z - z) <= this.areaRadius * this.areaRadius)
					{
						yield return new IntVec3(x, 0, z);
					}
					num = z + 1;
				}
				num = x + 1;
			}
			yield break;
		}

		// Token: 0x04002311 RID: 8977
		private static readonly IntRange AreaRadiusRange = new IntRange(45, 60);

		// Token: 0x04002312 RID: 8978
		private static readonly IntRange TicksBetweenStrikes = new IntRange(320, 800);

		// Token: 0x04002313 RID: 8979
		private const int RainDisableTicksAfterConditionEnds = 30000;

		// Token: 0x04002314 RID: 8980
		public IntVec2 centerLocation = IntVec2.Invalid;

		// Token: 0x04002315 RID: 8981
		public IntRange areaRadiusOverride = IntRange.zero;

		// Token: 0x04002316 RID: 8982
		public IntRange initialStrikeDelay = IntRange.zero;

		// Token: 0x04002317 RID: 8983
		public bool ambientSound;

		// Token: 0x04002318 RID: 8984
		private int areaRadius;

		// Token: 0x04002319 RID: 8985
		private int nextLightningTicks;

		// Token: 0x0400231A RID: 8986
		private Sustainer soundSustainer;
	}
}
