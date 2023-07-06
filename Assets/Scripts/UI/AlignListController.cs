using Assets.Scripts.Common;
using System;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class AlignListController<TObject> : AbsPoolingController<TObject> where TObject : MonoBehaviour
    {
        public override Transform GetParent()
        {
            throw new NotImplementedException();
        }
    }
}
