using Cysharp.Threading.Tasks;
using HarmonyLib;

namespace SpeedrunCutsceneSkip;

[HarmonyPatch]
internal class CentralHallPhone
{
    [HarmonyPostfix, HarmonyPatch(typeof(SimpleCutsceneManager), "PlayAnimation")]
    private static async void SimpleCutsceneManager_PlayAnimation_Postfix(SimpleCutsceneManager __instance)
    {
        if (!SpeedrunCutsceneSkip.Instance.SkipSetting.Value)
            return;
        if (__instance.transform.parent.parent.parent.name == "SimpleCutSceneFSM_Tree intro")
        {
            var goPath = FullPath.GetFullPath(__instance.gameObject);
            if (goPath == "AG_S1/Room/SimpleCutSceneFSM_Tree intro/FSM Animator/LogicRoot/[CutScene]")
            {
                Log.Info($"SpeedrunCutsceneSkip waiting 1.6 seconds before skipping the Warehouse Heng call");

                await UniTask.Delay(1667);

                Log.Info($"SpeedrunCutsceneSkip skipping the Senate Hall call");
                __instance.TrySkip();

                SpeedrunCutsceneSkip.AddSkippedTimeToLivesplit(CutsceneTimingConstants.CentralHallPhone);
            }
        }
    }
}
