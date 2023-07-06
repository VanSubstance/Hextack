using Assets.Scripts.Common.MainManager;
using Assets.Scripts.Unit;
using System.Linq;
using UnityEngine;

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
            socketList.All((socket) =>
            {
                socket.ActionOnEquip = (targetIdx) =>
                {
                    ChangeUnit(targetIdx, MainMainManager.Instance.CurrentSelectedUnitInfo);
                };
                return true;
            });
        }

        /// <summary>
        /// 덱 선택
        /// </summary>
        /// <param name="idx"></param>
        public void SelectDeck(int idx)
        {
            MainMainManager.Instance.CurrentDeckIdx = idx;
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
            for (int i = 0; i < 6; i++)
            {
                if (ServerData.User.Decks[MainMainManager.Instance.CurrentDeckIdx][i].Code.Equals(newUnitInfo.Code))
                {
                    // 겹치는 애가 있다 = 스왑해야함 i -> idx로, idx -> i로
                    ServerData.User.Decks[MainMainManager.Instance.CurrentDeckIdx][i] = ServerData.User.Decks[MainMainManager.Instance.CurrentDeckIdx][idx];
                    socketList[i].Init(ServerData.User.Decks[MainMainManager.Instance.CurrentDeckIdx][i]);
                    ServerData.User.Decks[MainMainManager.Instance.CurrentDeckIdx][idx] = newUnitInfo;
                    socketList[idx].Init(ServerData.User.Decks[MainMainManager.Instance.CurrentDeckIdx][idx]);
                    return;
                }
            }
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
