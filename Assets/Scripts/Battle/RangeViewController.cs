using Assets.Scripts.Map;
using System.Collections.Generic;

namespace Assets.Scripts.Battle
{
    public class RangeViewController : SingletonObject<RangeViewController>
    {
        private List<int[]> curInRange;

        /// <summary>
        /// 사정거리 내 타일 켜기 함수
        /// </summary>
        /// <param name="centre"></param>
        /// <param name="_radius"></param>
        public void ActivateInRange(HexTileController centre, int _radius)
        {
            foreach (int[] coor in (curInRange = CommonFunction.SeekCoorsInRange(centre.HexCoor.x, centre.HexCoor.y, centre.HexCoor.z, _radius)))
            {
                GlobalStatus.Map[coor[0]][coor[1]].InRangeVisual = true;
            }
        }

        /// <summary>
        /// 사정거리 내 타일 끄기 함수
        /// </summary>
        public void DeActivateInRange()
        {
            if (curInRange == null) return;
            foreach (int[] coor in curInRange)
            {
                GlobalStatus.Map[coor[0]][coor[1]].InRangeVisual = false;
            }
        }
    }
}
