using Assets.Scripts.Battle;
using Assets.Scripts.Unit;
using UnityEngine;
using static UnityEngine.AdaptivePerformance.Provider.AdaptivePerformanceSubsystemDescriptor;

namespace Assets.Scripts.Map
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class HexTileController : MonoBehaviour
    {
        public int x, y;
        private HexCoordinate hexCoordinate;
        public HexCoordinate HexCoor
        {
            get
            {
                return hexCoordinate;
            }
            set
            {
                hexCoordinate = value;
            }
        }
        public TileType tileType;
        public TileType TileTypee
        {
            get
            {
                return tileType;
            }
            set
            {
                tileType = value;
                switch (tileType)
                {
                    case TileType.Background:
                        meshRenderer.materials = new Material[] { GlobalDictionary.Materials.data["MarbleBlack"], GlobalDictionary.Materials.data["MarbleBlack"] };
                        break;
                    case TileType.Neutral:
                        meshRenderer.materials = new Material[] { GlobalDictionary.Materials.data["MarbleBlack"], GlobalDictionary.Materials.data["MarbleBlack"] };
                        break;
                    case TileType.Ally:
                        meshRenderer.materials = new Material[] { GlobalDictionary.Materials.data["MarbleWhite"], GlobalDictionary.Materials.data["MarbleWhite"] };
                        break;
                    case TileType.Enemy:
                        meshRenderer.materials = new Material[] { GlobalDictionary.Materials.data["MarbleGray"], GlobalDictionary.Materials.data["MarbleGray"] };
                        break;
                }
            }
        }

        private GameObject inRangeGO;

        public bool InRangeVisual
        {
            set
            {
                inRangeGO.SetActive(value);
            }
            get
            {
                return inRangeGO.activeSelf;
            }
        }

        private MeshRenderer meshRenderer;

        private UnitController unitAttached, unitPreview;
        /// <summary>
        /// 현재 부착된 유닛 코드 반환
        /// </summary>
        public string UnitCode
        {
            get
            {
                return unitAttached != null ? unitAttached.UnitCode : null;
            }
        }
        /// <summary>
        /// 설치 가능한 타일인지 판별
        /// </summary>
        public bool IsInstallable
        {
            get
            {
                return TileTypee == TileType.Ally;
            }
        }
        /// <summary>
        /// 내거가 아님 or 설치된 기물 존재 or 미리보기중 = true
        /// </summary>
        public bool IsPossessed
        {
            get
            {
                return !IsInstallable || unitAttached != null || IsPreview;
            }
        }
        private bool IsPreview;

        private void Awake()
        {
            inRangeGO = transform.GetChild(0).gameObject;
            InRangeVisual = false;
            meshRenderer = GetComponent<MeshRenderer>();
        }

        /// <summary>
        /// 초기화 함수
        /// </summary>
        /// <param name="_hexCoordinate"></param>
        /// <param name="_tileType"></param>
        /// <returns></returns>
        public HexTileController Init(HexCoordinate _hexCoordinate, TileType _tileType)
        {
            HexCoor = _hexCoordinate.Clone();
            x = HexCoor.x;
            y = HexCoor.y;
            TileTypee = _tileType;
            IsPreview = false;
            return this;
        }

        /// <summary>
        /// 타일에 기물 설치 함수
        /// </summary>
        /// <param name="unitController"></param>
        public void InstallUnit(UnitController unitController)
        {
            ClearPreview();
            if (unitAttached != null)
            {
                // 업그레이드
                unitController.Clear();
                unitAttached.LevelUp();
            }
            else
            {
                // 신규 배치
                unitAttached = unitController.Connect(this);
                int[] convertedCoor = CommonFunction.ConvertCoordinate(HexCoor);
                GlobalStatus.Units[convertedCoor[0]][convertedCoor[1]] = unitController;
                Vector3 resPos = transform.position;
                resPos.y = 1.1f;
                unitAttached.transform.position = resPos;
            }
        }

        /// <summary>
        /// 미리보기 함수
        /// </summary>
        /// <param name="unitController"></param>
        public void PreviewUnit(UnitController unitController)
        {
            unitPreview = unitController.PreviewInstallation();
            ActivateRange();
            IsPreview = true;
            Vector3 resPos = transform.position;
            resPos.y = 1.1f;
            unitPreview.transform.position = resPos;
        }

        /// <summary>
        /// 부착되어있던 유닛 제거 (부착 해제만 함) 함수
        /// </summary>
        public void ClearUnit()
        {
            IsPreview = false;
            DeActivateRange();
            unitAttached?.Disconnect();
            unitAttached = null;
        }

        /// <summary>
        /// 미리보기 꺼주기
        /// </summary>
        public void ClearPreview()
        {
            IsPreview = false;
            DeActivateRange();
            unitPreview?.Disconnect();
            unitPreview = null;
        }

        /// <summary>
        ///  사거리 가시화
        /// </summary>
        public void ActivateRange()
        {
            RangeViewController.Instance.ActivateInRange(this, unitPreview != null ? unitPreview.Range : unitAttached.Range);
        }

        /// <summary>
        /// 사거리 비가시화
        /// </summary>
        public void DeActivateRange()
        {
            RangeViewController.Instance.DeActivateInRange();
        }

        /// <summary>
        /// 만약 설치된 기물이 있다 -> 사거리 보여주기
        /// </summary>
        public void OnMouseDown()
        {
            if (unitAttached != null && unitAttached.IsLive)
            {
                ActivateRange();
            }
        }

        /// <summary>
        /// 사거리 끄기
        /// </summary>
        public void OnMouseUp()
        {
            if (unitAttached != null)
            {
                DeActivateRange();
            }
        }
    }
}
