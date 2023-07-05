using Assets.Scripts.Server;
using Assets.Scripts.UI.Choice;
using Assets.Scripts.UI.Window;
using Assets.Scripts.UI.Window.Result;
using Assets.Scripts.Unit;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class InGameUIManager : SingletonObject<InGameUIManager>
    {
        [SerializeField]
        private ChoiceManager choiceManager;
        [SerializeField]
        private UI.TextController textController;
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
        private new void Awake()
        {
            base.Awake();
            choiceManager = Instantiate(choiceManager, transform);
            infoController = Instantiate(infoController, transform);
            resultController = Instantiate(resultController, transform);
            TextCenter = "";
            TextTimer = "";
            TextEnemy = "";

            // 텍스트 풀 100개 사전 생성
            for (int i = 0; i < 100; i++)
            {
                if (1 < 10)
                {
                    GlobalStatus.HpGagePool.Enqueue(Instantiate(gageController, transform));
                }
                GlobalStatus.textPoll.Enqueue(Instantiate(textController, textHitParent));
            }
            IsRayCastable = false;
        }

        /// <summary>
        /// 데이터 업데이트 종료 후 실행되어야 하는 UI 매니저 초기화
        /// </summary>
        public void Init()
        {
            NickAlly = ServerData.User.nickName;
            NickEnemy = ServerData.Dungeon.Info.mapTitle;
            VisualizeDeck(ServerData.User.Deck, true);
            VisualizeDeck(ServerData.Dungeon.Info.unitCodeList, false);
            roundProgressGage.Init(ServerData.Dungeon.Info.rounds, 0, null);
        }

        /// <summary>
        /// 타이머 1초 진행
        /// </summary>
        public void PassSecond()
        {
            CurTimer = curTimer - 1;
        }

        /// <summary>
        /// 현재 덱 5개 중 2개 띄우기 함수
        /// </summary>
        public void InitChoices()
        {
            choiceManager.PickRandom();
        }

        public void FinishChoice()
        {
            if (++GlobalStatus.InGame.CntInstalled == GlobalStatus.CntUnit)
            {
                // 선택 종료 -> .5초 후 전투 시작
                ServerManager.Instance.FinishStagePlace();
                choiceManager.gameObject.SetActive(false);
                return;
            }
            choiceManager.PickRandom();
        }

        /// <summary>
        /// 텍스트 오브젝트 생성 함수
        /// </summary>
        public UI.TextController GetNewText()
        {
            UI.TextController res;
            if ((res = GlobalStatus.GetTextController()) == null)
            {
                res = Instantiate(textController, transform);
            }
            return res;
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
            resultController.Init();
        }
    }
}
