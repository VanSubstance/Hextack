using UnityEngine;

namespace Assets.Scripts.UI
{
    public class TextManager : AbsPoolingManager<TextManager>
    {
        [SerializeField]
        private Transform contentParent;
        public override Transform GetParent()
        {
            return contentParent;
        }
    }
}
