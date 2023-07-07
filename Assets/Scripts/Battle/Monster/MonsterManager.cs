using Assets.Scripts.Common.MainManager;
using Assets.Scripts.Common.Pooling;
using Assets.Scripts.Server;
using Assets.Scripts.Unit;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Battle.Monster
{
    public class MonsterManager : AbsPoolingController<MonsterManager>
    {
        private int cntTotalMonsterToSummon;

        public override Transform GetParent()
        {
            return transform;
        }

        public void SummonMonsters(UnitToken[] tokenList)
        {
            cntTotalMonsterToSummon = tokenList.Length;
            UnitInfo info;
            tokenList.All((token) =>
            {
                info = ServerData.Unit.data[token.Code];
                info.CntMonsterSummoned = 0;
                // 각 몬스터 별 소환 코루틴 실행
                ServerManager.Instance.ExecuteCrInRepeat(
                    () =>
                    {
                        info.CntMonsterSummoned++;
                        MainInGameManager.Instance.CurTimer++;
                        Debug.Log($"몬스터 소환:: {info.Title} => {info.IdxEnterance}");
                        GetNewComponent().Init(new MonsterController.Info()
                        {
                            Hp = 100,
                            Spd = 1,
                        });
                    }, () =>
                    {
                        return info.CntMonsterMax == info.CntMonsterSummoned;
                    }, () =>
                    {
                        cntTotalMonsterToSummon--;
                    }, info.TimeMarginSummon
                    );
                return true;
            });
        }

        /// <summary>
        /// 전투 종료인지 판별하는 함수
        /// </summary>
        /// <returns>0: 진행중, 1: 종료</returns>
        public int GetCurrentBattleStatus()
        {
            if (cntTotalMonsterToSummon == 0)
            {
                return 0;
            }
            return 1;
        }
    }
}
