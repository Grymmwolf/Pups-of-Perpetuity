// PupsOfPerpetuity V1.1
// Goal: Pets still get hungry and take normal damage, but NEVER starve to death.
// Implementation: 
//   1) Always report IsStarving = false.
//   2) Skip Pet._OnHungerChanged(), which is where starvation logic is triggered.

using BepInEx;
using BepInEx.Unity.IL2CPP;
using BepInEx.Logging;
using HarmonyLib;
using SSSGame;

namespace Grymms.PupsOfPerpetuity
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class PupsOfPerpetuityPlugin : BasePlugin
    {
        public const string PluginGuid = "com.grymm.askamods.pups_of_perpetuity";
        public const string PluginName = "Pups of Perpetuity (Anti-Starve V1)";
        public const string PluginVersion = "1.0.0";

        internal static Harmony Harmony;
        internal static ManualLogSource L;

        public override void Load()
        {
            L = Log;
            Harmony = new Harmony(PluginGuid);

            Harmony.PatchAll(typeof(Pet_IsStarving_SafetyPatch));
            Harmony.PatchAll(typeof(Pet_OnHungerChanged_BlockPatch));

            L.LogInfo("[PoP V1] Anti-Starve loaded.");
        }
    }

    [HarmonyPatch(typeof(Pet), "get_IsStarving")]
    static class Pet_IsStarving_SafetyPatch
    {
        static void Postfix(ref bool __result)
        {
            __result = false;
        }
    }

    [HarmonyPatch(typeof(Pet), "_OnHungerChanged")]
    static class Pet_OnHungerChanged_BlockPatch
    {
        static bool Prefix(Pet __instance)
        {
            return false;
        }
    }
}
