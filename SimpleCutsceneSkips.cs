using Cysharp.Threading.Tasks;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEngine.GridBrushBase;

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

        //kuafu dialogue after yingzhao
        {"A2_S5_ BossHorseman_GameLevel/Room/Prefab/夸伏演出_SimpleCutSceneFSM/FSM Animator/LogicRoot/[CutScene]", new SimpleCutsceneSkipData(){ delayTime = 2500, livesplitAddedTime = CutsceneTimingConstants.MeetingKuafu }},

        //Goumang Elevator
        {"A10_S1/Room/[CutScene]關鳥籠/FSM Animator/LogicRoot/[CutScene]", new SimpleCutsceneSkipData(){ delayTime = 250, livesplitAddedTime = CutsceneTimingConstants.GoumangElevator }},

        //Chiyou saves you
        {"A6_S1/Room/Prefab/Simple Binding Tool/SimpleCutSceneFSM_暈眩演出/FSM Animator/LogicRoot/[CutScene]", new SimpleCutsceneSkipData(){ delayTime = 250, livesplitAddedTime = 0.0f }},
        {"AG_S2/Room/NPCs/議會演出相關Binding/ShanShan 軒軒分身 FSM/FSM Animator/CutScene/SimpleCutSceneFSM_蚩尤救回羿/FSM Animator/LogicRoot/[CutScene]", new SimpleCutsceneSkipData(){ delayTime = 250, livesplitAddedTime = CutsceneTimingConstants.ChiyouSaveU }},

        //shennong scenes

            //eating cutscene
            {"AG_S2/Room/NPCs/議會演出相關Binding/NPC_ShinNon_Base/NPC_ShinNon_Base_FSM/FSM Animator/LogicRoot/NPC_ShinNong_BaseVer/General FSM Object/Animator(FSM)/LogicRoot/[CutScene]藥材加入藥酒", new SimpleCutsceneSkipData(){ delayTime = 250, livesplitAddedTime = CutsceneTimingConstants.ShennongEating }},

            //health upgrades
            {"A3_S7/Room/Prefab/神農中毒情境/NPC_ShenNong_Poison/General FSM Object/Animator(FSM)/LogicRoot/[CutScene]神農解毒演出", new SimpleCutsceneSkipData(){ delayTime = 1667, livesplitAddedTime = CutsceneTimingConstants.HealthUpgrade[0] }},
            {"AG_S2/Room/NPCs/議會演出相關Binding/NPC_ShinNon_Base/NPC_ShinNon_Base_FSM/FSM Animator/LogicRoot/NPC_ShinNong_BaseVer/General FSM Object/Animator(FSM)/LogicRoot/[CutScene]第二次喝酒", new SimpleCutsceneSkipData(){ delayTime = 250, livesplitAddedTime = CutsceneTimingConstants.HealthUpgrade[1] }},
            {"AG_S2/Room/NPCs/議會演出相關Binding/NPC_ShinNon_Base/NPC_ShinNon_Base_FSM/FSM Animator/LogicRoot/NPC_ShinNong_BaseVer/General FSM Object/Animator(FSM)/LogicRoot/[CutScene]第三次喝酒", new SimpleCutsceneSkipData(){ delayTime = 250, livesplitAddedTime = CutsceneTimingConstants.HealthUpgrade[2] }},
            {"AG_S2/Room/NPCs/議會演出相關Binding/NPC_ShinNon_Base/NPC_ShinNon_Base_FSM/FSM Animator/LogicRoot/NPC_ShinNong_BaseVer/General FSM Object/Animator(FSM)/LogicRoot/[CutScene]第四次喝酒", new SimpleCutsceneSkipData(){ delayTime = 250, livesplitAddedTime = CutsceneTimingConstants.HealthUpgrade[3] }},
            {"AG_S2/Room/NPCs/議會演出相關Binding/NPC_ShinNon_Base/NPC_ShinNon_Base_FSM/FSM Animator/LogicRoot/NPC_ShinNong_BaseVer/General FSM Object/Animator(FSM)/LogicRoot/[CutScene]第五次喝酒", new SimpleCutsceneSkipData(){ delayTime = 250, livesplitAddedTime = CutsceneTimingConstants.HealthUpgrade[4] }},
            {"AG_S2/Room/NPCs/議會演出相關Binding/NPC_ShinNon_Base/NPC_ShinNon_Base_FSM/FSM Animator/LogicRoot/NPC_ShinNong_BaseVer/General FSM Object/Animator(FSM)/LogicRoot/[CutScene]第六次喝酒", new SimpleCutsceneSkipData(){ delayTime = 250, livesplitAddedTime = CutsceneTimingConstants.HealthUpgrade[5] }},
            {"AG_S2/Room/NPCs/議會演出相關Binding/NPC_ShinNon_Base/NPC_ShinNon_Base_FSM/FSM Animator/LogicRoot/NPC_ShinNong_BaseVer/General FSM Object/Animator(FSM)/LogicRoot/[CutScene]第七次喝酒", new SimpleCutsceneSkipData(){ delayTime = 250, livesplitAddedTime = CutsceneTimingConstants.HealthUpgrade[6] }},
            {"AG_S2/Room/NPCs/議會演出相關Binding/NPC_ShinNon_Base/NPC_ShinNon_Base_FSM/FSM Animator/LogicRoot/NPC_ShinNong_BaseVer/General FSM Object/Animator(FSM)/LogicRoot/[CutScene]第八次喝酒", new SimpleCutsceneSkipData(){ delayTime = 250, livesplitAddedTime = CutsceneTimingConstants.HealthUpgrade[7] }},

        //village scenes
        {"GameLevel/Room/Prefab/村民避難所_階段 FSM Object/FSM Animator/View/村民避難所ControlRoom/Phase1(截全打電話後)/EventBinding/釋放村民 FSM/FSM Animator/LogicRoot/[CutScene]", new SimpleCutsceneSkipData(){ delayTime = 250, livesplitAddedTime = CutsceneTimingConstants.FreeApemen }},
        {"GameLevel/Room/Prefab/村民避難所_階段 FSM Object/FSM Animator/View/村民避難所ControlRoom/Phase2(救完桃花村後)/Phase2 建村 FSM/FSM Animator/LogicRoot/卜巫/Phase 2 出現中毒者/FSM A0_S10 村民中毒/FSM Animator/LogicRoot/[CutScene] 遇見中毒村民", new SimpleCutsceneSkipData(){ delayTime = 250, livesplitAddedTime = CutsceneTimingConstants.SickApemanChild }},
        {"GameLevel/Room/Prefab/村民避難所_階段 FSM Object/FSM Animator/View/村民避難所ControlRoom/Phase2(救完桃花村後)/Phase2 建村 FSM/FSM Animator/LogicRoot/卜巫/Phase 2 出現中毒者/FSM A0_S10 村民中毒/FSM Animator/LogicRoot/[CutScene] 給村民解藥", new SimpleCutsceneSkipData(){ delayTime = 250, livesplitAddedTime = CutsceneTimingConstants.CureApemanChild }},

        //shuanshuan scenes

            //modifying printer
            {"AG_S2/Room/NPCs/議會演出相關Binding/ShanShan 軒軒分身 FSM/FSM Animator/CutScene/[CutScene] 多功能工具組_改造印表機/FSM Animator/LogicRoot/[CutScene]", new SimpleCutsceneSkipData(){ delayTime = 250, livesplitAddedTime = CutsceneTimingConstants.Shuanshuan_ModifyingPrinter }},
            //drawing yi portrait
            {"AG_S2/Room/NPCs/議會演出相關Binding/ShanShan 軒軒分身 FSM/FSM Animator/CutScene/[CutScene] 文房四寶_畫羿/FSM Animator/LogicRoot/[CutScene]", new SimpleCutsceneSkipData(){ delayTime = 250, livesplitAddedTime = CutsceneTimingConstants.Shuanshuan_YiPortrait }},
            //tree growth 1
            {"AG_S2/Room/NPCs/議會演出相關Binding/ShanShan 軒軒分身 FSM/FSM Animator/CutScene/[CutScene] 種子_第一次成長/FSM Animator/LogicRoot/[CutScene]", new SimpleCutsceneSkipData(){ delayTime = 250, livesplitAddedTime = CutsceneTimingConstants.Shuanshuan_Tree1 }},
            //tree growth 2
            {"AG_S2/Room/NPCs/議會演出相關Binding/ShanShan 軒軒分身 FSM/FSM Animator/CutScene/[CutScene] 種子_第二次成長/FSM Animator/LogicRoot/[CutScene]", new SimpleCutsceneSkipData(){ delayTime = 250, livesplitAddedTime = CutsceneTimingConstants.Shuanshuan_Tree2 }},
            //tree growth 3
            {"AG_S2/Room/NPCs/議會演出相關Binding/ShanShan 軒軒分身 FSM/FSM Animator/CutScene/[CutScene] 種子_最終成長/FSM Animator/LogicRoot/[CutScene]", new SimpleCutsceneSkipData(){ delayTime = 250, livesplitAddedTime = CutsceneTimingConstants.Shuanshuan_Tree3 }},
            //playing flute
            {"AG_S2/Room/NPCs/議會演出相關Binding/ShanShan 軒軒分身 FSM/FSM Animator/CutScene/[CutScene] 古樂譜_解出曲子/FSM Animator/LogicRoot/[CutScene]", new SimpleCutsceneSkipData(){ delayTime = 250, livesplitAddedTime = CutsceneTimingConstants.Shuanshuan_Flute }},
            //coding weather controller
            {"AG_S2/Room/NPCs/議會演出相關Binding/ShanShan 軒軒分身 FSM/FSM Animator/CutScene/[CutScene] 天道雜誌_天氣控制器/FSM Animator/LogicRoot/[CutScene]", new SimpleCutsceneSkipData(){ delayTime = 250, livesplitAddedTime = CutsceneTimingConstants.Shuanshuan_WeatherController }},
            //clay 1
            {"AG_S2/Room/NPCs/議會演出相關Binding/ShanShan 軒軒分身 FSM/FSM Animator/CutScene/[CutScene] 赤土_捏陶機/FSM Animator/LogicRoot/[CutScene]", new SimpleCutsceneSkipData(){ delayTime = 250, livesplitAddedTime = CutsceneTimingConstants.Shuanshuan_Clay1 }},
            //clay 2
            {"AG_S2/Room/NPCs/議會演出相關Binding/ShanShan 軒軒分身 FSM/FSM Animator/CutScene/[CutScene] 赤土_送夸伏陶器/FSM Animator/LogicRoot/[CutScene]", new SimpleCutsceneSkipData(){ delayTime = 250, livesplitAddedTime = CutsceneTimingConstants.Shuanshuan_Clay2 }},


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
