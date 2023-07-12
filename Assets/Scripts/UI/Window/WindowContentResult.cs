using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Window
{
    public class WindowContentResult : AbsWindowContent<ResultInfo>
    {
        [SerializeField]
        private TextMeshProUGUI textRound, textAccuStone, textAccuSteel, textAccuMonster;

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
            gameObject.SetActive(true);
            return this;
        }
    }
}
