using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Fragment.Storage
{
    public class DeckLabelController : Button
    {
        private int idx;
        public int Idx
        {
            get
            {
                return idx;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            idx = transform.GetSiblingIndex();
        }
    }
}
