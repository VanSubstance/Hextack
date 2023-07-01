using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Unit;
using UnityEngine;

namespace Assets.Scripts.Server
{
    /// <summary>
    /// Resources 관리용 매니저
    /// </summary>
    public class DataManager : SingletonObject<DataManager>
    {
        private new void Awake()
        {
            base.Awake();
            LoadUnits();
        }
        
        /// <summary>
        /// 유닛 프리펩 로드 함수
        /// </summary>
        public void LoadUnits()
        {
            foreach (UnitController unit in Resources.LoadAll<UnitController>($"{GlobalDictionary.Prefab.Unit.rootPath}"))
            {
                GlobalDictionary.Prefab.Unit.data[unit.name] = unit;
            }
        }
    }
}
