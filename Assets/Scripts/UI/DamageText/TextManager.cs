using Assets.Scripts.Common.Pooling;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class TextManager : AbsPoolingController<TextManager>
    {
        [SerializeField]
        private Transform contentParent;
        public override Transform GetParent()
        {
            return contentParent;
        }
    }
}
