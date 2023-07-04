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
        }

        /// <summary>
        /// 던전 정보 받아오기 함수
        /// </summary>
        /// <param name="dungeonName"></param>
        private void LoadDungeonInfo(MapInfo _mapInfo)
        {
            ServerData.Dungeon.Info = _mapInfo;
            string basePath = $"Datas/Maps/{ServerData.Dungeon.Info.radius}/{ServerData.Dungeon.Info.Code}";
            ServerData.Dungeon.TilesInfo = Resources.LoadAll<HexCoordinate>($"{basePath}/installable");
            ServerData.Dungeon.MonsterInfo = new UnitToken[ServerData.Dungeon.Info.rounds][];
            for (int i = 0; i < ServerData.Dungeon.Info.rounds; i++)
            {
                ServerData.Dungeon.MonsterInfo[i] = Resources.LoadAll<UnitToken>($"{basePath}/single/rounds/{i + 1}");
            }
            // 덱 정보 받아오기
            ServerData.User.Deck = new UnitInfo[testDeck.Length];
            for (int i = 0; i < testDeck.Length; i++)
            {
                if (testDeck[i] == null) continue;
                ServerData.User.Deck[i] = ServerData.Unit.data[testDeck[i]];
                ServerData.User.Deck[i].AccuDamage = 0;
                ServerData.User.Deck[i].CountSummon = 0;
            }
        }

        private void Start()
        {
            // 서버 데이터 받아오기
            LoadDungeonInfo(mapInfo);

            GlobalStatus.InGame.Round = 1;
            GlobalStatus.InGame.WinCount = 0;
            GlobalStatus.InGame.AccuGold = 0;

            // 타일맵 생성
            MapManager.Instance.Init();
            // 유닛 매니저 초기화
            UnitManager.Instance.Init();
            // 투사체 매니저 초기화
            ProjectileManager.Instance.Init();
            // UI 매니저 정보 초기화
            UIManager.Instance.Init();

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
                        case IngameStageType.Exit:
                            ExitDungeon();
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
            StartCoroutine(CoroutineExecuteAfterWait(() =>
            {
                UIManager.Instance.TextCenter = $"라운드 {GlobalStatus.InGame.Round} 시작";
                UIManager.Instance.TextEnemy = $"라운드 {GlobalStatus.InGame.Round}";
                StartCoroutine(CoroutineExecuteAfterWait(() =>
                {
                    UIManager.Instance.TextCenter = "배치";
                    StartCoroutine(CoroutineExecuteAfterWait(() =>
                    {
                        UIManager.Instance.TextCenter = "";
                        UnitManager.Instance.InitUnits(ServerData.Dungeon.MonsterInfo[GlobalStatus.InGame.Round - 1], true);
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
            UIManager.Instance.IsRayCastable = true;
            GlobalStatus.UnitsActive.All((unitCtrl) =>
            {
                unitCtrl.InitBattle();
                return true;
            });
            UIManager.Instance.IsRayCastable = false;
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
                    UIManager.Instance.TextTimer = "";
                    switch (GlobalStatus.InGame.BattleStatus)
                    {
                        case 1:
                            UIManager.Instance.TextCenter = "승리";
                            GlobalStatus.InGame.WinCount++;
                            break;
                        case 2:
                            UIManager.Instance.TextCenter = "패배";
                            // 체력 깎여야 함
                            UIManager.Instance.DeductHP(true);
                            break;
                        case 3:
                            UIManager.Instance.TextCenter = "무승부";
                            break;
                    }
                    StartCoroutine(CoroutineExecuteAfterWait(() =>
                    {
                        GlobalStatus.UnitsActive.All((unitCtrl) =>
                        {
                            // 리셋
                            unitCtrl.ReInit();
                            return true;
                        });
                        GlobalStatus.InGame.Round++;
                        if (GlobalStatus.InGame.Round > ServerData.Dungeon.Info.rounds)
                        {
                            // 던전 종료 = 결과 페이지로
                            NextStage = IngameStageType.Exit;
                        }
                        else
                        {
                            // 필드 전부 리셋
                            // 진척도 ++
                            UIManager.Instance.UpdateProgress();
                            NextStage = IngameStageType.Prepare;
                        }
                    }, 1f));
                }, 1.5f));
            }, 0f));
        }

        /// <summary>
        /// 던전 종료 = 결과 보여주기
        /// </summary>
        private void ExitDungeon()
        {
            StartCoroutine(CoroutineExecuteAfterWait(() =>
            {
                UIManager.Instance.TextCenter = "던전 종료";
                StartCoroutine(CoroutineExecuteAfterWait(() =>
                {
                    // 결과 윈도우 보여주기
                    // 보여줄 결과 = 주사위 별 누적 딜량
                    // 승리한 라운드 수
                    // 메인 메뉴로 돌아가기
                    UIManager.Instance.OpenResult();
                }, 1f));
            }, 1f));
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
