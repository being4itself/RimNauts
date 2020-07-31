using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000005 RID: 5
	public class Building_TransporterPad : Building
	{
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.outputname = Faction.OfPlayer.Name;
		}

		public void Rename()
		{
			Find.WindowStack.Add(new Building_TransporterPad.Dialog_Rename_Pad(this));
		}

		public void activate()
		{
			Log.Message("Ship Saving Active: " + this.Active.ToString());
			this.Active = !this.Active;

		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo c in base.GetGizmos())
			{
				yield return c;
			}
			IEnumerator<Gizmo> enumerator = null;
			yield return new Command_Action
			{
				defaultLabel = "Name of file",
				icon = ContentFinder<Texture2D>.Get("UI/Buttons/Rename", true),
				defaultDesc = this.outputname,
				action = new Action(this.Rename)
			};
			if (Prefs.DevMode)
			{

				yield return new Command_Action
				{
					defaultLabel = "Activate",
					icon = ContentFinder<Texture2D>.Get("UI/Buttons/Rename", true),
					defaultDesc = this.Active.ToString(),
					action = new Action(this.activate)
				};
			}
			yield break;
		}

		public string outputname = "Saved_Pawns";
		public bool Active = false;


		public class Dialog_Rename_Pad : Dialog_Rename
		{
			public Dialog_Rename_Pad(Building_TransporterPad core)
			{
				this.transporterConsole = core;
				this.curName = core.outputname;
			}

			public override Vector2 InitialSize
			{
				get
				{
					return new Vector2(500f, 175f);
				}
			}

			protected override void SetName(string name)
			{
				this.transporterConsole.outputname = this.curName;
			}

			private Building_TransporterPad transporterConsole;
		}
	}
}
