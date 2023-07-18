using Assets.Scripts.Common.MainManager;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Fragment.Section.GearUpgrade
{
    public class SectionGearUpgradeContent : MonoBehaviour
    {
        [SerializeField]
        private GearUpgradeType upgradeType;
        [SerializeField]
        private TextMeshProUGUI textCurrent, textNext, textPrice;

        private int LevelCurrent
        {
            set
            {
                switch (upgradeType)
                {
                    case GearUpgradeType.Stone:
                        textCurrent.text = $"Lv {value} : {value * 10 + 20}";
                        textNext.text = $"Lv {value + 1} : {(value + 1) * 10 + 20}";
                        break;
                    case GearUpgradeType.Mining:
                        textCurrent.text = $"Lv {value}";
                        textNext.text = $"Lv {value + 1}";
                        break;
                }
            }
        }

        private int Price
        {
            set
            {
                textPrice.text = $"{value} Gear";
            }
        }

        private void Start()
        {
            LevelCurrent = ServerData.Saving.GearUpgradeLevel[upgradeType];
            Price = ServerData.Saving.GearUpgradeLevel[upgradeType] * 2;
        }

        public void Upgrade()
        {
            // 가격은 현재 레벨 * 2로 책정한다
            if (ServerData.Saving.AmountGear < ServerData.Saving.GearUpgradeLevel[upgradeType] * 2)
            {
                // 부족
            }
            else
            {
                // 업그레이드
                CommonOutGameManager.Instance.AmountGear = ServerData.Saving.AmountGear -= ServerData.Saving.GearUpgradeLevel[upgradeType] * 2;
                LevelCurrent = ++ServerData.Saving.GearUpgradeLevel[upgradeType];
                ServerData.Saving.GearLv[(int)upgradeType]++;
                Price = ServerData.Saving.GearUpgradeLevel[upgradeType] * 2;
            }
        }
    }
}
