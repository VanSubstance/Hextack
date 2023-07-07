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

        public override Transform GetParent()
        {
            return transform;
        }

        public void SummonMonsters(UnitToken[] tokenList)
        {
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
                        GetNewComponent().Init(new MonsterController.Info()
                        {
                            Hp = info.Hp,
                            Spd = 1,
                            InitPos = CommonFunction.ConvertHexCoordinateToWorldPosition(ServerData.InGame.DungeonInfo.EnteranceList[info.IdxEnterance]) + Vector3.up,
                        });
                    }, () =>
                    {
                        return info.CntMonsterMax == info.CntMonsterSummoned;
                    }, () =>
                    {
                    }, info.TimeMarginSummon
                    );
                return true;
            });
        }
    }
}
