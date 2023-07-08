using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Window.Result
{
    public class ResultController : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI textCountWin, textGold, textArtifact;

        private void Start()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 결과창 초기화
        /// </summary>
        public void Init()
        {
            textCountWin.text = $"{ServerData.InGame.CurrentRound} / {ServerData.InGame.MaxRound}";
            textGold.text = $"{ServerData.InGame.AccuGold} G";
            textArtifact.text = $"{ServerData.InGame.AccuArtifact} 개";
            gameObject.SetActive(true);
        }

        /// <summary>
        /// 그냥 수령하고 메인메뉴로 나가기
        /// </summary>
        public void ExitNormal()
        {
            ServerManager.Instance.ExitNormal();
        }

        /// <summary>
        /// 광고 보고 두배 수령하고 메인메뉴로 나가기
        /// </summary>
        public void ExitDouble()
        {
            ServerManager.Instance.ExitDouble();
        }
    }
}
