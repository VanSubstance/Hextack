using Assets.Scripts.Unit;
using UnityEngine;
using System.Linq;

namespace Assets.Scripts.UI.Fragment.Storage
{
    public class DeckSectionController : MonoBehaviour, IInitiable
    {
        [SerializeField]
        private UnitStorageController[] socketList = new UnitStorageController[6];
        [SerializeField]
        private DeckLabelController[] deckLabelList = new DeckLabelController[4];

        private void Awake()
        {
            deckLabelList.All((label) =>
            {
                label.onClick.AddListener(() =>
                {
                    SelectDeck(label.Idx);
                });
                return true;
            });
        }

        /// <summary>
        /// 현재 선택한 덱 인덱스
        /// </summary>
        private int currentIdx;

        /// <summary>
        /// 덱 선택
        /// </summary>
        /// <param name="idx"></param>
        public void SelectDeck(int idx)
        {
            currentIdx = idx;
            // 각 소켓 데이터 바꿔쳐주기
            int ii = 0;
            while (ii < 6)
            {
                ChangeUnit(ii, ServerData.User.Decks[idx][ii++]);
            }
        }

        /// <summary>
        /// 기물 해당 위치랑 바꾸기
        /// </summary>
        /// <param name="idx"></param>
        public void ChangeUnit(int idx, UnitInfo newUnitInfo)
        {
            socketList[idx].Init(newUnitInfo);
        }

        /// <summary>
        /// 초기화: 최초 덱 -> 0번으로 고정
        /// </summary>
        public void Init()
        {
            SelectDeck(0);
        }
    }
}
