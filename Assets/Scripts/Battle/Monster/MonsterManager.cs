using Assets.Scripts.Monster;
using UnityEngine;

namespace Assets.Scripts.Battle.Monster
{
    public class MonsterManager : AbsPoolingManager<MonsterManager>
    {

        public override Transform GetParent()
        {
            return transform;
        }

        public void SummonMonster(MonsterToken token)
        {
            MonsterInfo info = ServerData.Monster.data[token.Code].Clone();
            info.CntMonsterSummoned = 0;
            // 각 몬스터 별 소환 코루틴 실행
            ServerManager.Instance.ExecuteCrInRepeat(
                () =>
                {
                    info.CntMonsterSummoned++;
                    GetNewComponent().Init(new MonsterController.Info()
                    {
                        Hp = info.Hp,
                        Spd = 1,
                        InitPos = Vector2.zero,
                    });
                }, () =>
                {
                    return info.CntMonsterMax == info.CntMonsterSummoned;
                }, () =>
                {
                }, 1f
                );
        }
    }
}
