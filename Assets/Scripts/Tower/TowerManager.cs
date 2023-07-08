using UnityEngine;

namespace Assets.Scripts.Tower
{
    public class TowerManager : AbsPoolingManager<TowerManager>
    {
        public override Transform GetParent()
        {
            return transform;
        }
    }
}
