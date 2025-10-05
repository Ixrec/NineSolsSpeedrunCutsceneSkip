using HarmonyLib;

namespace SpeedrunCutsceneSkip;

[HarmonyPatch]
internal class PowerReservoirHengCall
{
    // The Heng flashback in Power Reservoir got its own special implementation class instead of using SimpleCutsceneManager
    [HarmonyPrefix, HarmonyPatch(typeof(A2_SG4_Logic), "EnterLevelStart")]
    private static void A2_SG4_Logic_EnterLevelStart(A2_SG4_Logic __instance)
    {
        if (SpeedrunCutsceneSkip.Instance.SkipSetting.Value)
        {
            Log.Info($"SpeedrunCutsceneSkip skipping the Power Reservoir (Center) Heng call");
            __instance.TrySkip();
        }
    }
}
