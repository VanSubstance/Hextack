using Assets.Scripts.Server;
using Assets.Scripts.UI.Choice;
using Assets.Scripts.UI.Window;
using Assets.Scripts.Unit;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class UIManager : SingletonObject<UIManager>
    {
        [SerializeField]
        private ChoiceManager choiceManager;
        [SerializeField]
        private UI.TextController textController;
        [SerializeField]
        private Transform textHitParent, rayCaster;
        [SerializeField]
        private TextMeshProUGUI textCenter, textTimer;
        [SerializeField]
        private InfoController infoController;
        private int curTimer;
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
                textTimer.text = value;
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
            DontDestroyOnLoad(this);
            choiceManager = Instantiate(choiceManager, transform);
            infoController = Instantiate(infoController, transform);
            TextCenter = "";
            TextTimer = "";

            // 텍스트 풀 100개 사전 생성
            for (int i = 0; i < 100; i++)
            {
                GlobalStatus.textPoll.Enqueue(Instantiate(textController, textHitParent));
            }
            IsRayCastable = false;
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
        /// 유닛 정보 띄우기
        /// </summary>
        /// <param name="_info"></param>
        public void InitUnitInfo(UnitInfo _info)
        {
            infoController.Init(_info);
        }

        /// <summary>
        /// 유닛 정보 닫기
        /// </summary>
        public void ClearUnitInfo()
        {
            infoController.Clear();
        }
    }
}
