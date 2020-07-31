using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;
using RimWorld;

namespace BorgAssimilate
{
	// Token: 0x02000CB8 RID: 3256
	[StaticConstructorOnStartup]
	public class ShieldBelt_Borg : Apparel
	{
		// Token: 0x17000DF7 RID: 3575
		// (get) Token: 0x06004EE6 RID: 20198 RVA: 0x001A8FB5 File Offset: 0x001A71B5
		private float EnergyMax
		{
			get
			{
				return this.GetStatValue(StatDefOf.EnergyShieldEnergyMax, true);
			}
		}

		// Token: 0x17000DF8 RID: 3576
		// (get) Token: 0x06004EE7 RID: 20199 RVA: 0x001A8FC3 File Offset: 0x001A71C3
		private float EnergyGainPerTick
		{
			get
			{
				return this.GetStatValue(StatDefOf.EnergyShieldRechargeRate, true) / 60f;
			}
		}

		// Token: 0x17000DF9 RID: 3577
		// (get) Token: 0x06004EE8 RID: 20200 RVA: 0x001A8FD7 File Offset: 0x001A71D7
		public float Energy
		{
			get
			{
				return this.energy;
			}
		}

		// Token: 0x17000DFA RID: 3578
		// (get) Token: 0x06004EE9 RID: 20201 RVA: 0x001A8FDF File Offset: 0x001A71DF
		public ShieldState ShieldState
		{
			get
			{
				if (this.ticksToReset > 0)
				{
					return ShieldState.Resetting;
				}
				return ShieldState.Active;
			}
		}

		// Token: 0x17000DFB RID: 3579
		// (get) Token: 0x06004EEA RID: 20202 RVA: 0x001A8FF0 File Offset: 0x001A71F0
		private bool ShouldDisplay
		{
			get
			{
				Pawn wearer = base.Wearer;
				return wearer.Spawned && !wearer.Dead && !wearer.Downed && (wearer.InAggroMentalState || wearer.Drafted || (wearer.Faction.HostileTo(Faction.OfPlayer) && !wearer.IsPrisoner) || Find.TickManager.TicksGame < this.lastKeepDisplayTick + this.KeepDisplayingTicks);
			}
		}

		// Token: 0x06004EEB RID: 20203 RVA: 0x001A906C File Offset: 0x001A726C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.energy, "energy", 0f, false);
			Scribe_Values.Look<int>(ref this.ticksToReset, "ticksToReset", -1, false);
			Scribe_Values.Look<int>(ref this.lastKeepDisplayTick, "lastKeepDisplayTick", 0, false);
		}

		// Token: 0x06004EEC RID: 20204 RVA: 0x001A90B9 File Offset: 0x001A72B9
		public override IEnumerable<Gizmo> GetWornGizmos()
		{
			if (Find.Selector.SingleSelectedThing == base.Wearer)
			{
				yield return new Gizmo_EnergyShieldStatus_Borg
				{
					shield = this
				};
			}
			yield break;
		}

		// Token: 0x06004EED RID: 20205 RVA: 0x001A90C9 File Offset: 0x001A72C9
		public override float GetSpecialApparelScoreOffset()
		{
			return this.EnergyMax * this.ApparelScorePerEnergyMax;
		}

		// Token: 0x06004EEE RID: 20206 RVA: 0x001A90D8 File Offset: 0x001A72D8
		public override void Tick()
		{
			base.Tick();
			if (base.Wearer == null)
			{
				this.energy = 0f;
				return;
			}
			if (this.ShieldState == ShieldState.Resetting)
			{
				this.ticksToReset--;
				if (this.ticksToReset <= 0)
				{
					this.Reset();
					return;
				}
			}
			else if (this.ShieldState == ShieldState.Active)
			{
				this.energy += this.EnergyGainPerTick;
				if (this.energy > this.EnergyMax)
				{
					this.energy = this.EnergyMax;
				}
			}
		}

		// Token: 0x06004EEF RID: 20207 RVA: 0x001A915C File Offset: 0x001A735C


		public override bool CheckPreAbsorbDamage(DamageInfo dinfo)
		{
			if (this.ShieldState != ShieldState.Active)
			{
				return false;
			}
			if (dinfo.Def == DamageDefOf.EMP)
			{
				this.energy = 0f;
				this.Break();
				return false;
			}
			if (dinfo.Def.isRanged || dinfo.Def.isExplosive)
			{
				if (dinfo.Def == this.LastDamage)
				{
					this.energy -= dinfo.Amount * this.EnergyLossPerDamage;
					//ModCore.Log("1");
				}
				else
				{
					this.LastDamage = dinfo.Def;
					return false;
					//ModCore.Log("0");
				}
				this.LastDamage = dinfo.Def;
				if (this.energy < 0f)
				{
					this.Break();
				}
				else
				{
					this.AbsorbedDamage(dinfo);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06004EF0 RID: 20208 RVA: 0x001A91EA File Offset: 0x001A73EA
		public void KeepDisplaying()
		{
			this.lastKeepDisplayTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06004EF1 RID: 20209 RVA: 0x001A91FC File Offset: 0x001A73FC
		private void AbsorbedDamage(DamageInfo dinfo)
		{
			SoundDefOf.EnergyShield_AbsorbDamage.PlayOneShot(new TargetInfo(base.Wearer.Position, base.Wearer.Map, false));
			this.impactAngleVect = Vector3Utility.HorizontalVectorFromAngle(dinfo.Angle);
			Vector3 loc = base.Wearer.TrueCenter() + this.impactAngleVect.RotatedBy(180f) * 0.5f;
			float num = Mathf.Min(10f, 2f + dinfo.Amount / 10f);
			MoteMaker.MakeStaticMote(loc, base.Wearer.Map, ThingDefOf.Mote_ExplosionFlash, num);
			int num2 = (int)num;
			for (int i = 0; i < num2; i++)
			{
				MoteMaker.ThrowDustPuff(loc, base.Wearer.Map, Rand.Range(0.8f, 1.2f));
			}
			this.lastAbsorbDamageTick = Find.TickManager.TicksGame;
			this.KeepDisplaying();
		}

		// Token: 0x06004EF2 RID: 20210 RVA: 0x001A92EC File Offset: 0x001A74EC
		private void Break()
		{
			SoundDefOf.EnergyShield_Broken.PlayOneShot(new TargetInfo(base.Wearer.Position, base.Wearer.Map, false));
			MoteMaker.MakeStaticMote(base.Wearer.TrueCenter(), base.Wearer.Map, ThingDefOf.Mote_ExplosionFlash, 12f);
			for (int i = 0; i < 6; i++)
			{
				MoteMaker.ThrowDustPuff(base.Wearer.TrueCenter() + Vector3Utility.HorizontalVectorFromAngle((float)Rand.Range(0, 360)) * Rand.Range(0.3f, 0.6f), base.Wearer.Map, Rand.Range(0.8f, 1.2f));
			}
			this.energy = 0f;
			this.ticksToReset = this.StartingTicksToReset;
		}

		// Token: 0x06004EF3 RID: 20211 RVA: 0x001A93C4 File Offset: 0x001A75C4
		private void Reset()
		{
			if (base.Wearer.Spawned)
			{
				SoundDefOf.EnergyShield_Reset.PlayOneShot(new TargetInfo(base.Wearer.Position, base.Wearer.Map, false));
				MoteMaker.ThrowLightningGlow(base.Wearer.TrueCenter(), base.Wearer.Map, 3f);
			}
			this.ticksToReset = -1;
			this.energy = this.EnergyOnReset;
		}

		// Token: 0x06004EF4 RID: 20212 RVA: 0x001A943C File Offset: 0x001A763C
		public override void DrawWornExtras()
		{
			if (this.ShieldState == ShieldState.Active && this.ShouldDisplay)
			{
				float num = Mathf.Lerp(1.2f, 1.55f, this.energy);
				Vector3 vector = base.Wearer.Drawer.DrawPos;
				vector.y = AltitudeLayer.MoteOverhead.AltitudeFor();
				int num2 = Find.TickManager.TicksGame - this.lastAbsorbDamageTick;
				if (num2 < 8)
				{
					float num3 = (float)(8 - num2) / 8f * 0.05f;
					vector += this.impactAngleVect * num3;
					num -= num3;
				}
				float angle = (float)Rand.Range(0, 360);
				Vector3 s = new Vector3(num, 1f, num);
				Matrix4x4 matrix = default(Matrix4x4);
				matrix.SetTRS(vector, Quaternion.AngleAxis(angle, Vector3.up), s);
				Graphics.DrawMesh(MeshPool.plane10, matrix, ShieldBelt_Borg.BubbleMat, 0);
			}
		}

		public override bool AllowVerbCast(IntVec3 root, Map map, LocalTargetInfo targ, Verb verb)
		{
			return true;
		}

		public ShieldBelt_Borg()
		{
		}

		static ShieldBelt_Borg()
		{
		}
		public DamageDef LastDamage = null;
		// Token: 0x04002C49 RID: 11337
		private float energy;

		// Token: 0x04002C4A RID: 11338
		private int ticksToReset = -1;

		// Token: 0x04002C4B RID: 11339
		private int lastKeepDisplayTick = -9999;

		// Token: 0x04002C4C RID: 11340
		private Vector3 impactAngleVect;

		// Token: 0x04002C4D RID: 11341
		private int lastAbsorbDamageTick = -9999;

		// Token: 0x04002C4E RID: 11342
		private const float MinDrawSize = 1.2f;

		// Token: 0x04002C4F RID: 11343
		private const float MaxDrawSize = 1.55f;

		// Token: 0x04002C50 RID: 11344
		private const float MaxDamagedJitterDist = 0.05f;

		// Token: 0x04002C51 RID: 11345
		private const int JitterDurationTicks = 8;

		// Token: 0x04002C52 RID: 11346
		private int StartingTicksToReset = 3200;

		// Token: 0x04002C53 RID: 11347
		private float EnergyOnReset = 0.2f;

		// Token: 0x04002C54 RID: 11348
		private float EnergyLossPerDamage = 0.033f;

		// Token: 0x04002C55 RID: 11349
		private int KeepDisplayingTicks = 1000;

		// Token: 0x04002C56 RID: 11350
		private float ApparelScorePerEnergyMax = 0.25f;

		// Token: 0x04002C57 RID: 11351
		private static readonly Material BubbleMat = MaterialPool.MatFrom("Other/ShieldBubble", ShaderDatabase.Transparent);
	}
}
