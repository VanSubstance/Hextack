using System.Collections.Generic;
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
        private int hp, damage, range;
        /// <summary>
        /// 1초 당 공격 횟수
        /// </summary>
        [SerializeField]
        private float atkPerSecond;
        /// <summary>
        /// 적용할 가/감 계수 (합연산)
        /// </summary>
        [SerializeField]
        private float rateToAdd;

        public int Hp
        {
            get
            {
                return (int)(hp * Mathf.Pow(1.6f, Lv - 1));
            }
        }
        public int Damage
        {
            get
            {
                return (int)(damage * Mathf.Pow(1.6f, Lv - 1));
            }
        }
        public int Range
        {
            get
            {
                return range;
            }
        }
        public float AtkPerSecond
        {
            get
            {
                return atkPerSecond;
            }
        }
        public float RateToAdd
        {
            get
            {
                return rateToAdd;
            }
        }
        public bool IsAttackable;
        public List<AbilityType> Abilities;
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
