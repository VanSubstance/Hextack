using Assets.Scripts.Unit;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Map
{
    public static class CommonFunction
    {

        private static float sprt3 = Mathf.Sqrt(3), hexRadius = 1;

        public static Vector3 ConvertCoordinateToWorldPosition(HexCoordinate hexCoor)
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
            return new int[] { hexCoor.x + GlobalStatus.Radius, hexCoor.y + GlobalStatus.Radius };
        }

        /// <summary>
        /// Hex 기준 좌표 -> Array 기준 배열 좌표 변환 함수
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static int[] ConvertCoordinate(int x, int y, int z)
        {
            return new int[] { x + GlobalStatus.Radius, y + GlobalStatus.Radius };
        }

        /// <summary>
        /// Hex 기준 좌표 -> range 이내의 Array 기준 배열 좌표들 변환 함수
        /// purpose가 0 초과 : Disable 된 객체는 무시
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="purpose">0: 위치 전부; 1: 적 기물 있는 위치만; 2: 아군 기물 있는 위치만</param>
        /// <returns></returns>
        public static List<int[]> SeekCoorsInRange(int x, int y, int z, int range, int purpose = 0, bool isTargetOnlyOne = false)
        {
            List<int[]> res = new List<int[]>();
            int sx, sy, cx, cy;
            int[] temp = ConvertCoordinate(x, y, z);
            sx = temp[0];
            sy = temp[1];
            if (purpose == 0)
            {
                res.Add(new int[] { sx, sy });
            }
            UnitController finder;
            void TryAdd()
            {
                finder = GlobalStatus.Units[cx][cy];
                if (
                    purpose == 0 ||
                    (purpose == 1 && finder != null && finder.IsEnemy) ||
                    (purpose == 2 && finder != null && !finder.IsEnemy)
                    )
                {
                    if (finder != null && !finder.gameObject.activeSelf)
                    {
                        return;
                    }
                    res.Add(new int[] { cx, cy });
                }
            }

            // 중심 기준 radius 길이만큼 십자
            for (int i = -range; i <= range; i++)
            {
                if (i == 0) continue;
                cx = sx + i;
                cy = sy;
                if (cx >= 0 && cx < GlobalStatus.Radius * 2 + 1 && GlobalStatus.Map[cx][cy] != null)
                {
                    TryAdd();
                    if (res.Count > 0 && isTargetOnlyOne) return res;
                }
                cx = sx;
                cy = sy + i;
                if (cy >= 0 && cy < GlobalStatus.Radius * 2 + 1 && GlobalStatus.Map[cx][cy] != null)
                {
                    TryAdd();
                    if (res.Count > 0 && isTargetOnlyOne) return res;
                }
            }

            // 2, 4분면: radius 길이 정육각형
            for (int i = 1; i <= range; i++)
            {
                for (int j = 1; j <= range; j++)
                {
                    cx = sx + i;
                    cy = sy - j;
                    if (cy >= 0 && cy < GlobalStatus.Radius * 2 + 1 && cx >= 0 && cx < GlobalStatus.Radius * 2 + 1 && GlobalStatus.Map[cx][cy] != null)
                    {
                        TryAdd();
                        if (res.Count > 0 && isTargetOnlyOne) return res;
                    }
                    cx = sx - i;
                    cy = sy + j;
                    if (cy >= 0 && cy < GlobalStatus.Radius * 2 + 1 && cx >= 0 && cx < GlobalStatus.Radius * 2 + 1 && GlobalStatus.Map[cx][cy] != null)
                    {
                        TryAdd();
                        if (res.Count > 0 && isTargetOnlyOne) return res;
                    }
                }
            }

            // 1, 3분면: radius - 1 길이 직각 이등변 삼각형
            for (int i = 1; i <= range - 1; i++)
            {
                for (int j = 1; j <= range - i; j++)
                {
                    cx = sx + i;
                    cy = sy + j;
                    if (cy >= 0 && cy < GlobalStatus.Radius * 2 + 1 && cx >= 0 && cx < GlobalStatus.Radius * 2 + 1 && GlobalStatus.Map[cx][cy] != null)
                    {
                        TryAdd();
                        if (res.Count > 0 && isTargetOnlyOne) return res;
                    }
                    cx = sx - i;
                    cy = sy - j;
                    if (cy >= 0 && cy < GlobalStatus.Radius * 2 + 1 && cx >= 0 && cx < GlobalStatus.Radius * 2 + 1 && GlobalStatus.Map[cx][cy] != null)
                    {
                        TryAdd();
                        if (res.Count > 0 && isTargetOnlyOne) return res;
                    }
                }
            }

            return res;
        }
    }
}
