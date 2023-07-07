using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Manager
{
    /// <summary>
    /// 인게임 UI 매니저
    /// </summary>
    public class UIInGameManager : SingletonObject<UIInGameManager>
    {
        [SerializeField]
        private TextMeshProUGUI textLife, textRound, textCenter;
        [SerializeField]
        private GageController gageLife, gageRound;

        private int currentLife;

        /// <summary>
        /// 초기화
        /// </summary>
        public void Init(System.Action _actionWhenRoundTimeDone)
        {
            currentLife = 30;
            gageLife.Init(30, 30, null, () =>
            {
                // 라이프 다 닳음 = 게임 종료

            }, null, new Color(0, 1, .8f, 1));
            gageRound.Init(40, 0, null, null, () =>
            {
                _actionWhenRoundTimeDone.Invoke();
                CancelInvoke("PassOneSecond");
            }, new Color(0, 1, .8f, 1));
        }

        /// <summary>
        /// 체력 차감
        /// </summary>
        /// <param name="isBoss"></param>
        public void ApplyLife(bool isBoss = false)
        {
            currentLife = Mathf.Max(currentLife - (isBoss ? 5 : 1), 0);
            gageLife.ApplyValue(currentLife, true);
        }

        /// <summary>
        /// 40초 카운트
        /// </summary>
        public void StartRound()
        {
            InvokeRepeating("PassOneSecond", 1f, 1f);
        }

        private void PassOneSecond()
        {
            gageRound.ApplyValue(+1);
        }
    }
}
