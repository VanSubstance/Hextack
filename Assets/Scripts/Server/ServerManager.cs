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
        private MapInfo mapInfo;
        [SerializeField]
        private bool isSingle;
        public HexCoordinate[] tilesInfo;
        public UnitToken[][] monstersInfo;
        private int curRound;
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
            curRound = 1;
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
                            InitUnits(curRound - 1);
                            break;
                        case IngameStageType.Place:
                            InitStagePlace();
                            break;
                        case IngameStageType.Applying:
                            InitStageBattle();
                            break;
                        case IngameStageType.Battle:
                            break;
                        case IngameStageType.Result:
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
        /// 적 유닛 생성
        /// </summary>
        /// <param name="idxRound"></param>
        private void InitUnits(int idxRound)
        {
            UnitManager.Instance.InitUnits(monstersInfo[idxRound], true);
            NextStage = IngameStageType.Place;
        }

        /// <summary>
        /// 기물 배치 시작
        /// </summary>
        public void InitStagePlace()
        {
            GlobalStatus.CntInstalled = 0;
            UIManager.Instance.InitChoices();
        }

        /// <summary>
        /// 기물 배치 종료 -> .5f초 대기 후 전투 돌입 하수
        /// </summary>
        public void FinishStagePlace()
        {
            StartCoroutine(CoroutineExecuteAfterWait(() =>
            {
                UIManager.Instance.TextCount = "3";
                StartCoroutine(CoroutineExecuteAfterWait(() =>
                {
                    UIManager.Instance.TextCount = "2";
                    // 사전 효과 선 실행
                    GlobalStatus.UnitsActive.All((unitCtrl) =>
                    {
                        unitCtrl.InitBattle();
                        return true;
                    });
                    StartCoroutine(CoroutineExecuteAfterWait(() =>
                    {
                        UIManager.Instance.TextCount = "1";
                        StartCoroutine(CoroutineExecuteAfterWait(() =>
                        {
                            UIManager.Instance.TextCount = "전투 시작 !";
                            NextStage = IngameStageType.Applying;
                            StartCoroutine(CoroutineExecuteAfterWait(() =>
                            {
                                UIManager.Instance.TextCount = "";
                            }, 1f));
                        }, 1f));
                    }, 1f));
                }, 1f));
            }, .5f));
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

        /// <summary>
        /// 전투 시작 함수
        /// </summary>
        private void InitStageBattle()
        {
            // 모든 기물 전투 상태로 돌입
            GlobalStatus.UnitsActive.All((unitCtrl) =>
            {
                unitCtrl.EnableBattle();
                return true;
            });
            // 전투 상태 체크 함수 실행
            GlobalStatus.InGame.BattleStatus = 0;
            UIManager.Instance.CurTimer = 60;
            StartCoroutine(CoroutineExecuteActionInRepeat(() =>
            {
                GlobalStatus.InGame.BattleStatus = UnitManager.Instance.GetCurrentBattleStatus();
                UIManager.Instance.PassSecond();
            }, () =>
            {
                return GlobalStatus.InGame.BattleStatus != 0;
            }, () =>
            {
                // 전투 종료 시 호출되는 함수
                Debug.Log($"전투 종료!!");
            }, 1f));
        }
    }
}
