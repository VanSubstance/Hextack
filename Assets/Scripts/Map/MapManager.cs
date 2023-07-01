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
        private Transform mapTf;

        private new void Awake()
        {
            base.Awake();
            mapTf = transform.GetChild(0);
        }

        /// <summary>
        /// 신규 맵 생성 함수
        /// </summary>
        /// <param name="_radius">반지름</param>
        /// <param name="coors">배치 가능 타일 좌표들</param>
        public void Init(HexCoordinate[] coors)
        {
            GlobalStatus.Map = new HexTileController[GlobalStatus.Radius * 2 + 1][];
            for (int i = 0; i <= GlobalStatus.Radius * 2; i++)
            {
                GlobalStatus.Map[i] = new HexTileController[GlobalStatus.Radius * 2 + 1];
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
            GlobalStatus.Map[convertedCoor[0]][convertedCoor[1]] = Instantiate(tilePrefab, mapTf).Init(centre, HexTileController.TileType.Neutral);
            GlobalStatus.Map[convertedCoor[0]][convertedCoor[1]].transform.localPosition = worldCoor;

            InitTiles(coors);
        }

        private void InitTiles(HexCoordinate[] coors)
        {
            HashSet<int> idxsBan = new HashSet<int>();
            if (GlobalStatus.IsSingle)
            {
                while (idxsBan.Count < GlobalStatus.CntTileBan)
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
                if (GlobalStatus.IsSingle && idxsBan.Contains(i))
                {
                    GlobalStatus.Map[convertedCoor[0]][convertedCoor[1]] = Instantiate(tilePrefab, mapTf).Init(coor, HexTileController.TileType.Neutral);
                }
                else
                {
                    GlobalStatus.Map[convertedCoor[0]][convertedCoor[1]] = Instantiate(tilePrefab, mapTf).Init(coor, HexTileController.TileType.Ally);
                }
                GlobalStatus.Map[convertedCoor[0]][convertedCoor[1]].transform.localPosition = worldCoor;

                coor.Reverse();
                convertedCoor = CommonFunction.ConvertCoordinate(coor);
                worldCoor = CommonFunction.ConvetCoordinateToWorldPosition(coor);
                GlobalStatus.Map[convertedCoor[0]][convertedCoor[1]] = Instantiate(tilePrefab, mapTf).Init(coor, HexTileController.TileType.Enemy);
                GlobalStatus.Map[convertedCoor[0]][convertedCoor[1]].transform.localPosition = worldCoor;
            }
        }
    }
}
