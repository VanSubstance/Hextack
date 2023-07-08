using UnityEngine;

namespace Assets.Scripts.Battle
{
    public class ProjectileManager : AbsPoolingManager<ProjectileManager>
    {
        public override Transform GetParent()
        {
            return transform;
        }

        public ProjectileController ExecuteNewProjectile(ProjectileController.Info info)
        {
            ProjectileController ret = GetNewComponent().GetComponent<ProjectileController>();
            ret.Init(info);
            return ret;
        }
    }
}
