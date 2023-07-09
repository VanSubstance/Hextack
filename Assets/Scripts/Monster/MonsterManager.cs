using UnityEngine;

namespace Assets.Scripts.Monster
{
    /// <summary>
    /// 몬스터 풀링 매니저
    /// </summary>
    public class MonsterManager : AbsPoolingManager<MonsterManager, MonsterInfo>
    {
        public override Transform GetParent()
        {
            return transform;
        }
    }
}
