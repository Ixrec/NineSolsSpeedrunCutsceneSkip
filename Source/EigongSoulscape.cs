using Cysharp.Threading.Tasks;
using HarmonyLib;

namespace SpeedrunCutsceneSkip;

[HarmonyPatch]
internal class EigongSoulscape
{
    [HarmonyPostfix, HarmonyPatch(typeof(SimpleCutsceneManager), "PlayAnimation")]
    private static async void SimpleCutsceneManager_PlayAnimation_Postfix(SimpleCutsceneManager __instance)
    {
        if (!SpeedrunCutsceneSkip.Instance.SkipSetting.Value)
            return;

        var _goPath = FullPath.GetFullPath(__instance.gameObject);
        Log.Info($"SpeedrunCutsceneSkip eigong check {__instance.name} - {_goPath}");

        if (__instance.transform.parent.parent.parent.name == "SimpleCutSceneFSM_易公陷阱")
        {
            var goPath = FullPath.GetFullPath(__instance.gameObject);
            if (goPath == "VR_TaoChang/Room/Prefab/Simple Binding Tool/SimpleCutSceneFSM_易公陷阱/FSM Animator/LogicRoot/[CutScene]")
            {
                Log.Info($"SpeedrunCutsceneSkip skipping the Eigong confrontation inside her soulscape");
                __instance.TrySkip();
            }
        }

        if (__instance.transform.parent.parent.parent.name == "SimpleCutSceneFSM_易公後妹妹回憶")
        {
            var goPath = FullPath.GetFullPath(__instance.gameObject);
            if (goPath == "VR_TaoChang/Room/SimpleCutSceneFSM_易公後妹妹回憶/FSM Animator/LogicRoot/[CutScene]")
            {
                Log.Info($"SpeedrunCutsceneSkip waiting 100 frames before skipping the Heng flashback inside of Eigong's soulscape");

                await UniTask.DelayFrame(100);

                Log.Info($"SpeedrunCutsceneSkip skipping the Heng flashback inside of Eigong's soulscape");
                __instance.TrySkip();
            }
        }

        if (__instance.transform.parent.parent.parent.name == "[CutScene]BackFromSleeppod")
        {
            var goPath = FullPath.GetFullPath(__instance.gameObject);
            if (goPath == "A11_S2/Room/敘事相關/Sleeppod  FSM_易公/[CutScene]BackFromSleeppod/FSM Animator/LogicRoot/[CutScene]")
            {
                Log.Info($"SpeedrunCutsceneSkip skipping the wakeup cutscene after Eigong's soulscape");
                __instance.TrySkip();
            }
        }
    }
}
