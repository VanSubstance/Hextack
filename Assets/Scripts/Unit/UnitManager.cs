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
        [SerializeField]
        private UnitController prefab;
        private Transform enemyTf, allyTf;

        private new void Awake()
        {
            base.Awake();
            allyTf = transform.GetChild(0);
            enemyTf = transform.GetChild(1);
        }

        /// <summary>
        /// 1 : 1
        /// 2 : 1 + 6
        /// 3 : 1 + 6 + 12
        /// </summary>

        public void Init()
        {
            GlobalDictionary.Prefab.Unit.Prefab = prefab;
            GlobalStatus.UnitPool = new Queue<UnitController>();
            GlobalStatus.Units = new UnitController[GlobalStatus.Radius * 2 + 1][];
            for (int i = 0; i <= GlobalStatus.Radius * 2; i++)
            {
                GlobalStatus.Units[i] = new UnitController[GlobalStatus.Radius * 2 + 1];
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
            GlobalStatus.Units[convertedCoor[0]][convertedCoor[1]] = Instantiate(GlobalDictionary.Prefab.Unit.Prefab, isEnemy ? enemyTf : allyTf).Init(t, isEnemy);
            GlobalStatus.Units[convertedCoor[0]][convertedCoor[1]].transform.localPosition = worldCoor;
        }
    }
}
