using Assets.Scripts.Common.Pooling;
using UnityEngine;

namespace Assets.Scripts.Battle
{
    public class ProjectileManager : AbsPoolingController<ProjectileManager>
    { 
        public override Transform GetParent()
        {
            return transform;
        }
    }
}
