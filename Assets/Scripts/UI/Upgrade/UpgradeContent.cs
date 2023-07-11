using Assets.Scripts.Tower;
using Assets.Scripts.UI.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Upgrade
{
    public class UpgradeContent : MonoBehaviour
    {
        [SerializeField]
        private TowerType towerType;
        [SerializeField]
        private TextMeshProUGUI textPrice, textLevel;
        [SerializeField]
        private Image image;

        /// <summary>
        /// 레벨 텍스트 업데이트
        /// </summary>
        private int CurrentLevel
        {
            set
            {
                textLevel.text = $"{value} → {value + 1}";
            }
        }

        /// <summary>
        /// 레벨업 가격 업데이트
        /// </summary>
        private int curPrice
        {
            set
            {
                textPrice.text = $"{value}";
            }
        }

        private void Start()
        {
            CurrentLevel = ServerData.InGame.LevelUpgradeTower[towerType];
            curPrice = ServerData.InGame.PriceUpgradeTower[towerType];
        }

        /// <summary>
        /// 레벨업 시도
        /// </summary>
        public void TryUpgrade()
        {
            if (ServerData.InGame.AmountSteel < ServerData.InGame.PriceUpgradeTower[towerType])
            {
                UIInGameManager.Instance.TextWarning = $"철광석이 부족합니다 !";
                return;
            }
            // 업그레이드
            UIInGameManager.Instance.AmountSteel = ServerData.InGame.AmountSteel -= ServerData.InGame.PriceUpgradeTower[towerType];
            CurrentLevel = ++ServerData.InGame.LevelUpgradeTower[towerType];
            curPrice = ++ServerData.InGame.PriceUpgradeTower[towerType];
        }
    }
}
