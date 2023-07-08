using Assets.Scripts.Map;
using Assets.Scripts.UI.Window.DungeonInfos;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.Window
{
    public class WindowController : SingletonObject<WindowController>, IInitiable
    {
        [SerializeField]
        private WindowContentDungeonListController dungeonListContent;
        [SerializeField]
        private WindowContentDungeonInfoController dungeonInfoContent;

        public void Init()
        {
            Close();
        }

        /// <summary>
        /// 던전 리스트 열기
        /// </summary>
        public void OpenDungeonList(List<DungeonInfo> _dungeonInfos)
        {
            Close();
            dungeonListContent.Init(_dungeonInfos);
            gameObject.SetActive(true);
        }

        /// <summary>
        /// 던전 정보 열기
        /// </summary>
        /// <param name="_dungeonInfo"></param>
        public void OpenDungeonInfo(DungeonInfo _dungeonInfo)
        {
            Close();
            dungeonInfoContent.Init(_dungeonInfo);
            gameObject.SetActive(true);
        }

        /// <summary>
        /// 윈도우 닫기
        /// </summary>
        public void Close()
        {
            gameObject.SetActive(false);

            // 던전 리스트 닫기
            dungeonListContent.Init(null);

            // 던전 정보 닫기
            dungeonInfoContent.Init(null);
        }
    }
}
