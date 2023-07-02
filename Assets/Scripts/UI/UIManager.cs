using Assets.Scripts.UI.Choice;
using UnityEngine;
using TMPro;
using Assets.Scripts.Server;

namespace Assets.Scripts.UI
{
    public class UIManager : SingletonObject<UIManager>
    {
        [SerializeField]
        private ChoiceManager choiceManager;
        [SerializeField]
        private TextMeshProUGUI textCount;
        /// <summary>
        /// 가운데 텍스트 변경 setter
        /// </summary>
        public string TextCount
        {
            set
            {
                textCount.text = value;
            }
        }
        private new void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this);
            choiceManager = Instantiate(choiceManager, transform);
            TextCount = "";
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
            if (++GlobalStatus.CntInstalled == GlobalStatus.CntUnit)
            {
                // 선택 종료 -> .5초 후 전투 시작
                ServerManager.Instance.FinishStagePlace();
                choiceManager.gameObject.SetActive(false);
                return;
            }
            choiceManager.PickRandom();
        }
    }
}
