using UnityEngine;

namespace Assets.Scripts.Battle.Area
{
    public class AreaManager : AbsPoolingManager<AreaManager, AreaInfo>
    {
        public override Transform GetParent()
        {
            return transform;
        }
        public override int GetCountPoolForFirst()
        {
            return 10;
        }
    }
}
