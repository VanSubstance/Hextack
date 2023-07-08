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

        public TextController ExecuteDamage(TextController.Info _info)
        {
            TextController ret = GetNewComponent().GetComponent<TextController>();
            ret.Init(_info);
            return ret;
        }
    }
}
