using Assets.Scripts.Battle;
using Assets.Scripts.Map;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Unit
{
    public class UnitManager : SingletonObject<UnitManager>
    {
        [SerializeField]
        private UnitController prefab;

        public void Init()
        {
            GlobalDictionary.Prefab.Unit.Prefab = prefab;
            GlobalStatus.UnitPool = new Queue<UnitController>();
            GlobalStatus.UnitsActive = new List<UnitController>();
            GlobalStatus.Units = new UnitController[GlobalStatus.Radius * 2 + 1][];
            for (int i = 0; i <= GlobalStatus.Radius * 2; i++)
            {
                GlobalStatus.Units[i] = new UnitController[GlobalStatus.Radius * 2 + 1];
            }
            // 유닛 풀 10개 사전 생성
            GlobalStatus.UnitPool.Enqueue(Instantiate(prefab, transform));
        }

        /// <summary>
        /// 유닛들 생성 함수
        /// </summary>
        /// <param name="infos"></param>
        public void InitUnits(UnitToken[] infos, bool isEnemy)
        {
            foreach (UnitToken info in infos)
            {
                InitUnits(info, isEnemy);
            }
        }

        /// <summary>
        /// 유닛 생성 함수
        /// </summary>
        /// <param name="info"></param>
        public void InitUnits(UnitToken info, bool isEnemy)
        {
            UnitToken t = info.Clone();
            t.z = 1;
            int[] convertedCoor;
            convertedCoor = CommonFunction.ConvertCoordinate(t);
            GlobalStatus.Map[convertedCoor[0]][convertedCoor[1]].InstallUnit(GetNewUnit().Init(t, isEnemy));
        }

        /// <summary>
        /// 유닛 오브젝트 생성 함수
        /// </summary>
        public UnitController GetNewUnit()
        {
            UnitController res;
            if ((res = GlobalStatus.GetUnit()) == null)
            {
                res = Instantiate(prefab, transform);
            }
            return res;
        }
    }
}
