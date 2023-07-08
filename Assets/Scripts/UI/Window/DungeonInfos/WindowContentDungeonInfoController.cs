using Assets.Scripts.Map;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Window.DungeonInfos
{
    public class WindowContentDungeonInfoController : MonoBehaviour, IInitiable<DungeonInfo>
    {
        [SerializeField]
        private TextMeshProUGUI textTitle, textDesc;

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
