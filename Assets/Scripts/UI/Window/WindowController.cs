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
        [SerializeField]
        private WindowContentDungeonInfoController dungeonInfoContent;
        [SerializeField]
        private InfoController unitInfoSimple;

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
        /// 기물 간단 정보 열기
        /// </summary>
        /// <param name="_unitInfo"></param>
        public void OpenUnitInfoSimple(UnitInfo _unitInfo)
        {
            unitInfoSimple.Init(_unitInfo, true);
            gameObject.SetActive(true);
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

            // 던전 리스트 닫기
            dungeonListContent.Init(null);

            // 던전 정보 닫기
            dungeonInfoContent.Init(null);

            // 간단 정보 닫기
            unitInfoSimple.Clear();
        }
    }
}
