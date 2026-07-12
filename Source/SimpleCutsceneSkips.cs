using Cysharp.Threading.Tasks;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SpeedrunCutsceneSkip;

struct SimpleCutsceneSkipData
{
    // Delay to apply before skipping the cutscene (in ms)
    public int delayTime; 

    //Time to add to livesplit when the cutscene is skipped (in s)
    public float livesplitAddedTime;
};

static class SimpleCutsceneSkipDataTable
{
    public static readonly Dictionary<string, SimpleCutsceneSkipData> Table = new Dictionary<string, SimpleCutsceneSkipData>() 
    {
        // Boundless repository entrance
        { "A4_S5/Room/TaoChangSniper_SimpleCutSceneFSM/FSM Animator/LogicRoot/[CutScene]",  new SimpleCutsceneSkipData(){  delayTime = 1667, livesplitAddedTime = CutsceneTimingConstants.YanlaoBoundlessRepositoryEntrance }},

        // Prison Torture Scene 
        { "A5_S2/Room/SimpleCutSceneFSM_A5妹妹回憶/FSM Animator/LogicRoot/[CutScene]",  new SimpleCutsceneSkipData(){  delayTime = 250, livesplitAddedTime = 0.0f }},
        { "A5_S2/Room/SimpleCutSceneFSM_A5妹妹回憶/FSM Animator/LogicRoot/[CutScene] Second",  new SimpleCutsceneSkipData(){  delayTime = 0, livesplitAddedTime = 0.0f }},
        { "A5_S2/Room/SimpleCutSceneFSM_A5妹妹回憶/FSM Animator/LogicRoot/[CutScene] Third",  new SimpleCutsceneSkipData(){  delayTime = 0, livesplitAddedTime = CutsceneTimingConstants.PrisonTortureScene }},    
    };
}

[HarmonyPatch]
internal class SimpleCutsceneSkip
{
    [HarmonyPostfix, HarmonyPatch(typeof(SimpleCutsceneManager), "PlayAnimation")]
    private static async void SimpleCutsceneManager_PlayAnimation_Postfix(SimpleCutsceneManager __instance)
    {
        if (!SpeedrunCutsceneSkip.Instance.SkipSetting.Value)
            return;

        var goPath = FullPath.GetFullPath(__instance.gameObject);
        if (SimpleCutsceneSkipDataTable.Table.ContainsKey(goPath))
        {
            int delayTime = SimpleCutsceneSkipDataTable.Table[goPath].delayTime;
            if(delayTime > 0)
            {
                Log.Info($"SpeedrunCutsceneSkip waiting {delayTime}ms before skipping cutscene: {goPath}");
                await UniTask.Delay(delayTime);
            }

            Log.Info($"SpeedrunCutsceneSkip skipping cutscene: {goPath}");
            __instance.TrySkip();

            float livesplitAddedTime = SimpleCutsceneSkipDataTable.Table[goPath].livesplitAddedTime;
            SpeedrunCutsceneSkip.AddSkippedTimeToLivesplit(livesplitAddedTime);
        }
    }
}
