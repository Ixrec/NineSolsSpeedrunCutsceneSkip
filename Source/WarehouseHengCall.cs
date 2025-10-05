using Cysharp.Threading.Tasks;
using HarmonyLib;

namespace SpeedrunCutsceneSkip;

[HarmonyPatch]
internal class WarehouseHengCall
{
    [HarmonyPostfix, HarmonyPatch(typeof(SimpleCutsceneManager), "PlayAnimation")]
    private static async void SimpleCutsceneManager_PlayAnimation_Postfix(SimpleCutsceneManager __instance)
    {
        if (!SpeedrunCutsceneSkip.Instance.SkipSetting.Value)
            return;

        if (__instance.transform.parent.parent.parent.name == "妹妹回憶_SimpleCutSceneFSM")
        {
            var goPath = FullPath.GetFullPath(__instance.gameObject);
            if (goPath == "A2_SG4/Room/妹妹回憶_SimpleCutSceneFSM/FSM Animator/LogicRoot/[CutScene]")
            {
                Log.Info($"SpeedrunCutsceneSkip waiting 100 frames before skipping the Warehouse Heng call");

                await UniTask.DelayFrame(100);

                Log.Info($"SpeedrunCutsceneSkip skipping the Warehouse Heng call");
                __instance.TrySkip();
            }
        }
    }
}
