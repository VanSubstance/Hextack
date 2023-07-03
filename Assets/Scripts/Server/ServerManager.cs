using Assets.Scripts.Battle;
using Assets.Scripts.Map;
using Assets.Scripts.UI;
using Assets.Scripts.Unit;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Server
{
    /// <summary>
    /// 서버 연결 관리용 매니저
    /// </summary>
    public class ServerManager : SingletonObject<ServerManager>
    {
        [SerializeField]
        private string[] testDeck;
        [SerializeField]
        private MapInfo mapInfo;
        [SerializeField]
        private bool isSingle;
        [HideInInspector]
        public HexCoordinate[] tilesInfo;
        public UnitToken[][] monstersInfo;
        private bool IsStageOver
        {
            set
            {
                GlobalStatus.IsInStage = !value;
            }
            get
            {
                return !GlobalStatus.IsInStage;
            }
        }

        private IngameStageType NextStage
        {
            set
            {
                GlobalStatus.CurrentStage = value;
                IsStageOver = true;
            }
        }

        private new void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(transform);
            Application.targetFrameRate = 1000;
            GlobalStatus.MapInfo = mapInfo;
            GlobalStatus.IsSingle = isSingle;
            NextStage = IngameStageType.Prepare;
            string basePath = $"Datas/Maps/{mapInfo.radius}/{mapInfo.mapTitle}";
            tilesInfo = Resources.LoadAll<HexCoordinate>($"{basePath}/installable");
            monstersInfo = new UnitToken[mapInfo.rounds][];
            for (int i = 0; i < mapInfo.rounds; i++)
            {
                monstersInfo[i] = Resources.LoadAll<UnitToken>($"{basePath}/single/rounds/{i + 1}");
            }
            GlobalStatus.InGame.Round = 1;
        }

        private void Start()
        {
            // 타일맵 생성
            MapManager.Instance.Init(tilesInfo);
            // 유닛 내니저 초기화
            UnitManager.Instance.Init();
            // 투사체 매니저 초기화
            ProjectileManager.Instance.Init();

            // 스테이지 관리 코루틴 시작
            StartCoroutine(CoroutineExecuteActionInRepeat(
                () =>
                {
                    if (!IsStageOver) return;
                    IsStageOver = false;
                    switch (GlobalStatus.CurrentStage)
                    {
                        case IngameStageType.Prepare:
                            // 기물 배치 시작
                            // 적 기물 배치
                            InitStagePrepare();
                            break;
                        case IngameStageType.Place:
                            InitStagePlace();
                            break;
                        case IngameStageType.Applying:
                            InitStageApplying();
                            break;
                        case IngameStageType.Battle:
                            InitStageBattle();
                            break;
                        case IngameStageType.Result:
                            InitStageResult();
                            break;
                    }
                },
                () =>
                {
                    return false;
                }, () =>
                {

                },
                1f
                ));
        }

        /// <summary>
        /// 덱 불러오기
        /// 아군 유닛 생성
        /// 적 유닛 생성 
        /// -> 배치 스테이지로 이동
        /// </summary>
        private void InitStagePrepare()
        {
            if (GlobalStatus.Deck == null)
            {
                // 최초 = 덱 초기화
                GlobalStatus.Deck = new UnitInfo[testDeck.Length];
                for (int i = 0; i < testDeck.Length; i++)
                {
                    if (testDeck[i] == null) continue;
                    GlobalStatus.Deck[i] = ServerData.Unit.data[testDeck[i]];
                }
            }
            StartCoroutine(CoroutineExecuteAfterWait(() =>
            {
                UIManager.Instance.TextCenter = $"라운드 {GlobalStatus.InGame.Round}";
                StartCoroutine(CoroutineExecuteAfterWait(() =>
                {
                    UIManager.Instance.TextCenter = "배치";
                    StartCoroutine(CoroutineExecuteAfterWait(() =>
                    {
                        UIManager.Instance.TextCenter = "";
                        UnitManager.Instance.InitUnits(monstersInfo[GlobalStatus.InGame.Round - 1], true);
                        NextStage = IngameStageType.Place;
                    }, 1f));
                }, 1f));
            }, .5f));
        }

        /// <summary>
        /// 배치 스테이지 시작
        /// </summary>
        public void InitStagePlace()
        {
            GlobalStatus.InGame.CntInstalled = 0;
            UIManager.Instance.InitChoices();
        }

        /// <summary>
        /// 배치 스테이지 종료 -> .5f초 대기 후 적용 스테이지로 이동
        /// </summary>
        public void FinishStagePlace()
        {
            StartCoroutine(CoroutineExecuteAfterWait(() =>
            {
                UIManager.Instance.TextCenter = $"배치 종료";
                StartCoroutine(CoroutineExecuteAfterWait(() =>
                {
                    UIManager.Instance.TextCenter = "전투 준비";
                    StartCoroutine(CoroutineExecuteAfterWait(() =>
                    {
                        UIManager.Instance.TextCenter = "2";
                        StartCoroutine(CoroutineExecuteAfterWait(() =>
                        {
                            UIManager.Instance.TextCenter = "1";
                            NextStage = IngameStageType.Applying;
                            StartCoroutine(CoroutineExecuteAfterWait(() =>
                            {
                                UIManager.Instance.TextCenter = "";
                            }, 1f));
                        }, 1f));
                    }, 1f));
                }, 1f));
            }, .5f));
        }

        /// <summary>
        /// 적용 스테이지 시작 -> 전투 스테이지로 이동
        /// </summary>
        private void InitStageApplying()
        {
            // 사전 효과 실행
            GlobalStatus.UnitsActive.All((unitCtrl) =>
            {
                unitCtrl.InitBattle();
                return true;
            });
            // 전투 상태 체크 함수 실행
            GlobalStatus.InGame.BattleStatus = 0;
            UIManager.Instance.CurTimer = 60;
            NextStage = IngameStageType.Battle;
        }

        /// <summary>
        /// 전투 스테이지 시작
        /// </summary>
        private void InitStageBattle()
        {
            // 모든 기물 전투 상태로 돌입
            GlobalStatus.UnitsActive.All((unitCtrl) =>
            {
                unitCtrl.EnableBattle();
                return true;
            });
            StartCoroutine(CoroutineExecuteActionInRepeat(() =>
            {
                GlobalStatus.InGame.BattleStatus = UnitManager.Instance.GetCurrentBattleStatus();
                UIManager.Instance.PassSecond();
            }, () =>
            {
                return GlobalStatus.InGame.BattleStatus != 0;
            }, () =>
            {
                NextStage = IngameStageType.Result;
            }, 1f));
        }

        /// <summary>
        /// 결과 스테이지 시작 -> 준비 스테이지로 이동
        /// </summary>
        private void InitStageResult()
        {
            StartCoroutine(CoroutineExecuteAfterWait(() =>
            {
                UIManager.Instance.TextCenter = "전투 종료";
                StartCoroutine(CoroutineExecuteAfterWait(() =>
                {
                    switch (GlobalStatus.InGame.BattleStatus)
                    {
                        case 1:
                            UIManager.Instance.TextCenter = "승리";
                            break;
                        case 2:
                            UIManager.Instance.TextCenter = "패배";
                            // 체력 깎여야 함
                            break;
                        case 3:
                            UIManager.Instance.TextCenter = "무승부";
                            break;
                    }
                    GlobalStatus.InGame.Round++;
                    // 필드 전부 리셋
                    GlobalStatus.UnitsActive.All((unitCtrl) =>
                    {
                        // 리셋
                        unitCtrl.ReInit();
                        return true;
                    });
                    StartCoroutine(CoroutineExecuteAfterWait(() =>
                    {
                        NextStage = IngameStageType.Prepare;
                    }, 1.5f));
                }, 1.5f));
            }, 0f));
        }

        /// <summary>
        /// 반복 실행 코루틴
        /// </summary>
        /// <param name="actionRepeat"></param>
        /// <param name="actionCondition">true 반환 시 코루틴 강제 종료</param>
        /// <param name="time"></param>
        /// <returns></returns>
        private IEnumerator CoroutineExecuteActionInRepeat(System.Action actionRepeat, System.Func<bool> actionCondition, System.Action actionEscape, float time)
        {
            while (true)
            {
                yield return new WaitForSeconds(time);
                if (actionCondition?.Invoke() == true)
                {
                    actionEscape?.Invoke();
                    yield break;
                }
                actionRepeat?.Invoke();
            }
        }


        /// <summary>
        /// 일정 시간 지연 지정된 함수 실행 코루틴
        /// </summary>
        /// <param name="actionAfter"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        private IEnumerator CoroutineExecuteAfterWait(System.Action actionAfter, float time)
        {
            yield return new WaitForSeconds(time);
            actionAfter?.Invoke();
        }

    }
}
