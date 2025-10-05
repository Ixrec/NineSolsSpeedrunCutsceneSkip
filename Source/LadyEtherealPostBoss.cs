using Cysharp.Threading.Tasks;
using HarmonyLib;

namespace SpeedrunCutsceneSkip;

[HarmonyPatch]
internal class LadyEtherealPostBoss
{
    [HarmonyPostfix, HarmonyPatch(typeof(SimpleCutsceneManager), "PlayAnimation")]
    private static void SimpleCutsceneManager_PlayAnimation_Postfix(SimpleCutsceneManager __instance)
    {
        if (SpeedrunCutsceneSkip.Instance.SkipSetting.Value)
        {
            if (__instance.transform.parent.parent.parent.name == "SimpleCutSceneFSM (離開幻境)")
            {
                var goPath = FullPath.GetFullPath(__instance.gameObject);
                if (goPath == "P2_R22_Savepoint_GameLevel/EventBinder/SimpleCutSceneFSM (離開幻境)/FSM Animator/LogicRoot/[CutScene]")
                {
                    Log.Info($"SpeedrunCutsceneSkip skipping Lady Ethereal's post-fight cutscene");
                    __instance.TrySkip();
                }
            }

            if (__instance.transform.parent.parent.parent.name == "A7_S6_Cutscene FSM")
            {
                var goPath = FullPath.GetFullPath(__instance.gameObject);
                if (goPath == "A7_S6_Memory_Butterfly_CutScene_GameLevel/A7_S6_Cutscene FSM/FSM Animator/LogicRoot/[CutScene]")
                {
                    Log.Info($"SpeedrunCutsceneSkip skipping the Azure Flyer flashback cutscene");
                    __instance.TrySkip();
                }
            }
        }
    }

    [HarmonyPrefix, HarmonyPatch(typeof(VideoPlayAction), "OnStateEnterImplement")]
    private static async void VideoPlayAction_OnStateEnterImplement(VideoPlayAction __instance)
    {
        if (__instance.name == "[Action] VideoPlayAction")
        {
            var goPath = FullPath.GetFullPath(__instance.gameObject);
            if (goPath == "A7_S6_Memory_Butterfly_CutScene_GameLevel/A7_S6_Cutscene FSM/--[States]/FSM/[State] PlayingVideo/[Action] VideoPlayAction")
            {
                if (SpeedrunCutsceneSkip.Instance.SkipSetting.Value)
                {
                    Log.Info($"SpeedrunCutsceneSkip waiting 100 frames before skipping the post-Lady Ethereal Heng flashback video");

                    await UniTask.DelayFrame(100);

                    Log.Info($"SpeedrunCutsceneSkip skipping the post-Lady Ethereal Heng flashback video");
                    AccessTools.Method(typeof(VideoPlayAction), "TrySkip").Invoke(__instance, []);
                }
            }
        }
    }
}
