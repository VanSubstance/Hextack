using Assets.Scripts.Common.MainManager;
using Assets.Scripts.Tower;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.Fragment.Section.GoldUpgrade
{
    public class SectionGoldUpgradeContent : MonoBehaviour
    {
        [SerializeField]
        private TowerUpgradeType upgradeTarget;
        [SerializeField]
        private SectionGoldUpgradeChoiceContent choiceMachine, choiceMagic, choiceBio;
        private Dictionary<TowerType, SectionGoldUpgradeChoiceContent> choiceList;

        private void Awake()
        {
            choiceList = new Dictionary<TowerType, SectionGoldUpgradeChoiceContent>()
            {
                {TowerType.Machine, choiceMachine },
                {TowerType.Magic, choiceMagic },
                {TowerType.Bio, choiceBio },
            };
            UpdateValue(TowerType.Machine); UpdateValue(TowerType.Magic); UpdateValue(TowerType.Bio);
        }

        private void UpdateValue(TowerType towerType)
        {
            choiceList[towerType].Lv = ServerData.OutGame.GoldUpgradeLevel[towerType][upgradeTarget];
            choiceList[towerType].Price = (1 + (ServerData.OutGame.GoldUpgradeLevel[towerType][upgradeTarget] / 10)) * 1000;
        }

        public void UpgradeMachine()
        {
            if (ServerData.User.AmountGold < (1 + (ServerData.OutGame.GoldUpgradeLevel[TowerType.Machine][upgradeTarget] / 10)) * 1000)
            {

            }
            else
            {
                CommonOutGameManager.Instance.AmountGold = ServerData.User.AmountGold -= (1 + (ServerData.OutGame.GoldUpgradeLevel[TowerType.Machine][upgradeTarget] / 10)) * 1000;
                ServerData.OutGame.GoldUpgradeLevel[TowerType.Machine][upgradeTarget]++;
                UpdateValue(TowerType.Machine);
            }
        }

        public void UpgradeMagic()
        {
            if (ServerData.User.AmountGold < (1 + (ServerData.OutGame.GoldUpgradeLevel[TowerType.Magic][upgradeTarget] / 10)) * 1000)
            {

            }
            else
            {
                CommonOutGameManager.Instance.AmountGold = ServerData.User.AmountGold -= (1 + (ServerData.OutGame.GoldUpgradeLevel[TowerType.Magic][upgradeTarget] / 10)) * 1000;
                ServerData.OutGame.GoldUpgradeLevel[TowerType.Magic][upgradeTarget]++;
                UpdateValue(TowerType.Magic);
            }
        }

        public void UpgradeBio()
        {
            if (ServerData.User.AmountGold < (1 + (ServerData.OutGame.GoldUpgradeLevel[TowerType.Bio][upgradeTarget] / 10)) * 1000)
            {

            }
            else
            {
                CommonOutGameManager.Instance.AmountGold = ServerData.User.AmountGold -= (1 + (ServerData.OutGame.GoldUpgradeLevel[TowerType.Bio][upgradeTarget] / 10)) * 1000;
                ServerData.OutGame.GoldUpgradeLevel[TowerType.Bio][upgradeTarget]++;
                UpdateValue(TowerType.Bio);
            }
        }
    }
}
