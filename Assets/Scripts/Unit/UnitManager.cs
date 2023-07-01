using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Map;
using Assets.Scripts.Server;
using UnityEngine;

namespace Assets.Scripts.Unit
{
    public class UnitManager : SingletonObject<UnitManager>
    {
        private Transform enemyTf, allyTf;
        private UnitController[][] units;

        private new void Awake()
        {
            base.Awake();
            allyTf = transform.GetChild(0);
            enemyTf = transform.GetChild(1);
        }

        public void Init()
        {
            units = new UnitController[ServerManager.Instance.Radius * 2 + 1][];
            for (int i = 0; i <= ServerManager.Instance.Radius * 2; i++)
            {
                units[i] = new UnitController[ServerManager.Instance.Radius * 2 + 1];
            }
        }

        /// <summary>
        /// 유닛들 생성 함수
        /// </summary>
        /// <param name="infos"></param>
        public void InitUnits(UnitLiveInfo[] infos, bool isEnemy)
        {
            foreach (UnitLiveInfo info in infos)
            {
                InitUnits(info, isEnemy);
            }
        }

        /// <summary>
        /// 유닛 생성 함수
        /// </summary>
        /// <param name="info"></param>
        public void InitUnits(UnitLiveInfo info, bool isEnemy)
        {
            UnitLiveInfo t = info.Clone();
            t.z = 1;
            int[] convertedCoor;
            Vector3 worldCoor;
            convertedCoor = CommonFunction.ConvertCoordinate(t);
            worldCoor = CommonFunction.ConvetCoordinateToWorldPosition(t);
            units[convertedCoor[0]][convertedCoor[1]] = Instantiate(GlobalDictionary.Prefab.Unit.data[info.title], isEnemy ? enemyTf : allyTf).Init(t, isEnemy);
            units[convertedCoor[0]][convertedCoor[1]].transform.localPosition = worldCoor;
        }
    }
}
