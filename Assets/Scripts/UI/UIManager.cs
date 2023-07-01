using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.UI.Choice;

namespace Assets.Scripts.UI
{
    public class UIManager : SingletonObject<UIManager>
    {
        [SerializeField]
        private ChoiceManager choiceManager;
        private new void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this);
            choiceManager = Instantiate(choiceManager, transform);
        }

        private void Start()
        {
            InitChoices();
        }

        /// <summary>
        /// 현재 덱 5개 중 2개 띄우기 함수
        /// </summary>
        public void InitChoices()
        {
            GlobalStatus.CntInstalled = 0;
            choiceManager.PickRandom();
        }

        public void FinishChoice()
        {
            if (++GlobalStatus.CntInstalled == 3)
            {
                // 선택 종료 -> .5초 후 전투 시작
                Debug.Log("Finish");
                choiceManager.gameObject.SetActive(false);
                return;
            }
            choiceManager.PickRandom();
        }
    }
}
