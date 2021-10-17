using HarmonyLib;
using Verse;
using RimWorld;
using static PermeableTerrain.ModSettings_PermeableTerrain;

namespace PermeableTerrain
{   
    [HarmonyPatch (typeof(Filth), nameof(Filth.SpawnSetup))]
    static class Patch_SpawnSetup
    {
        static void Postfix(ref Filth __instance, bool respawningAfterLoad)
        {
            if (respawningAfterLoad) return;
            
            int before = __instance.disappearAfterTicks;

			Map map = __instance.Map;
            TerrainDef terrainDef = map.terrainGrid.TerrainAt(__instance.Position);

            if (__instance.def.HasModExtension<Liquidy>() && terrainDef.HasModExtension<Permeable>())
            {
                __instance.disappearAfterTicks = (int)(__instance.disappearAfterTicks * terrainDef.GetModExtension<Permeable>().value * modifier);
            }  
            else __instance.disappearAfterTicks = (int)(__instance.disappearAfterTicks * otherModifier);

            if (logging) Log.Message("[Permeable Terrain] " + __instance.def.defName + " before: " + before / 60000 + " days | After: " + + __instance.disappearAfterTicks / 60000 + " days");
        }
    }
}