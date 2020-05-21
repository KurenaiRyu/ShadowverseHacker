using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using Wizard;

namespace ShadowverseHacker
{
    class Hacker : MonoBehaviour
    {
        int width = 360;
        int y = 0;
        GUIStyle style = new GUIStyle
        {
            fontSize = 22,
            wordWrap = true
        };
        BattleMgrBase BMgr;
        string info;

        private string Clean(string s)
        {
            return Regex.Replace(s, @" ?\[.*?\]", string.Empty);
        }
        public void Start()
        {
            File.WriteAllText(@"./hacker.txt", "hacker start");
            style.normal.textColor = Color.white;
            
        }
        public void Update()
        {
            Boolean debug = false;
            StringBuilder sb = new StringBuilder();
            if (BattleMgrBase.GetIns() != null)
            {
                BMgr = BattleMgrBase.GetIns();
                BattlePlayerPair pair;

                List<int> nums = new List<int>();
                for (int i = 2; i <= 5; i++)
                {
                    nums.Add(Predictor.RandomNum(i) + 1);
                }
                sb.Append(String.Join("、", nums));
                sb.Append("\n------------------\n");
                
                #region Normal Draw
                sb.Append("抽牌预测: \n");
                foreach (BattleCardBase c in Predictor.NormalDrawPredict(99))
                {
                    sb.Append(Clean(c.BaseParameter.CardName));
                    if (c.IsUnit) sb.Append($"({c.Cost},{c.Atk},{c.Life})");
                    sb.AppendLine();
                }
                sb.Append("------------------\n");
                foreach (BattleCardBase c in Predictor.ClassDrawPredict(9))
                {
                    sb.Append(Clean(c.BaseParameter.CardName));
                    if (c.IsUnit) sb.Append($"({c.Cost},{c.Atk},{c.Life})");
                    sb.AppendLine();
                }
                sb.Append("------------------\n");
                #endregion

                DetailPanelControl detail_panel = BMgr.DetailMgr.DetailPanelControl;
                if (detail_panel.IsShow)
                {
                    
                    BattleCardBase card = BMgr.DetailMgr.DetailPanelControl._card;
                    if (debug)
                    {
                        sb.Append("------------------\n");
                        sb.Append("BaseCardId: \n   -" + card.BaseParameter.BaseCardId + "\n");
                        sb.Append("CharType: \n   -" + card.BaseParameter.CharType + "\n");
                        sb.Append("Clan: \n   -" + card.BaseParameter.Clan + "\n");
                        sb.Append("EvoEffectType: \n   -" + card.BaseParameter.EvoEffectType + "\n");
                        sb.Append("SkillMoveType: \n   -" + String.Join("\n", card.BaseParameter.SkillMoveType + "\n"));
                        sb.Append("SummonEffectType: \n   -" + card.BaseParameter.SummonEffectType + "\n");
                        sb.Append("SummonMoveType: \n   -" + card.BaseParameter.SummonMoveType + "\n");
                        sb.Append("SkillEffectEnginType: \n   -" + String.Join("\n", card.BaseParameter.SkillEffectEnginType + "\n"));
                        sb.Append("SkillEffectTargetType: \n   -" + String.Join("\n", card.BaseParameter.SkillEffectTargetType + "\n"));
                        sb.Append("EvoSkillEffectEnginType: \n   -" + String.Join("\n", card.BaseParameter.EvoSkillEffectEnginType + "\n"));
                        sb.Append("EvoSkillEffectTargetType: \n   -" + String.Join("\n", card.BaseParameter.EvoSkillEffectTargetType + "\n"));

                        sb.Append("=======================\n");
                    }
                    pair = new BattlePlayerPair(card.SelfBattlePlayer, card.OpponentBattlePlayer);
                    sb.Append("选中卡牌: " + Clean(card.BaseParameter.CardName) + "\n");
                    sb.Append("效果预测: \n");
                    
                    int[] FieldFollowerPredictCardIds = {
                        123
                    };
                    int[] FieldFollowerNoClassPredictCardIds = {
                        101113010,//沉睡之森
                        103331030,//禁忌的研究者
                        103334010,//伊拉斯莫斯的秘密儀式
                        103521020,//死靈暗殺者
                        103521030,//蹣跚的活屍
                        103733010,//神魔審判所
                        104232010,//砲擊支援
                        106041020,//黃道天魔
                        107113010,//炎精之森
                        107141020,//昆蟲王
                        108011020,//蛋塔人
                        108713010,//愚神禮讚
                        108732010,//天狐之社
                        108621020,//銀鎖的使徒
                        112011020, //雷鳴軍神‧福尼加爾
                        113131030,//活潑的精靈‧小梅 
                        101111010//精靈女孩‧小梅
                    };
                    int[] NeftisPredictCardIds = {
                        103541010//奈芙蒂斯
                    };
                    int[] JabberwockPredictCardIds = {
                        105441020
                    };
                    int[] CountdownAmuletPredict = {
                        106733010//星導天球儀
                    };
                    int[] AmuletPredict = {
                        108741020//刻律涅
                    };

                    List<BattleCardBase> specialTargets = new List<BattleCardBase>();
                    int cardId = card.BaseParameter.BaseCardId;
                    if(Array.IndexOf(NeftisPredictCardIds, cardId) != -1)
                    {
                        specialTargets = Predictor.NeftisPredict();
                    }else if(Array.IndexOf(JabberwockPredictCardIds, cardId) != -1){
//                    	text += string.Join("、", randomClone.JabberwockPredict().ToArray());
                    }else if(Array.IndexOf(CountdownAmuletPredict, cardId) != -1){
//                    	text += string.Join("、", randomClone.CountdownAmuletPredict(2).ToArray());
                    }else if(Array.IndexOf(AmuletPredict, cardId) != -1){
//                    	text += string.Join("、", randomClone.AmuletPredict(2).ToArray());
                    }
                    
                    if (specialTargets.Count > 0) 
                    {
                        sb.Append(" >效果: \n");
                        foreach (BattleCardBase c in specialTargets)
                        {
                            sb.Append(Clean("   -" + c.BaseParameter.CardName));
                            if (c.IsUnit) sb.Append($"({c.Cost},{c.Atk},{c.Life})");
                            sb.AppendLine();
                        }
                    }
                    
                    Predictor.UpdateRandom();
                    foreach (SkillBase sk in card.Skills)
                    {
                        if (debug)
                        {
                            sb.Append(" >技能类型: " + sk.GetType() + "\n");
                            sb.Append(" >过滤器类型: " + sk.ApplyFilterCollection.ApplySelectFilter.GetType() + "\n");
                        }
                        if (sk is Skill_random_array)
                        {
                            string xyz = String.Join(", ", Predictor.RandomArray(sk as Skill_random_array, false));
                            sb.Append($" >XYZ: [{xyz}]\n");
                        }

                        List<BattleCardBase> targets = new List<BattleCardBase>();
                        SkillConditionCheckerOption checker = new SkillConditionCheckerOption();
                        if (sk.ApplyFilterCollection.ApplySelectFilter is SkillRandomSelectFilter)
                        {
                            targets = sk.ApplyFilterCollection.Filtering(pair, checker, sk.OptionValue).Cast<BattleCardBase>().ToList<BattleCardBase>();
                            targets = Predictor.RandomSelect(sk.ApplyFilterCollection.ApplySelectFilter, targets, sk.OptionValue, checker, false).ToList();
                        } else if (sk.ApplyFilterCollection.ApplySelectFilter is SkillIdNoDuplicationRandomSelectFilter)
                        {
                            targets = sk.ApplyFilterCollection.Filtering(pair, checker, sk.OptionValue).Cast<BattleCardBase>().ToList<BattleCardBase>();
                            targets = Predictor.IdNoDuplicateRandomSelect(sk.ApplyFilterCollection.ApplySelectFilter, targets, sk.OptionValue, checker, false).ToList();
                        } else if (sk.ApplyFilterCollection.ApplySelectFilter is SkillCostNoDuplicationRandomSelectFilter)
                        {
                            targets = sk.ApplyFilterCollection.Filtering(pair, checker, sk.OptionValue).Cast<BattleCardBase>().ToList<BattleCardBase>();
                            targets = Predictor.CostNoDuplicateRandomSelect(sk.ApplyFilterCollection.ApplySelectFilter, targets, sk.OptionValue, checker, false).ToList();
                        }
                        else
                        {
                            continue;
                        }
                        sb.Append(" >效果: \n");
                        foreach (BattleCardBase c in targets)
                        {
                            sb.Append("   -" + Clean(c.BaseParameter.CardName));
                            if (c.IsUnit) sb.Append($"({c.Cost},{c.Atk},{c.Life})");
                            sb.AppendLine();
                        }
                    }

                    Predictor.UpdateRandom();
                    sb.Append("进化效果预测:\n");
                    foreach (SkillBase sk in card.EvolutionSkills)
                    {
                        if (debug)
                        {
                            sb.Append(" >技能类型: " + sk.GetType() + "\n");
                            sb.Append(" >过滤器类型: " + sk.ApplyFilterCollection.ApplySelectFilter.GetType() + "\n");
                        }

                        if (sk is Skill_random_array)
                        {
                            string xyz = String.Join(", ", Predictor.RandomArray(sk as Skill_random_array, false));
                            sb.Append($" >XYZ: [{xyz}]\n");
                        }

                        List<BattleCardBase> targets = new List<BattleCardBase>();
                        SkillConditionCheckerOption checker = new SkillConditionCheckerOption();
                        if (sk.ApplyFilterCollection.ApplySelectFilter is SkillRandomSelectFilter)
                        {
                            targets = sk.ApplyFilterCollection.Filtering(pair, checker, sk.OptionValue).Cast<BattleCardBase>().ToList<BattleCardBase>();
                            targets = Predictor.RandomSelect(sk.ApplyFilterCollection.ApplySelectFilter, targets, sk.OptionValue, checker, false).ToList();
                        }
                        else if (sk.ApplyFilterCollection.ApplySelectFilter is SkillIdNoDuplicationRandomSelectFilter)
                        {
                            targets = sk.ApplyFilterCollection.Filtering(pair, checker, sk.OptionValue).Cast<BattleCardBase>().ToList<BattleCardBase>();
                            targets = Predictor.IdNoDuplicateRandomSelect(sk.ApplyFilterCollection.ApplySelectFilter, targets, sk.OptionValue, checker, false).ToList();
                        }
                        else if (sk.ApplyFilterCollection.ApplySelectFilter is SkillCostNoDuplicationRandomSelectFilter)
                        {
                            targets = sk.ApplyFilterCollection.Filtering(pair, checker, sk.OptionValue).Cast<BattleCardBase>().ToList<BattleCardBase>();
                            targets = Predictor.CostNoDuplicateRandomSelect(sk.ApplyFilterCollection.ApplySelectFilter, targets, sk.OptionValue, checker, false).ToList();
                        }
                        else
                        {
                            continue;
                        }
                        sb.Append(" >效果: \n");
                        foreach (BattleCardBase c in targets)
                        {
                            sb.Append(Clean("   -" + c.BaseParameter.CardName));
                            if (c.IsUnit) sb.Append($"({c.Cost},{c.Atk},{c.Life})");
                            sb.AppendLine();
                        }
                        
                        
                    }
                }
            } else
            {
                sb.Append("不在战斗中\n");
                BMgr = null;
            }
            info = sb.ToString();
        }

        public void OnGUI()
        {
            GUI.Box(new Rect(Screen.width - width, y, width, Screen.height - y), "");
            GUI.Label(new Rect(Screen.width - width, y, width, Screen.height - y), info, style);
        }

        public List<BattleCardBase> special()
        {
            return null;
        }
    }
}
