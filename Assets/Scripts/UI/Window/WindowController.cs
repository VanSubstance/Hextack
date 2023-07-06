using Assets.Scripts.Common.MainManager;
using Assets.Scripts.Map;
using Assets.Scripts.UI.Window.DungeonInfos;
using Assets.Scripts.UI.Window.UnitInfos;
using Assets.Scripts.Unit;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.Window
{
    public class WindowController : SingletonObject<WindowController>, IInitiable
    {
        [SerializeField]
        private WindowContentUnitInfoController unitInfoContent;
        [SerializeField]
        private WindowContentDungeonListController dungeonListContent;

        public void Init()
        {
            Close();
        }

        /// <summary>
        /// 기물 정보 윈도우 열기
        /// </summary>
        public void OpenUnitInfo(UnitInfo _unitInfo)
        {
            Close();
            unitInfoContent.Init(_unitInfo);
            gameObject.SetActive(true);
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

        public void OpenDungeonInfo(DungeonInfo _dungeonInfo)
        {
            Close();
            Debug.Log($"던전 정보 오ㅡ픈");
        }

        /// <summary>
        /// 윈도우 닫기
        /// </summary>
        public void Close()
        {
            gameObject.SetActive(false);

            // 기물 정보 닫기
            MainMainManager.Instance.CurrentSelectedUnitInfo = null;
            unitInfoContent.Init(null);

            // 던전 정보 닫기
            dungeonListContent.Init(null);
        }
    }
}
