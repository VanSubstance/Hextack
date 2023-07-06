using Assets.Scripts.Common.Pooling;
using UnityEngine;

namespace Assets.Scripts.Battle
{
    public class ProjectileManager : AbsPoolingController
    {
        public static ProjectileManager Instance;

        private new void Awake()
        {
            base.Awake();
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public override Transform GetParent()
        {
            return transform;
        }
    }
}
