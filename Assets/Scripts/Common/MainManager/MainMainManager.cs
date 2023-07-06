using Assets.Scripts.UI.Window;
using Assets.Scripts.Unit;
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

        [SerializeField]
        private UnitStorageController unitStoragePrefab;

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

        private new void Awake()
        {
            base.Awake();
            for (int i = 0; i < 100; i++)
            {
                GlobalStatus.UnitStoragePool.Enqueue(Instantiate(unitStoragePrefab, transform));
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
            TextNick = ServerData.User.Base.NickName;
            AmountGold = ServerData.User.Base.AmountGold;
            AmountArtifact = ServerData.User.Base.AmountArtifact;

            // 프레그먼트들 초기화
            SwiperController.Instance.Init(startIdx);

            // 윈도우 초기화
            WindowController.Instance.Init();
        }

        /// <summary>
        /// 신규 차옥 유닛 오브젝트 호출 함수
        /// 풀에 있음 -> 꺼내주기; 없다 -> 생성해서 주기
        /// </summary>
        /// <returns></returns>
        public UnitStorageController GetUnitStorage()
        {
            UnitStorageController res;
            if ((res = GlobalStatus.GetUnitStorage()) == null)
            {
                return Instantiate(unitStoragePrefab, transform);
            }
            return res;
        }
    }
}
