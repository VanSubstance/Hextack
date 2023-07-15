using UnityEngine;

namespace Assets.Scripts.UI.DamageText
{
    public class TextManager : AbsPoolingManager<TextManager, TextInfo>
    {
        [SerializeField]
        private Transform contentParent;
        public override Transform GetParent()
        {
            return contentParent;
        }
    }
}
