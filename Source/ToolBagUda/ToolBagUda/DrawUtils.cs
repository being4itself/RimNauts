using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace BorgAssimilate
{
	// Token: 0x0200001C RID: 28
	[StaticConstructorOnStartup]
	internal static class DrawUtils
	{
		// Token: 0x06000068 RID: 104 RVA: 0x00003D8C File Offset: 0x00001F8C
		public static void DrawRadiusRingEx(IntVec3 center, float radius)
		{
			DrawUtils.ringDrawCells.Clear();
			int num = GenRadial.NumCellsInRadius(radius);
			for (int i = 0; i < num; i++)
			{
				DrawUtils.ringDrawCells.Add(center + GenRadial.RadialPattern[i]);
			}
			DrawUtils.DrawFieldEdgesEx(DrawUtils.ringDrawCells);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00003DE5 File Offset: 0x00001FE5
		public static void DrawFieldEdgesEx(List<IntVec3> cells)
		{
			DrawUtils.DrawFieldEdgesEx(cells, Color.white);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00003DE5 File Offset: 0x00001FE5
		public static void DrawFieldEdges(List<IntVec3> cells)
		{
			DrawUtils.DrawFieldEdgesEx(cells, Color.white);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00003DF4 File Offset: 0x00001FF4
		public static void DrawFieldEdgesEx(List<IntVec3> cells, Color color)
		{
			Map currentMap = Find.CurrentMap;
			Material material = MaterialPool.MatFrom(new MaterialRequest
			{
				shader = ShaderDatabase.Transparent,
				color = color,
				BaseTexPath = "UI/Overlays/TargetHL"
			});
			material.GetTexture("_MainTex").wrapMode = TextureWrapMode.Clamp;
			bool flag = DrawUtils.fieldGrid == null;
			if (flag)
			{
				DrawUtils.fieldGrid = new BoolGrid(currentMap);
			}
			else
			{
				DrawUtils.fieldGrid.ClearAndResizeTo(currentMap);
			}
			int x = currentMap.Size.x;
			int z = currentMap.Size.z;
			int count = cells.Count;
			for (int i = 0; i < count; i++)
			{
				bool flag2 = cells[i].InBounds(currentMap);
				if (flag2)
				{
					DrawUtils.fieldGrid[cells[i].x, cells[i].z] = true;
				}
			}
			for (int j = 0; j < count; j++)
			{
				IntVec3 intVec = cells[j];
				bool flag3 = intVec.InBounds(currentMap);
				if (flag3)
				{
					DrawUtils.rotNeeded[0] = (intVec.z < z - 1 && !DrawUtils.fieldGrid[intVec.x, intVec.z + 1]);
					DrawUtils.rotNeeded[1] = (intVec.x < x - 1 && !DrawUtils.fieldGrid[intVec.x + 1, intVec.z]);
					DrawUtils.rotNeeded[2] = (intVec.z > 0 && !DrawUtils.fieldGrid[intVec.x, intVec.z - 1]);
					DrawUtils.rotNeeded[3] = (intVec.x > 0 && !DrawUtils.fieldGrid[intVec.x - 1, intVec.z]);
					for (int k = 0; k < 4; k++)
					{
						bool flag4 = DrawUtils.rotNeeded[k];
						if (flag4)
						{
							Mesh plane = MeshPool.plane10;
							Vector3 position = intVec.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
							Rot4 rot = new Rot4(k);
							Graphics.DrawMesh(plane, position, rot.AsQuat, material, 0);
						}
					}
				}
			}
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00004048 File Offset: 0x00002248
		public static void RenderMouseoverTarget()
		{
			Vector3 position = UI.MouseCell().ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
			//Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, Utils.texAttackTarget, 0);
		}

		// Token: 0x04000024 RID: 36
		private static List<IntVec3> ringDrawCells = new List<IntVec3>();

		// Token: 0x04000025 RID: 37
		private static BoolGrid fieldGrid;

		// Token: 0x04000026 RID: 38
		private static bool[] rotNeeded = new bool[4];
	}
}
