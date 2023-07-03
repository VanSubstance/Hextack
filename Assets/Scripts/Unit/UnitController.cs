using Assets.Scripts.Map;
using UnityEngine;

namespace Assets.Scripts.Unit
{
    public class UnitController : MonoBehaviour
    {
        private UnitInfo liveInfo;

        /// <summary>
        /// 유닛 코드 반환
        /// </summary>
        public string UnitCode
        {
            get
            {
                return liveInfo.Code;
            }
        }
        public int Range
        {
            get
            {
                return liveInfo.Range;
            }
        }
        public bool IsEnemy;
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

        /// <summary>
        /// 실제 전투를 관리하는 컨트롤러
        /// </summary>
        public UnitBattleController BattleController;

        /// <summary>
        /// 생존 확인
        /// </summary>
        public bool IsLive
        {
            get
            {
                return gameObject.activeSelf;
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
            particle = transform.GetChild(0).GetComponent<ParticleSystem>();
            BattleController = GetComponent<UnitBattleController>();
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 초기화 함수
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_isEnemy"></param>
        /// <returns></returns>
        public UnitController Init(UnitToken _info, bool _isEnemy)
        {
            liveInfo = ServerData.Unit.data[_info.Title].Clone();
            meshFilter.mesh = GlobalDictionary.Mesh.data[_info.Title];
            meshCollider.sharedMesh = meshFilter.mesh;
            meshCollider.convex = true;
            IsEnemy = _isEnemy;
            TargetMaterial = "Fade";
            meshRenderer.materials = new Material[] { };
            gameObject.SetActive(true);
            return this;
        }

        /// <summary>
        /// 기물 레벨업
        /// </summary>
        /// <returns></returns>
        public UnitController LevelUp()
        {
            Debug.Log("업그레이드");
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
            Disconnect();
            return this;
        }

        /// <summary>
        /// 타일과 연결 함수
        /// </summary>
        /// <param name="_tileInstalled"></param>
        /// <returns></returns>
        public UnitController Connect(HexTileController _tileInstalled)
        {
            GlobalStatus.UnitsActive.Add(this);
            if (IsEnemy)
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
            GlobalStatus.UnitsActive.Remove(this);
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

        /// <summary>
        /// 전투 초기화 함수: 사전 효과 선 적용
        /// </summary>
        public void InitBattle()
        {
            BattleController.Init(liveInfo, tileInstalled.HexCoor, IsEnemy, () =>
            {
                Clear();
            });
        }

        /// <summary>
        /// 전투 활성화 함수
        /// </summary>
        public void EnableBattle()
        {
            BattleController.Enable();
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
