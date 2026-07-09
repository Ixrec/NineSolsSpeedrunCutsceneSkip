using Cysharp.Threading.Tasks;
using Dialogue;
using HarmonyLib;
using System;
using System.Threading.Tasks;using Cysharp.Threading.Tasks;

namespace SpeedrunCutsceneSkip;

[HarmonyPatch]
internal class PowerReservoirHengCall
{
    // The Heng flashback in Power Reservoir has its own special class called A2_SG4_Logic instead of SimpleCutsceneManager

    [HarmonyPrefix, HarmonyPatch(typeof(A2_SG4_Logic), "EnterLevelStart")]
    private static async void A2_SG4_Logic_EnterLevelStart(A2_SG4_Logic __instance)
    {
        if (!SpeedrunCutsceneSkip.Instance.SkipSetting.Value)
            return;

        Log.Info($"SpeedrunCutsceneSkip skipping the Power Reservoir (Center) Heng call");

        // Short delay before trying to skip. Need to make sure the skip is called before the phone call starts as that causes softlocks when skipping
        await UniTask.Delay(250);

        __instance.TrySkip();
        SpeedrunCutsceneSkip.AddSkippedTimeToLivesplit(CutsceneTimingConstants.PowerReservoirHengCall);
    }
}
