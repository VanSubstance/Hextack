using UnityEngine;
using Assets.Scripts.Map;

namespace Assets.Scripts.Unit
{
    public class UnitController : MonoBehaviour
    {
        private UnitLiveInfo info;
        private bool isEnemy;
        private MeshRenderer meshRenderer;

        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        public UnitController Init(UnitLiveInfo _info, bool _isEnemy)
        {
            info = _info.Clone();
            isEnemy = _isEnemy;
            if (isEnemy)
            {
                Debug.Log("갸아악");
                meshRenderer.materials = new Material[] { GlobalDictionary.Materials.data["Red"] };
            }
            else
            {
                meshRenderer.materials = new Material[] { GlobalDictionary.Materials.data["White"] };
            }
            return this;
        }
    }
}
