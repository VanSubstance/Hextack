using Assets.Scripts.Unit;
using System;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Window.Result
{
    public class ResultController : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI textCountWin, textGold;
        [SerializeField]
        private UnitStatisticController[] statControllers;

        private void Start()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 결과창 초기화
        /// </summary>
        public void Init()
        {
            textCountWin.text = $"{GlobalStatus.InGame.WinCount} / {ServerData.Dungeon.Info.rounds}";
            textGold.text = $"{GlobalStatus.InGame.AccuGold} G";
            Array.Sort(ServerData.User.Deck);
            int idx = 0, maxDamage = ServerData.User.Deck[0].AccuDamage;
            foreach (UnitInfo info in ServerData.User.Deck)
            {
                statControllers[idx++].Init(info, maxDamage);
            }
            gameObject.SetActive(true);
        }
    }
}
