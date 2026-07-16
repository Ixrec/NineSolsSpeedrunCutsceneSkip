using Cysharp.Threading.Tasks;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SpeedrunCutsceneSkip;

[HarmonyPatch]
internal class YanlaoPreFight
{
    // The Yanlao fight has a special implementation class not covered by our SimpleCutsceneManager patches
    [HarmonyPrefix, HarmonyPatch(typeof(A4_S5_Logic), "EnterLevelStart")]
    private static async void A4_S5_Logic_Start(A4_S5_Logic __instance)
    {
        if (!SpeedrunCutsceneSkip.Instance.SkipSetting.Value)
            return;

        if (__instance.FirstTimeAnimationPlayed.CurrentValue)
        {
            return;
        }

        await UniTask.Delay(1000);

        Log.Info($"SpeedrunCutsceneSkip skipping the Yanlao Pre-Fight Scene");

        __instance.BeforeMangaBubble.TrySkip();
        __instance.StartCutscene.TrySkip();
        __instance.BubbleDialogue.TrySkip();

        __instance.MechClawGamePlay.gameObject.SetActive(value: true);
        __instance.GianMechClawMonsterBase.gameObject.SetActive(value: true);
        __instance.SwapClaw();
        __instance.StartCutsceneEmergency.PlayCutscene(delegate
        {
            // Use AccessTools to call StartFight as it's a private method
            var privateMethod = AccessTools.Method(typeof(A4_S5_Logic), "StartFight", new Type[] { });
            // 2. Invoke it (pass null for the first argument if it is a static method)
            privateMethod.Invoke(__instance, new object[] { });
        });

        SpeedrunCutsceneSkip.AddSkippedTimeToLivesplit(CutsceneTimingConstants.YanlaoBossIntro);
    }
}

