using Assets.Scripts.Unit;
using UnityEngine;

namespace Assets.Scripts.Map
{
    public class HexTileController : MonoBehaviour
    {
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
                        meshRenderer.materials = new Material[] { GlobalDictionary.Materials.data["Black"] };
                        break;
                    case TileType.Neutral:
                        meshRenderer.materials = new Material[] { GlobalDictionary.Materials.data["Black"] };
                        break;
                    case TileType.Ally:
                        meshRenderer.materials = new Material[] { GlobalDictionary.Materials.data["White"] };
                        break;
                    case TileType.Enemy:
                        meshRenderer.materials = new Material[] { GlobalDictionary.Materials.data["Grey"] };
                        break;
                }
            }
        }

        private MeshRenderer meshRenderer;

        private UnitController unitAttached;
        /// <summary>
        /// 내거가 아님 or 설치된 기물 존재 or 미리보기중 = true
        /// </summary>
        public bool IsPossessed
        {
            get
            {
                return TileTypee != TileType.Ally || unitAttached != null || IsPreview;
            }
        }
        private bool IsPreview;

        /// <summary>
        /// 초기화 함수
        /// </summary>
        /// <param name="_hexCoordinate"></param>
        /// <param name="_tileType"></param>
        /// <returns></returns>
        public HexTileController Init(HexCoordinate _hexCoordinate, TileType _tileType)
        {
            HexCoor = _hexCoordinate.Clone();
            meshRenderer = GetComponent<MeshRenderer>();
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
            IsPreview = false;
            unitAttached = unitController.ConfirmInstallation();
            Vector3 resPos = transform.position;
            resPos.y = .5f;
            unitAttached.transform.position = resPos;
        }

        /// <summary>
        /// 미리보기 함수
        /// </summary>
        /// <param name="unitController"></param>
        public void PreviewUnit(UnitController unitController)
        {
            unitAttached = unitController.PreviewInstallation();
            IsPreview = true;
            Vector3 resPos = transform.position;
            resPos.y = .5f;
            unitAttached.transform.position = resPos;
        }

        /// <summary>
        /// 부착되어있던 유닛 제거 (부착 해제만 함) 함수
        /// </summary>
        public void ClearUnit()
        {
            IsPreview = false;
            unitAttached = null;
        }

        public enum TileType
        {
            Background, Neutral, Ally, Enemy
        }
    }
}
