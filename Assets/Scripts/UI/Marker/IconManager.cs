using Assets.Scripts.Common.Pooling;
using UnityEngine;

namespace Assets.Scripts.UI.Marker
{
    public class IconManager : AbsPoolingController<IconManager>
    {
        public override Transform GetParent()
        {
            return transform;
        }
    }
}
