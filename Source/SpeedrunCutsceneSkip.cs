using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using NineSolsAPI;

namespace SpeedrunCutsceneSkip;

[BepInDependency(NineSolsAPICore.PluginGUID)]
[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class SpeedrunCutsceneSkip : BaseUnityPlugin {
    // https://docs.bepinex.dev/articles/dev_guide/plugin_tutorial/4_configuration.html
    public ConfigEntry<bool> SkipSetting = null!;

    public static SpeedrunCutsceneSkip Instance = null!;

    private Harmony harmony = null!;

    private void Awake() {
        Log.Init(Logger);
        RCGLifeCycle.DontDestroyForever(gameObject);
        Instance = this;

        // Load patches from any class annotated with @HarmonyPatch
        harmony = Harmony.CreateAndPatchAll(typeof(SpeedrunCutsceneSkip).Assembly);

        SkipSetting = Config.Bind("", "Skip Certain Cutscenes", true,
            "Turn this off and this mod will no longer do anything.");

        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }

    private void OnDestroy() {
        // Make sure to clean up resources here to support hot reloading

        harmony.UnpatchSelf();
    }
}