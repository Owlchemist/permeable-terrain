using Verse;
using HarmonyLib;
using UnityEngine;
using System;
using System.Collections.Generic;
using static PermeableTerrain.ModSettings_PermeableTerrain;
 
namespace PermeableTerrain
{
    public class Mod_PermeableTerrain : Mod
	{

		public Mod_PermeableTerrain(ModContentPack content) : base(content)
		{
			new Harmony(this.Content.PackageIdPlayerFacing).PatchAll();
			base.GetSettings<ModSettings_PermeableTerrain>();
			if (Prefs.DevMode) LongEventHandler.QueueLongEvent(() => DevReport(), null, false, null);
		}

		void DevReport()
		{
			var terrainDefs = DefDatabase<TerrainDef>.AllDefsListForReading;
			List<string> report = new List<string>();
			foreach (TerrainDef terrainDef in terrainDefs)
			{
				if(terrainDef.HasModExtension<Permeable>()) report.Add(terrainDef.label);
			}
			Log.Message("[Permeable Terrain] The following terrains have been defined as being permeable:\n - " + string.Join("\n - ", report));
		}

		public override void DoSettingsWindowContents(Rect inRect)
		{
			Listing_Standard options = new Listing_Standard();
			options.Begin(inRect);
			options.Label("PermeableTerrain.Modifier".Translate("100%", "25%", "300%") + Math.Round(modifier, 2), -1f, "PermeableTerrain.Modifier.Desc".Translate("10%"));
			modifier = options.Slider(modifier, 0.25f, 3f);
			options.Label("PermeableTerrain.OtherModifier".Translate("100%", "1%", "300%") + Math.Round(otherModifier, 2), -1f, "PermeableTerrain.OtherModifier.Desc".Translate());
			otherModifier = options.Slider(otherModifier, 0.01f, 3f);
			if (Prefs.DevMode) options.CheckboxLabeled("DevMode: Enable logging", ref logging, null);
			options.End();
			base.DoSettingsWindowContents(inRect);
		}

		public override string SettingsCategory()
		{
			return "Permeable Terrain";
		}

		public override void WriteSettings()
		{
			base.WriteSettings();
		}
	}

	public class ModSettings_PermeableTerrain : ModSettings
	{
		public override void ExposeData()
		{
			Scribe_Values.Look<float>(ref modifier, "modifier", 1f, false);
			Scribe_Values.Look<float>(ref otherModifier, "otherModifier", 1f, false);

			base.ExposeData();
		}
		public static float modifier = 1f;
		public static float otherModifier = 1f;
		public static bool logging = false;
	}
}
