using Assets.Scripts.Map;
using UnityEngine;

namespace Assets.Scripts.Unit
{
    public class UnitController : MonoBehaviour
    {
        private UnitInfo liveInfo;
        public int Range
        {
            get
            {
                return liveInfo.Range;
            }
        }
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
        private HexTileController tileInstalled;

        /// <summary>
        /// 파티클 시스템
        /// </summary>
        private ParticleSystem particle;

        private void Awake()
        {
            gameObject.SetActive(false);
            meshRenderer = GetComponent<MeshRenderer>();
            meshFilter = GetComponent<MeshFilter>();
            meshCollider = GetComponent<MeshCollider>();
            rigid = GetComponent<Rigidbody>();
            UseGravity = false;
            particle = transform.GetChild(0).GetComponent<ParticleSystem>();
        }

        /// <summary>
        /// 초기화 함수
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_isEnemy"></param>
        /// <returns></returns>
        public UnitController Init(UnitToken _info, bool _isEnemy)
        {
            liveInfo = GlobalDictionary.Scriptable.Unit.data[_info.Title].Clone();
            meshFilter.mesh = GlobalDictionary.Mesh.data[_info.Title];
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

        /// <summary>
        /// 타일과 연결 함수
        /// </summary>
        /// <param name="_tileInstalled"></param>
        /// <returns></returns>
        public UnitController Connect(HexTileController _tileInstalled)
        {
            if (isEnemy)
            {
                TargetMaterial = "Red";
            }
            else
            {
                TargetMaterial = "White";
            }
            UseGravity = true;
            tileInstalled = _tileInstalled;
            return this;
        }


        /// <summary>
        /// 타일과의 연결을 끊는 함수
        /// </summary>
        /// <param name="_tileInstalled"></param>
        /// <returns></returns>
        public UnitController Disconnect()
        {
            tileInstalled = null;
            return this;
        }

        /// <summary>
        /// 미리보기 처리 함수
        /// </summary>
        /// <returns></returns>
        public UnitController PreviewInstallation()
        {
            TargetMaterial = "Fade";
            UseGravity = true;
            return this;
        }

        private void OnMouseDown()
        {
            if (tileInstalled != null)
            {
                tileInstalled.OnMouseDown();
            }
        }

        private void OnMouseUp()
        {
            if (tileInstalled != null)
            {
                tileInstalled.OnMouseUp();
            }
        }
    }
}
