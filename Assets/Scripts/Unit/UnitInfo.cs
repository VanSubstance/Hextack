using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Unit
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "UnitInfo", menuName = "Scriptables/Unit/Info", order = int.MaxValue)]
    public class UnitInfo : ScriptableObject, IComparable
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

        /// <summary>
        /// 이번 라운드에 소환한 몬스터 수, 소환 입구
        /// </summary>
        [HideInInspector]
        public int CntMonsterSummoned, IdxEnterance;

        /// <summary>
        /// 한 라운드에 소환하는 몬스터 수
        /// </summary>
        public int CntMonsterMax;

        /// <summary>
        /// 소환 사이 시간
        /// </summary>
        public float TimeMarginSummon;

        /// <summary>
        /// 획득량
        /// </summary>
        public int Gold;

        /// <summary>
        /// 이번 던전 누적 데미지
        /// </summary>
        [HideInInspector]
        public int AccuDamage;

        /// <summary>
        /// 이번 던전에서 사용한 횟수
        /// </summary>
        [HideInInspector]
        public int CountSummon;

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

        public int CompareTo(object obj)
        {
            UnitInfo comp = obj as UnitInfo;
            return comp.AccuDamage.CompareTo(this.AccuDamage);
        }
    }
}
