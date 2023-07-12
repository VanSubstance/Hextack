using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Window
{
    public class WindowContentResult : AbsWindowContent<ResultInfo>
    {
        [SerializeField]
        private TextMeshProUGUI textRound, textAccuStone, textAccuSteel, textAccuMonster,
            textUpgradeMachine, textUpgradeBio, textUpgradeMagic,
            textDealMachine, textDealBio, textDealMagic,
            textGold, textGear
            ;

        public override AbsWindowContent<ResultInfo> CloseExtra()
        {
            return this;
        }

        public override AbsWindowContent<ResultInfo> Init(ResultInfo parameter)
        {
            textRound.text = $"{ServerData.InGame.CurrentRound - 1} / {ServerData.InGame.MaxRound}";
            textAccuStone.text = string.Format("{0:N0}", ServerData.InGame.AmountStoneUsage);
            textAccuSteel.text = string.Format("{0:N0}", ServerData.InGame.AmountSteelUsage);
            textAccuMonster.text = string.Format("{0:N0}", ServerData.InGame.CountMonsterKill);

            textUpgradeMachine.text = $"Lv {string.Format("{0:N0}", ServerData.InGame.LevelUpgradeTower[Tower.TowerType.Machine])}";
            textUpgradeMagic.text = $"Lv {string.Format("{0:N0}", ServerData.InGame.LevelUpgradeTower[Tower.TowerType.Magic])}";
            textUpgradeBio.text = $"Lv {string.Format("{0:N0}", ServerData.InGame.LevelUpgradeTower[Tower.TowerType.Bio])}";

            textDealMachine.text = $"{string.Format("{0:N0}", ServerData.InGame.AmountDealByCategory[Tower.TowerType.Machine])}";
            textDealMagic.text = $"{string.Format("{0:N0}", ServerData.InGame.AmountDealByCategory[Tower.TowerType.Magic])}";
            textDealBio.text = $"{string.Format("{0:N0}", ServerData.InGame.AmountDealByCategory[Tower.TowerType.Bio])}";

            textGold.text = $"{string.Format("{0:N0}", (ServerData.InGame.AccuGold = ServerData.InGame.CurrentRound * 100))}";
            textGear.text = $"{string.Format("{0:N0}", ServerData.InGame.AccuGear)}";
            gameObject.SetActive(true);
            return this;
        }
    }
}
