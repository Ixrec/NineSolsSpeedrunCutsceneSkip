using BepInEx;
using BepInEx.Configuration;
using Dialogue;
using HarmonyLib;
using NineSolsAPI;
using UnityEngine;

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

        Notifications.Awake();
    }

    public static string dialogueSkipNotificationId = "";
    public static (SimpleCutsceneManager?, string) activeCutscene = (null, "");

    private void SkipActiveCutsceneOrDialogue() {
        var dpgo = GameObject.Find("GameCore(Clone)/RCG LifeCycle/UIManager/GameplayUICamera/Always Canvas/DialoguePlayer(KeepThisEnable)");
        var dp = dpgo?.GetComponent<DialoguePlayer>();

        if (activeCutscene.Item1 != null) {
            var scm = activeCutscene.Item1;
            Log.Info($"calling TrySkip() on {scm.name}");
            AccessTools.Method(typeof(SimpleCutsceneManager), "TrySkip").Invoke(scm, []);
            if (AccessTools.FieldRefAccess<SimpleCutsceneManager, bool>("isMangaPauseing").Invoke(scm)) {
                Log.Info($"also calling Resume() since it was 'manga paused'");
                AccessTools.Method(typeof(SimpleCutsceneManager), "Resume").Invoke(scm, []);
            }
            Notifications.CancelNotification(activeCutscene.Item2);
            activeCutscene = (null, "");
            return;
        }
    }

    private void Update() {
        Notifications.Update();
    }

    private void OnDestroy() {
        // Make sure to clean up resources here to support hot reloading

        harmony.UnpatchSelf();
        Notifications.OnDestroy();
    }
}