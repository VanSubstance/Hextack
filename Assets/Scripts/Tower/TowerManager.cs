using UnityEngine;

namespace Assets.Scripts.Tower
{
    public class TowerManager : AbsPoolingManager<TowerManager>
    {
        public override Transform GetParent()
        {
            return transform;
        }

        public TowerController GetNewTower(string code, Vector3 position)
        {
            TowerController ret = GetNewComponent().GetComponent<TowerController>();
            ret.Init(new TowerController.Info()
            {
                Code = code,
                Position = position,
            });

            return ret;
        }
    }
}
