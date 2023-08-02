using Assets.Scripts.UI.Achievement;
using Assets.Scripts.UI.Manager;
using UnityEngine;

namespace Assets.Scripts.Monster
{
    public class MonsterManager : AbsPoolingManager<MonsterManager, MonsterInfo>
    {
        public override Transform GetParent()
        {
            return transform;
        }
        public override int GetCountPoolForFirst()
        {
            return 300;
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
                    clone.Tracks = Path.PathManager.Instance.PathList[Mathf.Min(Path.PathManager.Instance.PathList.Length, token.IdxEnterance)].TargetTr;
                    clone.Hp = (int) (clone.Hp * Mathf.Pow(1.4f, ServerData.InGame.CurrentRound - 2));
                    clone.MaxHp = +clone.Hp;
                    GetNewContent(clone);
                    ServerData.InGame.CountMonsterLive++;
                    UIInGameManager.Instance.AchievementContainer.Achievements.ForEach((ach) =>
                    {
                        if (ach.ResourceType.Equals(AchievementInfo.TargetResourceType.Monster))
                        {
                            ach.UpdateCondition();
                        }
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
