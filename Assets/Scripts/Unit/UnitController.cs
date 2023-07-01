using UnityEngine;
using Assets.Scripts.Map;

namespace Assets.Scripts.Unit
{
    public class UnitController : MonoBehaviour
    {
        private UnitLiveInfo info;
        private bool isEnemy;
        private MeshRenderer meshRenderer;
        private MeshFilter meshFilter;
        private MeshCollider meshCollider;

        private void Awake()
        {
            gameObject.SetActive(false);
            meshRenderer = GetComponent<MeshRenderer>();
            meshFilter = GetComponent<MeshFilter>();
            meshCollider = GetComponent<MeshCollider>();
        }

        public UnitController Init(UnitLiveInfo _info, bool _isEnemy)
        {
            info = _info.Clone();
            meshFilter.mesh = GlobalDictionary.Mesh.data[_info.title];
            meshCollider.sharedMesh = meshFilter.mesh;
            meshCollider.convex = true;
            isEnemy = _isEnemy;
            if (isEnemy)
            {
                meshRenderer.materials = new Material[] { GlobalDictionary.Materials.data["Red"] };
            }
            else
            {
                meshRenderer.materials = new Material[] { GlobalDictionary.Materials.data["White"] };
            }
            gameObject.SetActive(true);
            return this;
        }
    }
}
