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
                        meshRenderer.materials = new Material[] { MapManager.MaterialBlack };
                        break;
                    case TileType.Neutral:
                        meshRenderer.materials = new Material[] { MapManager.MaterialBlack };
                        break;
                    case TileType.Ally:
                        meshRenderer.materials = new Material[] { MapManager.MaterialWhite };
                        break;
                    case TileType.Enemy:
                        meshRenderer.materials = new Material[] { MapManager.MaterialGrey };
                        break;
                }
            }
        }

        private MeshRenderer meshRenderer;

        public HexTileController Init(HexCoordinate _hexCoordinate, TileType _tileType)
        {
            HexCoor = _hexCoordinate.Clone();
            meshRenderer = GetComponent<MeshRenderer>();
            TileTypee = _tileType;
            return this;
        }

        public enum TileType
        {
            Background, Neutral, Ally, Enemy
        }
    }
}
