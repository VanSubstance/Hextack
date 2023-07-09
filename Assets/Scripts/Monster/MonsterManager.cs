using Assets.Scripts.Monster;
using UnityEngine;

namespace Assets.Scripts.Monster
{
    public class MonsterManager : AbsPoolingManager<MonsterManager, MonsterInfo>
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
                    MonsterInfo clone = info.Clone();
                    clone.Tracks = new()
                    {
                        new Vector3(-8.5f, 0, 3),
                        new Vector3(-6f, 0, 3),
                        new Vector3(6, 0, -3),
                        new Vector3(0, 0, -6.5f),
                        new Vector3(0, 0, 6.5f),
                        new Vector3(6f, 0, 3),
                        new Vector3(-6, 0, -3),
                        new Vector3(-5.5f, 0, 6.5f),
                    };
                    GetNewContent(clone);
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
