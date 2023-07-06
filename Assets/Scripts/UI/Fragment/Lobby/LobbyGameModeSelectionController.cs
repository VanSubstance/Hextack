using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.Scripts.UI.Window;
using System.Linq;
using Assets.Scripts.Map;

namespace Assets.Scripts.UI.Fragment.Lobby
{
    public class LobbyGameModeSelectionController : MonoBehaviour, IInitiable
    {
        [SerializeField]
        private Button btnSingleNew, btnSingleLoad;
        private TextMeshProUGUI textHistory;
        private string TextHistory
        {
            set
            {
                textHistory.text = value;
            }
        }

        public void Init()
        {
            textHistory = btnSingleLoad.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

            // 만약 이전 진행도가 있다 => 버튼 보여주고 텍스트 바꿔주기
            // 없다 => 걍 버림
            if (true)
            {
                textHistory.text = $"현재 던전 이름";
                btnSingleLoad.gameObject.SetActive(true);
            }
            else
            {
                textHistory.text = string.Empty;
                btnSingleLoad.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 신규 던전 선택창 열기
        /// </summary>
        public void OpenDungeonSelection()
        {
            WindowController.Instance.OpenDungeonList(ServerData.Dungeon.DungeonList.Values.ToList());
        }

        /// <summary>
        /// 이전 진행중인 기록 정보창 열기
        /// </summary>
        public void OpenHistory()
        {
            WindowController.Instance.OpenDungeonInfo(ServerData.Dungeon.History);
        }
    }
}
