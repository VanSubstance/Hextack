using Assets.Scripts.Battle;
using Assets.Scripts.Common.MainManager;
using Assets.Scripts.Unit;
using UnityEngine;

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

        private Unit.UnitController unitAttached, unitPreview;
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
            transform.position = CommonFunction.ConvertCoordinateToWorldPosition(_hexCoordinate);
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
        public void InstallUnit(Unit.UnitController unitController)
        {
            if (!unitController.IsEnemy)
            {
                foreach (UnitInfo _info in ServerData.InGame.DeckAlly)
                {
                    if (_info.Code.Equals(unitController.Info.Code))
                    {
                        _info.CountSummon++;
                        break;
                    }
                }
            }
            ClearPreview();
            EffectManager.Instance.ExecutNewEffect("Cloud", transform.position, Color.white);
            if (unitAttached != null)
            {
                // 업그레이드
                unitController.Clear();
                UpgradeUnit();
            }
            else
            {
                // 신규 배치
                unitAttached = unitController.Connect(this);
                int[] convertedCoor = CommonFunction.ConvertCoordinate(HexCoor);
                GlobalStatus.Units[convertedCoor[0]][convertedCoor[1]] = unitController;
                Vector3 resPos = transform.position;
                resPos.y = 1f;
                unitAttached.transform.position = resPos;
            }
        }

        /// <summary>
        /// 설치된 기물 업그레이드
        /// </summary>
        public void UpgradeUnit()
        {
            ClearPreview();
            unitAttached.LevelUp();
            // 레벨업 이펙트 띄워주기
            EffectManager.Instance.ExecutNewEffect("LevelUp", transform.position + (Vector3.up * 2), Color.white);
        }

        /// <summary>
        /// 미리보기 함수
        /// </summary>
        /// <param name="unitController"></param>
        public void PreviewUnit(Unit.UnitController unitController, bool isWithSight = true)
        {
            if (unitAttached == null)
            {
                // 신규
                unitPreview = unitController.PreviewInstallation(this);
                IsPreview = true;
                Vector3 resPos = transform.position;
                resPos.y = 1.1f;
                unitPreview.transform.position = resPos;
            }
            else
            {
                // 업그레이드 = 기존 기물 레벨업 대기상태 걸어주기
                Debug.Log("업그레이드 예정 상태 이펙트 고안");
            }
            if (isWithSight)
            {
                ActivateRange();
            }
        }

        /// <summary>
        /// 미리보기 유닛 설치 확정
        /// </summary>
        public void InstallPreview()
        {
            if (unitAttached == null)
            {
                InstallUnit(unitPreview);
            } else
            {
                UpgradeUnit();
            }
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
                MainInGameManager.Instance.InitUnitInfo(unitAttached.Info);
                ActivateRange();
                return;
            }
            if (unitPreview != null)
            {
                MainInGameManager.Instance.InitUnitInfo(unitPreview.Info);
                ActivateRange();
                return;
            }
        }

        /// <summary>
        /// 사거리 끄기
        /// </summary>
        public void OnMouseUp()
        {
            if (unitAttached != null ||
                unitPreview != null)
            {
                MainInGameManager.Instance.ClearUnitInfo();
                DeActivateRange();
            }
        }
    }
}
