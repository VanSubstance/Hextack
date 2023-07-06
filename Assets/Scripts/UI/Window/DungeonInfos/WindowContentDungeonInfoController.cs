using Assets.Scripts.Map;
using Assets.Scripts.Server;
using Assets.Scripts.UI.Fragment.Storage;
using System.Linq;
using TMPro;
using UnityEngine;
using static UnityEngine.AdaptivePerformance.Provider.AdaptivePerformanceSubsystemDescriptor;

namespace Assets.Scripts.UI.Window.DungeonInfos
{
    public class WindowContentDungeonInfoController : MonoBehaviour, IInitiable<DungeonInfo>
    {
        [SerializeField]
        private TextMeshProUGUI textTitle, textDesc;
        [SerializeField]
        private DeckSectionController deckEnemy, deckAlly;

        private DungeonInfo info;

        public void Init(DungeonInfo param)
        {
            if (param == null)
            {
                gameObject.SetActive(false);
                return;
            }
            info = param;
            textTitle.text = param.mapTitle;
            textDesc.text = param.Desc.Replace("\\n", "\n");
            deckEnemy.IsSimpleInfo = deckAlly.IsSimpleInfo = true;
            deckEnemy.InitMenually(info.unitCodeList.Select((code) =>
            {
                return ServerData.Unit.data[code].Clone();
            }).ToArray());
            deckAlly.SelectDeck(0);
            gameObject.SetActive(true);
        }

        /// <summary>
        /// 전투 시작
        /// </summary>
        public void StartGame()
        {
            //전투 시작
            ServerManager.Instance.EnterDungeon(info.Code);
        }
    }
}
