using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Wizard;
using Wizard.Battle.Mulligan;
using Wizard.Battle.Phase;

namespace ShadowverseHacker
{
    static class Predictor
    {
        static StableRandomClone random;

        public static void UpdateRandom()
        {
            Predictor.random = new StableRandomClone();
        }

        public static int RandomNum(int size) 
        {
            UpdateRandom();
            return  random.StableRandom(size);
        }

        public static List<BattleCardBase> NormalDrawPredict(int count)
        {
            BattlePlayerBase player = BattleMgrBase.GetIns().GetBattlePlayer(true);
            UpdateRandom();
            
            if (player.DeckCardList.Count < count) count = player.DeckCardList.Count;

            IOrderedEnumerable<BattleCardBase> ordered = from x in player.DeckCardList
                                                                   orderby x.Index
                                                                   select x;
            List<BattleCardBase> deck = ordered.ToList<BattleCardBase>();
            List<BattleCardBase> drawed = new List<BattleCardBase>();
            for (int i = 0; i < count; i++)
            {
                int index = Predictor.random.StableRandom(deck.Count);
                drawed.Add(deck[index]);
                deck.Remove(deck[index]);
            }
            return drawed;
        }

        public static List<BattleCardBase> ClassDrawPredict(int count)
        {
            BattlePlayerBase player = BattleMgrBase.GetIns().GetBattlePlayer(true);
            UpdateRandom();
            
            IOrderedEnumerable<BattleCardBase> ordered = from x in player.DeckCardList
                where x.BaseParameter.CharType == CardBasePrm.CharaType.NORMAL
                orderby x.Index
                select x;
            List<BattleCardBase> deck = ordered.ToList<BattleCardBase>();
            if (deck.Count < count) count = deck.Count;
            List<BattleCardBase> result = new List<BattleCardBase>();
            for (int i = 0; i < count; i++)
            {
                int index = Predictor.random.StableRandom(deck.Count);
                result.Add(deck[index]);
                deck.Remove(deck[index]);
            }
            return result;
            
        }
        
        public static List<BattleCardBase> NeftisPredict()
        {
            UpdateRandom();
            BattleMgrBase ins = BattleMgrBase.GetIns();
            IEnumerable<BattleCardBase> enumerable = from x in ins.GetBattlePlayer(true).DeckCardList
                orderby x.Index
                where x.BaseParameter.CharType == CardBasePrm.CharaType.NORMAL && x.BaseParameter.BaseCardId != 103541010
                select x;
            List<BattleCardBase> list = new List<BattleCardBase>();
            List<BattleCardBase> result = new List<BattleCardBase>();
            foreach (BattleCardBase item in enumerable)
            {
                list.Add(item);
            }
            int num = 5 - ins.GetBattlePlayer(true).ClassAndInPlayCardList.Count;
            num = Math.Min(num, list.Count);
            for (int i = 0; i < num; i++)
            {
                if (list.Count > 0)
                {
                    int index = Predictor.random.StableRandom(list.Count);
                    BattleCardBase card = list[index];
                    list = (from c in list
                        where c.Card.BaseParameter.Cost != card.BaseParameter.Cost
                        select c).ToList<BattleCardBase>();
                    result.Add(card);
                }
            }
            return result;
        }

        public static List<BattleCardBase> MulliganPredict()
        {
            BattlePlayerBase player = BattleMgrBase.GetIns().GetBattlePlayer(true);
            NetworkMulliganPhase nmp = typeof(BattleMgrBase).GetField("_phase", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(BattleMgrBase.GetIns()) as NetworkMulliganPhase;
            NetworkMulliganMgr nmm = typeof(NetworkMulliganPhase).GetField("_networkMulliganMgr", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(nmp) as NetworkMulliganMgr;
            MulliganCtrl pmc = nmm.PlayerMlgCtrl;
            if (pmc != null)
            {
                List<int> index_list = typeof(MulliganCtrl).GetField("_mulliganAfterCardIndexList", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(pmc) as List<int>;
                return index_list.Select(i => BattleMgrBase.GetIns().GetBattleCardIdx(player.DeckCardList, i)).ToList<BattleCardBase>();
            }
            return new List<BattleCardBase>();
        }

        public static int[] RandomArray(Skill_random_array sk, Boolean flushRandom)
        {
            if (flushRandom) UpdateRandom();
            int size = sk.OptionValue.GetInt(SkillFilterCreator.ContentKeyword.random_range, -1);
            int max = sk.OptionValue.GetInt(SkillFilterCreator.ContentKeyword.max, -1);
            int sum;
            try
            {
                sum = sk.OptionValue.GetInt(SkillFilterCreator.ContentKeyword.sum, -1);
            }
            catch (Exception e)
            {
                //进化次数
                sum = sk.SkillPrm.selfBattlePlayer.EvolvedCards.Count;
            }
            int[] arr = new int[size];
            for (int i = 0; i < sum; i++)
            {
                if (max != -1)
                {
                    List<int> list = new List<int>();
                    for (int j = 0; j < size; j++)
                    {
                        if (arr[j] < max)
                        {
                            list.Add(j);
                        }
                    }
                    if (list.Count<int>() == 0)
                    {
                        break;
                    }
                    int randomCount = random.StableRandom(list.Count);
                    arr[list[randomCount]]++;
                }
                else
                {
                    int randomCount2 = random.StableRandom(size);
                    arr[randomCount2]++;
                }
            }
            return arr;
        }

        public static IEnumerable<BattleCardBase>  RandomSelect(ISkillSelectFilter filter, IEnumerable<BattleCardBase> cards, SkillOptionValue option, SkillConditionCheckerOption checkerOption, Boolean flushRandom)
        {
            if (flushRandom) UpdateRandom();
            int count = filter.CalcCount(option);
            cards = from x in cards
                    orderby x.Index
                    select x;
            List<BattleCardBase> cardList = cards.ToList<BattleCardBase>();
            count = Math.Min(count, cardList.Count);
            for (int i = 0; i < count; i++)
            {
                int index = Predictor.random.StableRandom(cardList.Count);
                BattleCardBase card = cardList[index];
                cardList.Remove(card);
                yield return card;
            }
            yield break;
        }

        public static IEnumerable<BattleCardBase> IdNoDuplicateRandomSelect(ISkillSelectFilter filter, IEnumerable<BattleCardBase> cards, SkillOptionValue option, SkillConditionCheckerOption checkerOption, Boolean flushRandom)
        {
            if (flushRandom) UpdateRandom();
            int count = filter.CalcCount(option);
            cards = from x in cards
                    orderby x.Index
                    select x;
            List<BattleCardBase> cardList = cards.ToList<BattleCardBase>();
            count = Math.Min(count, cardList.Count);
            for (int i = 0; i < count; i++)
            {
                int index = Predictor.random.StableRandom(cardList.Count);
                BattleCardBase card = cardList[index];
                cardList = (from c in cardList
                            where c.Card.BaseParameter.BaseCardId != card.BaseParameter.BaseCardId
                            select c).ToList<BattleCardBase>();
                yield return card;
            }
            yield break;
        }

        public static IEnumerable<BattleCardBase> CostNoDuplicateRandomSelect(ISkillSelectFilter filter, IEnumerable<BattleCardBase> cards, SkillOptionValue option, SkillConditionCheckerOption checkerOption, Boolean flushRandom)
        {
            if (flushRandom) UpdateRandom();
            int count = filter.CalcCount(option);
            cards = from x in cards
                    orderby x.Index
                    select x;
            List<BattleCardBase> cardList = cards.ToList<BattleCardBase>();
            count = Math.Min(count, cardList.Count);
            for (int i = 0; i < count; i++)
            {
                int index = Predictor.random.StableRandom(cardList.Count);
                BattleCardBase card = cardList[index];
                cardList = (from c in cardList
                            where c.Card.BaseParameter.Cost != card.BaseParameter.Cost
                            select c).ToList<BattleCardBase>();
                yield return card;
            }
            yield break;
        }
    }
}
