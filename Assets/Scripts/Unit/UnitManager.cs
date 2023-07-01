using Assets.Scripts.Map;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Unit
{
    public class UnitManager : SingletonObject<UnitManager>
    {
        [SerializeField]
        private UnitController prefab;

        private new void Awake()
        {
            base.Awake();
        }

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
            convertedCoor = CommonFunction.ConvertCoordinate(t);
            GlobalStatus.Units[convertedCoor[0]][convertedCoor[1]] = Instantiate(GlobalDictionary.Prefab.Unit.Prefab, transform).Init(t, isEnemy);
            GlobalStatus.Map[convertedCoor[0]][convertedCoor[1]].InstallUnit(GlobalStatus.Units[convertedCoor[0]][convertedCoor[1]]);
        }
    }
}
