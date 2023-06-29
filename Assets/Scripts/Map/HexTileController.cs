using UnityEngine;

namespace Assets.Scripts.Map
{
    public class HexTileController : MonoBehaviour
    {
        public int x, y;
        private bool isPermitted;
        public bool IsPermitted
        {
            get { return isPermitted; }
            set
            {
                isPermitted = value;
                meshRenderer.materials = new Material[] { isPermitted ? MapManager.MaterialActive : MapManager.MaterialInactive };
            }
        }
        private MeshRenderer meshRenderer;

        public HexTileController Init(int _x, int _y, bool _isPermitted = false)
        {
            x = _x;
            y = _y;
            meshRenderer = GetComponent<MeshRenderer>();
            IsPermitted = _isPermitted;
            return this;
        }
    }
}
