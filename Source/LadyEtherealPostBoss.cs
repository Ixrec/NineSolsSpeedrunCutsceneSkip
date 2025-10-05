using HarmonyLib;

namespace SpeedrunCutsceneSkip;

[HarmonyPatch]
internal class LadyEtherealPostBoss
{
    [HarmonyPrefix, HarmonyPatch(typeof(VideoPlayAction), "OnStateEnterImplement")]
    private static void VideoPlayAction_OnStateEnterImplement(VideoPlayAction __instance)
    {
        if (__instance.name == "[Action] VideoPlayAction")
        {
            var goPath = FullPath.GetFullPath(__instance.gameObject);
            if (goPath == "GameLevel/Room/Prefab/SimpleCutSceneFSM_結局_大爆炸/--[States]/FSM/[State] PlayCutSceneEnd/[Action] VideoPlayAction")
            {
                if (SpeedrunCutsceneSkip.Instance.SkipSetting.Value)
                {
                    Log.Info($"SpeedrunCutsceneSkip skipping the post-Lady Ethereal Heng flashback video");
                    AccessTools.Method(typeof(VideoPlayAction), "TrySkip").Invoke(__instance, []);
                }
            }
        }
    }
}
