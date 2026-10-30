
using BepInEx;
using HarmonyLib;

namespace CleanStacks
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    internal class CleanStacks : BaseUnityPlugin
    {
        public const string PluginGUID = "com.jotunn.CleanStacks";
        public const string PluginName = "CleanStacks";
        public const string PluginVersion = "1.1.0";

        private Harmony harmony;

        private void Awake()
        {
            Logger.LogInfo("CleanStacks has landed");

            harmony = new Harmony(PluginGUID);
            harmony.PatchAll();

            Logger.LogInfo(
                "CleanStacks: stack display patches loaded.");
        }

        private void OnDestroy()
        {
            harmony?.UnpatchSelf();
        }
    }
}
