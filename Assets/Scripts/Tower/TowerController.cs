using UnityEngine;
using Assets.Scripts.Common.Pooling;

namespace Assets.Scripts.Tower
{
    public class TowerController : AbsPoolingContent
    {
        public override void Clear()
        {
            throw new System.NotImplementedException();
        }

        protected override bool InitExtra(AbsPoolingContent.Info _info)
        {
            if (_info is not Info info)
            {
                return false;
            }
            return true;
        }

        public new class Info : AbsPoolingContent.Info
        {

        }
    }
}
