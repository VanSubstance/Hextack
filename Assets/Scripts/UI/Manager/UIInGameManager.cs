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
        private TextMeshProUGUI textLife, textRound, textCenter, textStone, textSteel;
        [SerializeField]
        private GageController gageLife, gageRound;

        private int currentLife, currentTimeLeft;
        public int CurrentCountMonster;
        private readonly int Time_Round = 45;

        public int AmountStone
        {
            set
            {
                textStone.text = $"{string.Format("{0:N0}", value)}";
            }
        }

        public int AmountSteel
        {
            set
            {
                textSteel.text = $"{string.Format("{0:N0}", value)}";
            }
        }


        public string TextCenter
        {
            set
            {
                textCenter.text = value;
            }
        }

        /// <summary>
        /// 초기화
        /// </summary>
        public void Init(System.Action _actionWhenRoundTimeDone)
        {
            TextCenter = $"던전 시작";
            currentLife = 30;
            textLife.text = $"남은 체력: {currentLife}";
            gageLife.Init(30, 30, null, () =>
            {
                // 라이프 다 닳음 = 게임 종료

            }, null, new Color(0, 1, .8f, 1));

            CurrentCountMonster = 0;
            currentTimeLeft = Time_Round;
            gageRound.Init(Time_Round, 0, null, null, () =>
            {
                CancelInvoke("PassOneSecond");
                _actionWhenRoundTimeDone.Invoke();
                currentTimeLeft = Time_Round;
                gageRound.ApplyValue(0, true);
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
            textLife.text = $"남은 체력: {currentLife}";
        }

        /// <summary>
        /// 40초 카운트
        /// </summary>
        public void StartRound()
        {
            gageRound.gameObject.SetActive(true);
            InvokeRepeating("PassOneSecond", 1f, 1f);
        }

        private void PassOneSecond()
        {
            currentTimeLeft--;
            textRound.text = $"다음 라운드까지 {currentTimeLeft}초";
            gageRound.ApplyValue(+1);
        }
    }
}
