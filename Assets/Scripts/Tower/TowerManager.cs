using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Assets.Scripts.Tower
{
    public class TowerManager : AbsPoolingManager<TowerManager, TowerInfo>
    {
        public List<TowerController> TowerLiveList = new List<TowerController>();
        private Coroutine CrSustainTowerLastClicked;
        public override Transform GetParent()
        {
            return transform;
        }

        /// <summary>
        /// 설치된 타워 중 같은 타워 리스트 반환
        /// </summary>
        /// <param name="tower"></param>
        /// <returns></returns>
        public List<TowerController> FindSameTowers(TowerController tower)
        {
            return TowerLiveList.Where((towerC) =>
            {
                return towerC.Code.Equals(tower.Code);
            }).ToList();
        }

        /// <summary>
        /// 마지막으로 선택한 타워 유지시간 = 0.5초
        /// </summary>
        public void SustainTowerLastClicked()
        {
            if (CrSustainTowerLastClicked != null)
            {
                ServerManager.Instance.StopCoroutine(CrSustainTowerLastClicked);
                CrSustainTowerLastClicked = null;
            }
            CrSustainTowerLastClicked = ServerManager.Instance.ExecuteWithDelay(() =>
            {
                ServerData.InGame.LastTowerClicked = null;
            }, .5f);
        }
    }
}
