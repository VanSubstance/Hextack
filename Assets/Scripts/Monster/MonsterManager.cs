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
                    clone.Tracks = Path.PathManager.Instance.PathList[token.IdxEnterance].TargetTr;
                    GetNewContent(clone);
                    ServerData.InGame.CountMonsterLive++;
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
