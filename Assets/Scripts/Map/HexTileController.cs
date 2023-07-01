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
        private TileType tileType;
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

        public UnitController UnitAttached;

        public HexTileController Init(HexCoordinate _hexCoordinate, TileType _tileType)
        {
            HexCoor = _hexCoordinate.Clone();
            meshRenderer = GetComponent<MeshRenderer>();
            TileTypee = _tileType;
            return this;
        }

        /// <summary>
        /// 타일에 기물 설치 함수
        /// </summary>
        /// <param name="unitController"></param>
        public void InstallUnit(UnitController unitController)
        {
            UnitAttached = unitController.ConfirmInstallation();
            Vector3 resPos = transform.position;
            resPos.y = .5f;
            UnitAttached.transform.position = resPos;
        }

        public enum TileType
        {
            Background, Neutral, Ally, Enemy
        }
    }
}
