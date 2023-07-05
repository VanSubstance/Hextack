using Assets.Scripts.UI.Window;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Assets.Scripts.Common.MainManager
{
    public class MainMainManager : SingletonObject<MainMainManager>
    {
        [SerializeField]
        private SwiperController swiper;
        /// <summary>
        /// 시작 프레그먼트 번호
        /// </summary>
        [SerializeField]
        private int startIdx;
        [SerializeField]
        private TextMeshProUGUI textGold, textArtifact, textNick;
        [SerializeField]
        private Image imageUser;

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
            TextNick = ServerData.User.nickName;
            AmountGold = ServerData.User.AmountGold;
            AmountArtifact = ServerData.User.AmountArtifact;
        }
    }
}
