using UnityEngine;

namespace Assets.Scripts.Battle
{
    public class ProjectileManager : AbsPoolingManager<ProjectileManager>
    { 
        public override Transform GetParent()
        {
            return transform;
        }
    }
}
