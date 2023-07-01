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
        private Material materialBlack, materialWhite, materialRed, materialBlue;

        private int radius, cntBuff, cntDebuff, cntUnit;
        private string mapTitle;

        private HexTileController[][] map;
        private Transform mapTf;

        private static float sprt3, hexRadius = 1;
        public static Material MaterialBlack, MaterialWhite, MaterialRed, MaterialBlue;

        /// <summary>
        ///  맵 중간 기준 육각 타일 좌표 -> Array 기준 배열 좌표로 반환해주는 함수
        /// </summary>
        /// <param name="hexCoor"></param>
        /// <returns></returns>
        public int[] ConvertCoordinate(HexCoordinate hexCoor)
        {
            return new int[] { hexCoor.x + radius, hexCoor.y + radius };
        }

        public static Vector3 ConvetCoordinateToWorldPosition(HexCoordinate hexCoor)
        {
            float xx, yy;
            xx = hexCoor.x * hexRadius * (3 / 2f);
            yy = hexCoor.x * hexRadius * sprt3 / 2f;
            yy += hexCoor.y * hexRadius * sprt3;
            return new Vector3(
                xx,
                hexCoor.z / 2f,
                yy
                );
        }

        private new void Awake()
        {
            base.Awake();
            mapTf = transform.GetChild(0);
            MaterialBlack = materialBlack;
            MaterialWhite = materialWhite;
            MaterialRed = materialRed;
            MaterialBlue = materialBlue;
            sprt3 = Mathf.Sqrt(3f);
        }

        /// <summary>
        /// 신규 맵 생성 함수
        /// </summary>
        /// <param name="mapInfo"></param>
        public void InitMap(MapInfo info)
        {
            radius = info.radius;
            mapTitle = info.mapTitle;
            cntBuff = info.cntBuff;
            cntDebuff = info.cntDebuff;
            cntUnit = info.cntUnit;
            map = new HexTileController[radius * 2 + 1][];
            for (int i = 0; i <= radius * 2; i++)
            {
                map[i] = new HexTileController[radius * 2 + 1];
            }
            InitField();
        }

        private void InitField()
        {
            int[] convertedCoor;
            Vector3 worldCoor;
            HexCoordinate centre = new(0, 0, 0);
            convertedCoor = ConvertCoordinate(centre);
            worldCoor = ConvetCoordinateToWorldPosition(centre);
            map[convertedCoor[0]][convertedCoor[1]] = Instantiate(tilePrefab, mapTf).Init(centre, HexTileController.TileType.Ally);
            map[convertedCoor[0]][convertedCoor[1]].transform.localPosition = worldCoor;

            string basePath = $"Datas/Maps/{radius}/{mapTitle}";
            HexCoordinate[] temp = Resources.LoadAll<HexCoordinate>($"{basePath}");
            InitTiles(temp);
        }

        private void InitTiles(HexCoordinate[] coors)
        {
            int[] convertedCoor;
            Vector3 worldCoor;
            foreach (HexCoordinate coor in coors)
            {
                convertedCoor = ConvertCoordinate(coor);
                worldCoor = ConvetCoordinateToWorldPosition(coor);
                map[convertedCoor[0]][convertedCoor[1]] = Instantiate(tilePrefab, mapTf).Init(coor, HexTileController.TileType.Ally);
                map[convertedCoor[0]][convertedCoor[1]].transform.localPosition = worldCoor;

                coor.Reverse();
                convertedCoor = ConvertCoordinate(coor);
                worldCoor = ConvetCoordinateToWorldPosition(coor);
                map[convertedCoor[0]][convertedCoor[1]] = Instantiate(tilePrefab, mapTf).Init(coor, HexTileController.TileType.Enemy);
                map[convertedCoor[0]][convertedCoor[1]].transform.localPosition = worldCoor;
            }
        }
    }
}
