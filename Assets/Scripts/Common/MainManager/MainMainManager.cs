using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Common.MainManager
{
    public class MainMainManager : SingletonObject<MainMainManager>
    {
        /// <summary>
        /// 시작 프레그먼트 번호
        /// </summary>
        [SerializeField]
        private int startIdx;
        [SerializeField]
        private TextMeshProUGUI textGold, textArtifact, textNick;
        [SerializeField]
        private Image imageUser;

        private bool isTryingEquip;

        /// <summary>
        /// 닉네임 설정
        /// </summary>
        public string TextNick
        {
            set
            {
                textNick.text = value;
            }
        }

        /// <summary>
        /// 골드 설정
        /// </summary>
        public int AmountGold
        {
            set
            {
                textGold.text = string.Format("{0:N0}", value);
            }
        }

        /// <summary>
        /// 아티펙트 개수 설정
        /// </summary>
        public int AmountArtifact
        {
            set
            {
                textArtifact.text = string.Format("{0:N0}", value);
            }
        }

        /// <summary>
        /// 설치를 시도하고 있는가 ?
        /// </summary>
        public bool IsTryingEquip
        {
            get
            {
                return isTryingEquip;
            }
            set
            {
                isTryingEquip = value;
            }
        }

        /// <summary>
        /// 메인 메뉴 시작
        /// </summary>
        private void Start()
        {
            Init();
        }

        /// <summary>
        /// 기본적인 정보 초기화 = 닉네임, 재화 정보
        /// </summary>
        public void Init()
        {
            AmountGold = ServerData.User.AmountGold;
            AmountArtifact = ServerData.User.AmountGear;
        }

        public void ChangeUnit()
        {

        }
    }
}

