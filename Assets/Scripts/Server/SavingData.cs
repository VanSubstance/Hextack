using Assets.Scripts.Tower;
using Assets.Scripts.UI.Fragment.Section.GearUpgrade;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Server
{
    [CreateAssetMenu(fileName = "Saving", menuName = "Scriptables/Data/Basic", order = int.MaxValue)]
    public class SavingData : ScriptableObject
    {
        /// <summary>
        /// 음소거 여부
        /// </summary>
        public bool IsMute;

        /// <summary>
        /// 볼륨
        /// </summary>
        [Range(0, 1)]
        public float Volume;

        /// <summary>
        /// 소지 골드량
        /// </summary>
        public int AmountGold;

        /// <summary>
        /// 소지 기어량
        /// </summary>
        public int AmountGear;

        public int[] GoldLvMachine;
        public int[] GoldLvMagic;
        public int[] GoldLvBio;

        public int[] GearLv;

        /// <summary>
        /// 골드 업그레이드
        /// </summary>
        public Dictionary<TowerType, Dictionary<TowerUpgradeType, int>> GoldUpgradeLevel;

        /// <summary>
        /// 기어 업그레이드
        /// </summary>
        public Dictionary<GearUpgradeType, int> GearUpgradeLevel;
    }
}
