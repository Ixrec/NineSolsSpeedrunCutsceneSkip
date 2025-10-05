using Cysharp.Threading.Tasks;
using Dialogue;
using HarmonyLib;
using System;

namespace SpeedrunCutsceneSkip;

[HarmonyPatch]
internal class EDSanctumHengCall
{
    // For some reason we have to wait until the first dialogue starts before TrySkip() will work,
    // so we end up with two patches: one to record the A2_SG4_Logic instance, and one to actually skip it.
    private static SimpleCutsceneManager? activeSanctumCall = null;

    [HarmonyPostfix, HarmonyPatch(typeof(SimpleCutsceneManager), "PlayAnimation")]
    private static async void SimpleCutsceneManager_PlayAnimation_Postfix(SimpleCutsceneManager __instance)
    {
        if (SpeedrunCutsceneSkip.Instance.SkipSetting.Value)
        {
            if (__instance.transform.parent.parent.parent.name == "SimpleCutSceneFSM_A9_S3妹妹回憶")
            {
                var goPath = FullPath.GetFullPath(__instance.gameObject);
                if (goPath == "A9_S3/Room/SimpleCutSceneFSM_A9_S3妹妹回憶/FSM Animator/LogicRoot/[CutScene]")
                {
                    Log.Info($"SpeedrunCutsceneSkip waiting for dialogue to start before skipping the ED Sanctum Heng call");
                    activeSanctumCall = __instance;
                }
            }
        }
    }

    [HarmonyPrefix, HarmonyPatch(typeof(DialoguePlayer), "StartDialogue")]
    private static async void DialoguePlayer_StartDialogue(DialoguePlayer __instance, DialogueGraph dialogueGraph, Action callback, bool withBackground)
    {
        if (SpeedrunCutsceneSkip.Instance.SkipSetting.Value)
        {
            if (activeSanctumCall != null)
            {
                Log.Info($"SpeedrunCutsceneSkip waiting 100 frames before skipping the ED Sanctum Heng call");

                await UniTask.DelayFrame(100);

                Log.Info($"SpeedrunCutsceneSkip skipping the ED Sanctum Heng call");
                __instance.TrySkip();
                activeSanctumCall.TrySkip();
                activeSanctumCall = null;
            }
        }
    }
}
