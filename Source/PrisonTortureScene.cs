//using Cysharp.Threading.Tasks;
//using HarmonyLib;

//namespace SpeedrunCutsceneSkip;

//[HarmonyPatch]
//internal class PrisonTortureScene
//{
//    [HarmonyPostfix, HarmonyPatch(typeof(SimpleCutsceneManager), "PlayAnimation")]
//    private static async void SimpleCutsceneManager_PlayAnimation_Postfix(SimpleCutsceneManager __instance)
//    {
//        if (!SpeedrunCutsceneSkip.Instance.SkipSetting.Value)
//            return;
//        if (__instance.transform.parent.parent.parent.name == "SimpleCutSceneFSM_A5妹妹回憶")
//        {
//            var goPath = FullPath.GetFullPath(__instance.gameObject);
//            if (goPath == "A5_S2/Room/SimpleCutSceneFSM_A5妹妹回憶/FSM Animator/LogicRoot/[CutScene]")
//            {
//                Log.Info($"SpeedrunCutsceneSkip waiting before skipping Prison cutscene");
//                await UniTask.Delay(250);
                
//                Log.Info($"SpeedrunCutsceneSkip skipping Prison Heng cutscene");
//                __instance.TrySkip();
//            }

//            if (goPath == "A5_S2/Room/SimpleCutSceneFSM_A5妹妹回憶/FSM Animator/LogicRoot/[CutScene] Second")
//            {
//                Log.Info($"SpeedrunCutsceneSkip skipping Prison Torture cutscene 1");
//                __instance.TrySkip();
//            }

//            if (goPath == "A5_S2/Room/SimpleCutSceneFSM_A5妹妹回憶/FSM Animator/LogicRoot/[CutScene] Third")
//            {
//                Log.Info($"SpeedrunCutsceneSkip skipping Prison Torture cutscene 2");
//                __instance.TrySkip();
//                SpeedrunCutsceneSkip.AddSkippedTimeToLivesplit(CutsceneTimingConstants.PrisonTortureScene);
//            }
//        }
//    }
//}
