using UnityEngine;
using Assets.Scripts.Map;
using Assets.Scripts.Unit;
using System.Collections;
using Assets.Scripts.UI;
using System.Linq;

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
            // 유닛 메니저 초기화
            UnitManager.Instance.Init();

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

        private IEnumerator CoroutineExecuteActionInRepeat(System.Action actionRepeat, System.Func<bool> actionEscape, float time)
        {
            while (true)
            {
                yield return new WaitForSeconds(time);
                if (actionEscape?.Invoke() == true)
                {
                    yield break;
                }
                actionRepeat?.Invoke();
            }
        }

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
            Debug.Log("전투 시작");
            GlobalStatus.UnitsActive.All((unitCtrl) =>
            {
                unitCtrl.InitBattle();
                return true;
            });
        }
    }
}
