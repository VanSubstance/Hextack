using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Fragment.Section.GoldUpgrade
{
    public class SectionGoldUpgradeChoiceContent : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI textLv, textPrice;

        /// <summary>
        /// 레벨
        /// </summary>
        public int Lv
        {
            set
            {
                textLv.text = $"Lv {value}";
            }
        }

        /// <summary>
        /// 가격
        /// </summary>
        public int Price
        {
            set
            {
                textPrice.text = $"{string.Format("{0:N0}", value)} G";
            }
        }
    }
}
