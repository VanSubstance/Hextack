using Assets.Scripts.Battle.Monster;
using Assets.Scripts.Map;
using Assets.Scripts.Server;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Choice;
using Assets.Scripts.UI.Window;
using Assets.Scripts.UI.Window.Result;
using Assets.Scripts.Unit;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Common.MainManager
{
    public class MainInGameManager : SingletonObject<MainInGameManager>
    {
        [SerializeField]
        private DungeonInfo testDungeon;
        [SerializeField]
        private ChoiceManager choiceManager;
        [SerializeField]
        private Transform textHitParent, rayCaster, allyHpTf, enemyHpTf, allyDeckTf, enemyDeckTf;
        [SerializeField]
        private TextMeshProUGUI textCenter, textTimer, textEnemy, nickNameAlly, nickNameEnemy;
        [SerializeField]
        private InfoController infoController;
        [SerializeField]
        private GageController gageController, roundProgressGage;
        [SerializeField]
        private ResultController resultController;

        private int curTimer;

        /// <summary>
        /// 좌측 닉네임 텍스트
        /// </summary>
        public string NickAlly
        {
            set
            {
                nickNameAlly.text = value;
            }
        }


        /// <summary>
        /// 우측 닉네임 텍스트
        /// </summary>
        public string NickEnemy
        {
            set
            {
                nickNameEnemy.text = value;
            }
        }

        /// <summary>
        /// 가운데 텍스트 변경 setter
        /// </summary>
        public string TextCenter
        {
            set
            {
                textCenter.text = value;
            }
        }
        /// <summary>
        /// 가운데 텍스트 변경 setter
        /// </summary>
        public string TextTimer
        {
            set
            {
                if (value.Equals(""))
                {
                    textTimer.text = "60";
                    return;
                }
                textTimer.text = value;
            }
        }
        /// <summary>
        /// 헤더 적 체력 대체 텍스트 변경 setter
        /// </summary>
        public string TextEnemy
        {
            set
            {
                textEnemy.text = value;
            }
        }

        /// <summary>
        /// 헤더 중간 숫자 텍스트
        /// </summary>
        public int CurTimer
        {
            get
            {
                return curTimer;
            }
            set
            {
                curTimer = value;
                textTimer.text = $"{value}";
            }
        }

        /// <summary>
        /// UI가 레이캐스팅을 해야하는가 ?
        /// </summary>
        public bool IsRayCastable
        {
            set
            {
                rayCaster.gameObject.SetActive(value);
            }
        }
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

        public IngameStageType NextStage
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
            if (ServerData.InGame.DungeonInfo == null)
            {
                ServerData.InGame.DungeonInfo = testDungeon;
            }
            ServerManager.Instance.LoadDungeonInfo();
            choiceManager = Instantiate(choiceManager, transform);
            infoController = Instantiate(infoController, transform);
            resultController = Instantiate(resultController, transform);
            TextCenter = "";
            TextTimer = "0";
            TextEnemy = "";

            IsRayCastable = false;
            GlobalStatus.CurScene = "InGame";
            NextStage = IngameStageType.Prepare;
        }

        private void Start()
        {
            StartDungeon();
        }

        /// <summary>
        /// 던전 라이프사이클 시작
        /// </summary>
        public void StartDungeon()
        {

            GlobalStatus.InGame.Round = 1;
            GlobalStatus.InGame.WinCount = 0;
            GlobalStatus.InGame.AccuGold = 0;

            // 유닛 매니저 초기화
            UnitManager.Instance.Init();
            // UI 매니저 정보 초기화
            Init();

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
                    return !GlobalStatus.CurScene.Equals("InGame");
                }, () =>
                {

                },
                1f
                ));
        }

        /// <summary>
        /// 데이터 업데이트 종료 후 실행되어야 하는 UI 매니저 초기화
        /// </summary>
        public void Init()
        {
            NickAlly = ServerData.User.Base.NickName;
            NickEnemy = ServerData.InGame.DungeonInfo.mapTitle;
            VisualizeDeck(ServerData.InGame.DeckAlly, true);
            VisualizeDeck(ServerData.InGame.DungeonInfo.unitCodeList, false);
            roundProgressGage.Init(ServerData.InGame.DungeonInfo.rounds, 0, null);
        }

        ///// <summary>
        ///// 타이머 1초 진행
        ///// </summary>
        //public void PassSecond()
        //{
        //    CurTimer = curTimer - 1;
        //}

        /// <summary>
        /// 현재 덱 5개 중 2개 띄우기 함수
        /// </summary>
        public void InitChoices()
        {
            choiceManager.PickRandom();
        }

        public void FinishChoice()
        {
            if (++GlobalStatus.InGame.CntInstalled == GlobalStatus.CntUnitSummonAtOnce)
            {
                // 선택 종료 -> .5초 후 전투 시작
                FinishStagePlace();
                choiceManager.gameObject.SetActive(false);
                return;
            }
            choiceManager.PickRandom();
        }

        /// <summary>
        /// 게이지 오브젝트 생성 함수
        /// </summary>
        public GageController GetNewGage()
        {
            GageController res;
            if ((res = GlobalStatus.GetHpGageController()) == null)
            {
                res = Instantiate(gageController, transform);
            }
            return res;
        }

        /// <summary>
        /// 유닛 정보 띄우기
        /// </summary>
        /// <param name="_info"></param>
        public void InitUnitInfo(UnitInfo _info)
        {
            infoController.Init(_info);
        }

        /// <summary>
        /// 체력 차감
        /// </summary>
        public void DeductHP(bool isAlly, int cnt = 1)
        {
            Transform temp;
            if (isAlly)
            {
                while (0 < cnt--)
                {
                    temp = allyHpTf.GetChild(0);
                    temp.gameObject.SetActive(false);
                    temp.SetAsLastSibling();
                }
            }
            else
            {
                while (0 < cnt--)
                {
                    temp = enemyHpTf.GetChild(0);
                    temp.gameObject.SetActive(false);
                    temp.SetAsLastSibling();
                }
            }
        }

        /// <summary>
        /// 유닛 정보 닫기
        /// </summary>
        public void ClearUnitInfo()
        {
            infoController.Clear();
        }

        /// <summary>
        /// 덱 헤더에 연결
        /// </summary>
        /// <param name="deck"></param>
        /// <param name="isAlly"></param>
        public void VisualizeDeck(UnitInfo[] deck, bool isAlly)
        {
            if (isAlly)
            {
                for (int idx = 0; idx < deck.Length; idx++)
                {
                    if (idx >= 6) return;
                    allyDeckTf.GetChild(idx).GetComponent<HeaderUnitController>().Init(deck[idx]);
                }
            }
            else
            {
                for (int idx = 0; idx < deck.Length; idx++)
                {
                    if (idx >= 6) return;
                    enemyDeckTf.GetChild(idx).GetComponent<HeaderUnitController>().Init(deck[idx]);
                }
            }
        }

        /// <summary>
        /// 덱 헤더에 연결
        /// </summary>
        /// <param name="isAlly"></param>
        public void VisualizeDeck(string[] codeList, bool isAlly)
        {
            if (isAlly)
            {
                for (int idx = 0; idx < codeList.Length; idx++)
                {
                    if (idx >= 6) return;
                    allyDeckTf.GetChild(idx).GetComponent<HeaderUnitController>().Init(ServerData.Unit.data[codeList[idx]]);
                }
            }
            else
            {
                for (int idx = 0; idx < codeList.Length; idx++)
                {
                    if (idx >= 6) return;
                    enemyDeckTf.GetChild(idx).GetComponent<HeaderUnitController>().Init(ServerData.Unit.data[codeList[idx]]);
                }
            }
        }

        /// <summary>
        /// 던전 진척도 업데이트
        /// </summary>
        public void UpdateProgress()
        {
            roundProgressGage.ApplyValue(+1);
        }

        /// <summary>
        /// 결과창 열기
        /// </summary>
        public void OpenResult()
        {
            TextCenter = string.Empty;
            resultController.Init();
        }

        /// <summary>
        /// 아군 유닛 생성
        /// 적 유닛 생성 
        /// -> 배치 스테이지로 이동
        /// </summary>
        private void InitStagePrepare()
        {
            StartCoroutine(CoroutineExecuteAfterWait(() =>
            {
                TextCenter = $"라운드 {GlobalStatus.InGame.Round} 시작";
                TextEnemy = $"라운드 {GlobalStatus.InGame.Round}";
                StartCoroutine(CoroutineExecuteAfterWait(() =>
                {
                    TextCenter = "배치";
                    StartCoroutine(CoroutineExecuteAfterWait(() =>
                    {
                        TextCenter = "";
                        UnitManager.Instance.PreviewEnemies(ServerData.InGame.MonsterInfo[GlobalStatus.InGame.Round - 1]);
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
            InitChoices();
        }

        /// <summary>
        /// 배치 스테이지 종료 -> .5f초 대기 후 적용 스테이지로 이동
        /// </summary>
        public void FinishStagePlace()
        {
            StartCoroutine(CoroutineExecuteAfterWait(() =>
            {
                TextCenter = $"배치 종료";
                StartCoroutine(CoroutineExecuteAfterWait(() =>
                {
                    TextCenter = "전투 준비";
                    StartCoroutine(CoroutineExecuteAfterWait(() =>
                    {
                        TextCenter = "2";
                        StartCoroutine(CoroutineExecuteAfterWait(() =>
                        {
                            TextCenter = "1";
                            NextStage = IngameStageType.Applying;
                            StartCoroutine(CoroutineExecuteAfterWait(() =>
                            {
                                TextCenter = "";
                            }, 1f));
                        }, 1f));
                    }, 1f));
                }, 1f));
            }, .5f));
        }

        /// <summary>
        /// 적용 스테이지 시작: 적 유닛 배치 -> 전투 스테이지로 이동
        /// </summary>
        private void InitStageApplying()
        {
            // 사전 효과 실행
            IsRayCastable = true;
            GlobalStatus.UnitsActive.All((unitCtrl) =>
            {
                unitCtrl.InitBattle();
                return true;
            });
            IsRayCastable = false;
            // 전투 상태 체크 함수 실행
            GlobalStatus.InGame.BattleStatus = 0;
            CurTimer = 0;
            NextStage = IngameStageType.Battle;
        }

        /// <summary>
        /// 전투 스테이지 시작
        /// </summary>
        private void InitStageBattle()
        {
            IsRayCastable = true;
            MonsterManager.Instance.SummonMonsters(ServerData.InGame.MonsterInfo[GlobalStatus.InGame.Round - 1]);
            IsRayCastable = false;

            // 모든 기물 전투 상태로 돌입
            GlobalStatus.UnitsActive.All((unitCtrl) =>
            {
                unitCtrl.EnableBattle();
                return true;
            });
            // 전투 종료 조건 판별
            StartCoroutine(CoroutineExecuteActionInRepeat(() =>
            {
                GlobalStatus.InGame.BattleStatus = (CurTimer == 0 ? 1 : 0);
                //PassSecond();
            }, () =>
            {
                return GlobalStatus.InGame.BattleStatus != 0;
            }, () =>
            {
                NextStage = IngameStageType.Result;
            }, 1.1f));
        }

        /// <summary>
        /// 결과 스테이지 시작 -> 준비 스테이지로 이동
        /// </summary>
        private void InitStageResult()
        {
            StartCoroutine(CoroutineExecuteAfterWait(() =>
            {
                TextCenter = "전투 종료";
                StartCoroutine(CoroutineExecuteAfterWait(() =>
                {
                    TextTimer = "";
                    switch (GlobalStatus.InGame.BattleStatus)
                    {
                        case 1:
                            TextCenter = "승리";
                            GlobalStatus.InGame.WinCount++;
                            break;
                        case 2:
                            TextCenter = "패배";
                            // 체력 깎여야 함
                            DeductHP(true);
                            break;
                        case 3:
                            TextCenter = "무승부";
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
                        if (GlobalStatus.InGame.Round > ServerData.InGame.DungeonInfo.rounds)
                        {
                            // 던전 종료 = 결과 페이지로
                            NextStage = IngameStageType.Exit;
                        }
                        else
                        {
                            // 필드 전부 리셋
                            // 진척도 ++
                            UpdateProgress();
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
                TextCenter = "던전 종료";
                StartCoroutine(CoroutineExecuteAfterWait(() =>
                {
                    // 결과 윈도우 보여주기
                    // 보여줄 결과 = 주사위 별 누적 딜량
                    // 승리한 라운드 수
                    // 메인 메뉴로 돌아가기
                    OpenResult();
                }, 1f));
            }, 1f));
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
    }
}
