using UnityEngine;

namespace Assets.Scripts.Battle.Projectile
{
    public class ProjectileManager : AbsPoolingManager<ProjectileManager, ProjectileInfo>
    {
        public override Transform GetParent()
        {
            return transform;
        }
    }
}
