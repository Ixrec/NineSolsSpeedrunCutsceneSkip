using Dialogue;
using HarmonyLib;
using System;

namespace SpeedrunCutsceneSkip;

[HarmonyPatch]
internal class PowerReservoirHengCall
{
    // The Heng flashback in Power Reservoir has its own special class called A2_SG4_Logic instead of SimpleCutsceneManager

    // For some reason we have to wait until the first dialogue starts before TrySkip() will work,
    // so we end up with two patches: one to record the A2_SG4_Logic instance, and one to actually skip it.
    private static A2_SG4_Logic? activeA2SG4 = null;

    [HarmonyPrefix, HarmonyPatch(typeof(A2_SG4_Logic), "EnterLevelStart")]
    private static void A2_SG4_Logic_EnterLevelStart(A2_SG4_Logic __instance)
    {
        if (SpeedrunCutsceneSkip.Instance.SkipSetting.Value)
        {
            Log.Info($"SpeedrunCutsceneSkip waiting for dialogue to start before skipping the Power Reservoir (Center) Heng call");
            activeA2SG4 = __instance;
        }
    }

    [HarmonyPrefix, HarmonyPatch(typeof(DialoguePlayer), "StartDialogue")]
    private static void DialoguePlayer_StartDialogue(DialoguePlayer __instance, DialogueGraph dialogueGraph, Action callback, bool withBackground)
    {
        if (SpeedrunCutsceneSkip.Instance.SkipSetting.Value)
        {
            if (dialogueGraph.gameObject.name == "妹妹來電1_Dialogue" && activeA2SG4 != null)
            {
                var graphGoPath = FullPath.GetFullPath(dialogueGraph.gameObject);
                Log.Debug($"DialoguePlayer_StartDialogue for dialogueGraph {graphGoPath}");

                if (graphGoPath == "A2_SG4/Logic/PhoneActingLoigic(Part1)/妹妹來電1_Dialogue")
                {
                    Log.Info($"SpeedrunCutsceneSkip skipping the Power Reservoir (Center) Heng call");
                    activeA2SG4!.TrySkip();
                    activeA2SG4 = null;
                }
            }
        }
    }
}
