using UnityEngine;

namespace Assets.Scripts.Tower
{
    public class TowerManager : AbsPoolingManager<TowerManager, TowerInfo>
    {
        public override Transform GetParent()
        {
            return transform;
        }
    }
}
