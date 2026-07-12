using Cysharp.Threading.Tasks;
using HarmonyLib;

namespace SpeedrunCutsceneSkip;

[HarmonyPatch]
internal class PavillionNymphIntro
{
    [HarmonyPostfix, HarmonyPatch(typeof(SimpleCutsceneManager), "PlayAnimation")]
    private static async void SimpleCutsceneManager_PlayAnimation_Postfix(SimpleCutsceneManager __instance)
    {
        if (!SpeedrunCutsceneSkip.Instance.SkipSetting.Value)
            return;

        if (__instance.transform.parent.parent.parent.parent.name == "NPC_AICore_FSM")
        {
            var goPath = FullPath.GetFullPath(__instance.gameObject);
            if (goPath == "AG_S2/Room/Prefab/ControlRoom FSM Binding Tool/NPC_AICore_Base/NPC_AICore_Base_FSM/FSM Animator/LogicRoot/NPC_AICore_FSM/General FSM Object/Animator(FSM)/LogicRoot/[CutScene]初次對話演出")
            {
                Log.Info($"SpeedrunCutsceneSkip waiting 1.6 seconds before skipping the Ruyi activation cutscene");

                await UniTask.Delay(1667);

                Log.Info($"SpeedrunCutsceneSkip skipping the Ruyi restore sequence");
                __instance.TrySkip();

                SpeedrunCutsceneSkip.AddSkippedTimeToLivesplit(CutsceneTimingConstants.PavillionNymphIntro_RuyiActivation);
            }
        }
        if (__instance.transform.parent.parent.parent.name == "SimpleCutSceneFSM_軒軒到基地演出")
        {
            var goPath = FullPath.GetFullPath(__instance.gameObject);
            if (goPath == "AG_S2/Room/NPCs/議會演出相關Binding/SimpleCutSceneFSM_軒軒到基地演出/FSM Animator/LogicRoot/[CutScene]")
            {
                Log.Info($"SpeedrunCutsceneSkip waiting 1.6 seconds before skipping the Shuanshuan intro dialogue");

                await UniTask.Delay(1667);

                Log.Info($"SpeedrunCutsceneSkip skipping the Shuanshuan dialogue");
                __instance.TrySkip();

                SpeedrunCutsceneSkip.AddSkippedTimeToLivesplit(CutsceneTimingConstants.PavillionNymphIntro_ShuanshuanDialogue);
            }
        }
    }
}
