using Assets.Scripts.Server;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Map
{
    /// <summary>
    /// 반지름만큼 정육각형 타일을 정육각형으로 생성 및 관리하는 매니저
    /// </summary>
    public class MapManager : SingletonObject<MapManager>
    {
        [SerializeField]
        private HexTileController tilePrefab;
        [SerializeField]
        private Material materialBlack, materialGrey, materialWhite, materialRed, materialBlue;

        private HexTileController[][] map;
        private Transform mapTf;
        public static Material MaterialBlack, MaterialGrey, MaterialWhite, MaterialRed, MaterialBlue;

        private new void Awake()
        {
            base.Awake();
            mapTf = transform.GetChild(0);
            MaterialBlack = materialBlack;
            MaterialGrey = materialGrey;
            MaterialWhite = materialWhite;
            MaterialRed = materialGrey;
            MaterialBlue = materialBlue;
        }

        /// <summary>
        /// 신규 맵 생성 함수
        /// </summary>
        /// <param name="_radius">반지름</param>
        /// <param name="coors">배치 가능 타일 좌표들</param>
        public void Init(HexCoordinate[] coors)
        {
            map = new HexTileController[ServerManager.Instance.Radius * 2 + 1][];
            for (int i = 0; i <= ServerManager.Instance.Radius * 2; i++)
            {
                map[i] = new HexTileController[ServerManager.Instance.Radius * 2 + 1];
            }
            InitField(coors);
        }

        private void InitField(HexCoordinate[] coors)
        {
            int[] convertedCoor;
            Vector3 worldCoor;
            HexCoordinate centre = new(0, 0, 0);
            convertedCoor = CommonFunction.ConvertCoordinate(centre);
            worldCoor = CommonFunction.ConvetCoordinateToWorldPosition(centre);
            map[convertedCoor[0]][convertedCoor[1]] = Instantiate(tilePrefab, mapTf).Init(centre, HexTileController.TileType.Neutral);
            map[convertedCoor[0]][convertedCoor[1]].transform.localPosition = worldCoor;

            InitTiles(coors);
        }

        private void InitTiles(HexCoordinate[] coors)
        {
            HashSet<int> idxsBan = new HashSet<int>();
            if (ServerManager.Instance.IsSingle)
            {
                while (idxsBan.Count < ServerManager.Instance.CntTileBan)
                {
                    idxsBan.Add(Random.Range(0, coors.Length));

                }
            }

            int[] convertedCoor;
            Vector3 worldCoor;
            HexCoordinate coor;
            for (int i = 0; i < coors.Length; i++)
            {
                coor = coors[i];
                convertedCoor = CommonFunction.ConvertCoordinate(coor);
                worldCoor = CommonFunction.ConvetCoordinateToWorldPosition(coor);
                if (ServerManager.Instance.IsSingle && idxsBan.Contains(i))
                {
                    map[convertedCoor[0]][convertedCoor[1]] = Instantiate(tilePrefab, mapTf).Init(coor, HexTileController.TileType.Neutral);
                }
                else
                {
                    map[convertedCoor[0]][convertedCoor[1]] = Instantiate(tilePrefab, mapTf).Init(coor, HexTileController.TileType.Ally);
                }
                map[convertedCoor[0]][convertedCoor[1]].transform.localPosition = worldCoor;

                coor.Reverse();
                convertedCoor = CommonFunction.ConvertCoordinate(coor);
                worldCoor = CommonFunction.ConvetCoordinateToWorldPosition(coor);
                map[convertedCoor[0]][convertedCoor[1]] = Instantiate(tilePrefab, mapTf).Init(coor, HexTileController.TileType.Enemy);
                map[convertedCoor[0]][convertedCoor[1]].transform.localPosition = worldCoor;
            }
        }
    }
}
