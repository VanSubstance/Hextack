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
        private Rigidbody rigid;
        private bool UseGravity
        {
            set
            {
                rigid.useGravity = value;
            }
        }
        public string TargetMaterial
        {
            set
            {
                meshRenderer.materials = new Material[] { GlobalDictionary.Materials.data[value] };
            }
        }

        private void Awake()
        {
            gameObject.SetActive(false);
            meshRenderer = GetComponent<MeshRenderer>();
            meshFilter = GetComponent<MeshFilter>();
            meshCollider = GetComponent<MeshCollider>();
            rigid = GetComponent<Rigidbody>();
            UseGravity = false;
        }

        /// <summary>
        /// 초기화 함수
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_isEnemy"></param>
        /// <returns></returns>
        public UnitController Init(UnitLiveInfo _info, bool _isEnemy)
        {
            info = _info.Clone();
            meshFilter.mesh = GlobalDictionary.Mesh.data[_info.title];
            meshCollider.sharedMesh = meshFilter.mesh;
            meshCollider.convex = true;
            isEnemy = _isEnemy;
            TargetMaterial = "Fade";
            meshRenderer.materials = new Material[] { };
            gameObject.SetActive(true);
            return this;
        }

        /// <summary>
        /// 기물 오브젝트 풀에 반납 함수
        /// </summary>
        /// <returns></returns>
        public UnitController Clear()
        {
            gameObject.SetActive(false);
            UseGravity = false;
            GlobalStatus.UnitPool.Enqueue(this);
            return this;
        }

        public UnitController ConfirmInstallation()
        {
            if (isEnemy)
            {
                TargetMaterial = "Red";
            } else
            {
                TargetMaterial = "White";
            }
            UseGravity = true;
            return this;
        }
    }
}
