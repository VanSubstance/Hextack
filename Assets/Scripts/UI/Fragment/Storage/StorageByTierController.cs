using Assets.Scripts.Common.MainManager;
using Assets.Scripts.Unit;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Fragment.Storage
{
    public class StorageByTierController : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI textTitle;
        [SerializeField]
        private Transform unitStorageParent;
        private List<UnitInfo>[] units;

        /// <summary>
        /// 해당 티어 초기화
        /// </summary>
        /// <param name="targetIdx"></param>
        public StorageByTierController Init(int tier)
        {
            if (units == null)
            {
                // 데이터부터 정리
                units = new List<UnitInfo>[5];
                for (int i = 1; i < 5; i++)
                {
                    units[i] = new List<UnitInfo>();
                }
                // 분류
                foreach (UnitInfo unitInfo in ServerData.User.Storages)
                {
                    units[unitInfo.Tier].Add(unitInfo);
                }
            }
            switch (tier)
            {
                case 1:
                    textTitle.text = $"일반";
                    break;
                case 2:
                    textTitle.text = $"레어";
                    break;
                case 3:
                    textTitle.text = $"유니크";
                    break;
                case 4:
                    textTitle.text = $"에픽";
                    break;
            }
            foreach (UnitInfo unitInfo in units[tier])
            {
                MainMainManager.Instance.GetUnitStorage().Init(unitInfo, unitStorageParent);
            }
            return this;
        }
    }
}
