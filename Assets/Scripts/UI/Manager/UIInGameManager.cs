using Assets.Scripts.Battle;
using Assets.Scripts.UI.Achievement;
using Assets.Scripts.UI.Window;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Manager
{
    /// <summary>
    /// 인게임 UI 매니저
    /// </summary>
    public class UIInGameManager : SingletonObject<UIInGameManager>
    {
        [SerializeField]
        private TextMeshProUGUI textLife, textRound, textCenter, textStone, textSteel, textWarning, textInfo, textProgress, textMinigLv, textSpeed;
        [SerializeField]
        private GageController gageLife, gageRound;
        [SerializeField]
        private Button btnEarlyStart;
        [SerializeField]
        public AchievementContainer AchievementContainer;

        private int currentLife, currentTimeLeft;
        private Coroutine crTimer;

        /// <summary>
        /// 인게임이 끝났는지 여부
        /// </summary>
        public bool IsInGameOver;

        /// <summary>
        /// 현재 채굴 레벨
        /// </summary>
        public int MiningLv
        {
            set
            {
                textMinigLv.text = $"Lv {value}";
            }
        }

        /// <summary>
        /// 현재 라운드 / 전체 라운드
        /// </summary>
        public int TextProgress
        {
            set
            {
                textProgress.text = $"{value} / {ServerData.InGame.MaxRound}";
            }
        }

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

        public int TextSpeed
        {
            set
            {
                textSpeed.text = $"X {value}";
            }
        }

        /// <summary>
        /// 센터 메세지 출력
        /// </summary>
        public string TextCenter
        {
            set
            {
                textCenter.text = value;
                if (timerTextCenter != null)
                {
                    ServerManager.Instance.StopCoroutine(timerTextCenter);
                }
                timerTextCenter = ServerManager.Instance.ExecuteWithDelay(() =>
                {
                    textCenter.text = string.Empty;
                }, 1f);
            }
        }
        private Coroutine timerTextCenter;

        /// <summary>
        /// 경고 메세지 출력
        /// </summary>
        public string TextWarning
        {
            set
            {
                textWarning.text = value;
                if (timerTextWarning != null)
                {
                    ServerManager.Instance.StopCoroutine(timerTextWarning);
                }
                timerTextWarning = ServerManager.Instance.ExecuteWithDelay(() =>
                {
                    textWarning.text = string.Empty;
                }, 1f);
            }
        }
        private Coroutine timerTextWarning;

        /// <summary>
        /// 정보 메세지 출력
        /// </summary>
        public string TextInfo
        {
            set
            {
                textInfo.text = value;
                if (timerTextInfo != null)
                {
                    ServerManager.Instance.StopCoroutine(timerTextInfo);
                }
                timerTextInfo = ServerManager.Instance.ExecuteWithDelay(() =>
                {
                    textInfo.text = string.Empty;
                }, 1f);
            }
        }
        private Coroutine timerTextInfo;

        /// <summary>
        /// 초기화
        /// </summary>
        public void Init(System.Action _actionWhenRoundTimeDone)
        {
            IsInGameOver = false;
            btnEarlyStart.gameObject.SetActive(false);
            MiningLv = ServerData.InGame.MiningLevel;
            TextCenter = $"던전 시작";
            TextInfo = string.Empty;
            currentLife = 30;
            textLife.text = $"남은 체력: {currentLife}";
            gageLife.Init(30, 30, null, () =>
            {
                // 라이프 다 닳음 = 게임 종료
                ExitGame();
            }, null, new Color(0, 1, .8f, 1));

            ServerData.InGame.CountMonsterLive = 0;
            currentTimeLeft = ServerData.InGame.TimeRound;
            gageRound.Init(ServerData.InGame.TimeRound, 0, null, null, () =>
            {
                _actionWhenRoundTimeDone.Invoke();
                currentTimeLeft = ServerData.InGame.TimeRound;
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
        /// 라운드 시작
        /// </summary>
        public void StartRound()
        {
            TextProgress = ServerData.InGame.CurrentRound;
            if (crTimer != null)
            {
                // 남은 시간 / 10 만큼 추가 인컴 지급
                CommonInGameManager.Instance.AmountStone += currentTimeLeft / 10;
                // 기존 타이머 폐기
                ServerManager.Instance.StopCoroutine(crTimer);
                crTimer = null;
            }
            currentTimeLeft = ServerData.InGame.TimeRound;
            if (ServerData.InGame.CurrentRound == ServerData.InGame.MaxRound)
            {
                // 마지막 라운드
                // 조기 시작 버튼 끄기
                btnEarlyStart.gameObject.SetActive(false);
            }
            else
            {
                btnEarlyStart.gameObject.SetActive(true);
            }
            gageRound.ApplyValue(0, true);
            gageRound.gameObject.SetActive(true);
            crTimer = ServerManager.Instance.ExecuteCrInRepeat(() =>
            {
                PassOneSecond();
            }, null, null, 1f);
        }

        /// <summary>
        /// 게임 종료
        /// </summary>
        public void ExitGame()
        {
            // 타이머 정지 및 파기
            ServerManager.Instance.StopCoroutine(crTimer);
            crTimer = null;

            // 인게임 종료
            IsInGameOver = true;

            // 결과창 열기
            WindowFullContainer.Instance.Open(new ResultInfo()
            {

            });
        }

        private void PassOneSecond()
        {
            currentTimeLeft--;
            textRound.text = $"다음 라운드까지 {currentTimeLeft}초";
            gageRound.ApplyValue(+1);
        }
    }
}
