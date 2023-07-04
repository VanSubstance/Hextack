﻿using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Unit
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "UnitInfo", menuName = "Scriptables/Unit/Info", order = int.MaxValue)]
    public class UnitInfo : ScriptableObject
    {
        public string Code;
        public string Title;
        public string Desc;
        [HideInInspector]
        public int Lv = 1;
        [SerializeField]
        private int hp, range;
        [SerializeField]
        [Range(1, 4)]
        private int tier;

        public int Hp
        {
            get
            {
                return (int)(hp * RateMultipleByLv);
            }
        }
        public int Range
        {
            get
            {
                return range;
            }
        }

        /// <summary>
        /// 기물 티어
        /// </summary>
        public int Tier
        {
            get
            {
                return tier;
            }
        }

        /// <summary>
        /// 레벨에 따른 가중치 (레벨 당 1.6배)
        /// </summary>
        public float RateMultipleByLv
        {
            get
            {
                return Mathf.Pow(1.6f, Lv - 1);
            }
        }
        /// <summary>
        /// 기물의 능력들 리스트
        /// </summary>
        public List<AbilityInfo> AbilityInfos;
        public UnitInfo Clone()
        {
            return Instantiate(this);
        }
        public UnitToken GetLiveInfo()
        {
            UnitToken res = CreateInstance<UnitToken>();
            res.Code = Code;
            return res;
        }
    }
}
