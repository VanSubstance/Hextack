using Assets.Scripts.Server;
using UnityEngine;

namespace Assets.Scripts.Map
{
    public static class CommonFunction
    {

        private static float sprt3 = Mathf.Sqrt(3), hexRadius = 1;

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

        /// <summary>
        ///  맵 중간 기준 육각 타일 좌표 -> Array 기준 배열 좌표로 반환해주는 함수
        /// </summary>
        /// <param name="hexCoor"></param>
        /// <returns></returns>
        public static int[] ConvertCoordinate(HexCoordinate hexCoor)
        {
            return new int[] { hexCoor.x + ServerManager.Instance.Radius, hexCoor.y + ServerManager.Instance.Radius };
        }
    }
}
