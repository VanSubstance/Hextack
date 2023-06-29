using UnityEngine;

namespace Assets.Scripts.Map
{
    /// <summary>
    /// 반지름만큼 정육각형 타일을 정육각형으로 생성 및 관리하는 매니저
    /// </summary>
    public class MapManager : SingletonObject<MapManager>
    {
        [SerializeField]
        private int radius;
        [SerializeField]
        private HexTileController tilePrefab;
        private HexTileController[][] map;
        private static float sprt3, hexRadius = 1;

        public int[] ConvertCoordinate(int x, int y)
        {
            return new int[] { x + radius, y + radius };
        }

        public float[] ConvetCoordinateToWorldPosition(int x, int y)
        {
            float xx, yy;
            xx = x * hexRadius * (3 / 2f);
            yy = x * hexRadius * sprt3 / 2f;
            yy += y * hexRadius * sprt3;
            return new float[] { xx, yy };
        }

        private new void Awake()
        {
            base.Awake();
            // 반지름만큼 생성
            sprt3 = Mathf.Sqrt(3f);
            map = new HexTileController[radius * 2 + 1][];
            for (int i = 0; i <= radius * 2; i++)
            {
                map[i] = new HexTileController[radius * 2 + 1];
            }

            // 해당하는 칸들만 할당
            int lim, tempC;
            int[] convertedCoor;
            float[] worldCoor;
            for (int r = -radius; r <= radius; r++)
            {
                lim = Mathf.Abs(r);
                for (int c = -radius; c <= radius - lim; c++)
                {
                    tempC = r < 0 ? -c : c;
                    convertedCoor = ConvertCoordinate(tempC, r);
                    worldCoor = ConvetCoordinateToWorldPosition(tempC, r);
                    map[convertedCoor[0]][convertedCoor[1]] = Instantiate(tilePrefab, transform).Init(tempC, r);
                    map[convertedCoor[0]][convertedCoor[1]].transform.localPosition = new Vector3(worldCoor[0], 0, worldCoor[1]);
                }
            }
        }
    }
}
