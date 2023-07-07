using Assets.Scripts.Common.Pooling;
using UnityEngine;

namespace Assets.Scripts.Tower
{
    public class TowerManager : AbsPoolingController<TowerManager>
    {
        public override Transform GetParent()
        {
            return transform;
        }
    }
}
