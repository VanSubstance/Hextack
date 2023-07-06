using Assets.Scripts.Unit;
using UnityEngine;

namespace Assets.Scripts.UI.Window
{
    public class WindowController : SingletonObject<WindowController>, IInitiable
    {
        [SerializeField]
        private WindowContentUnitInfoController unitInfoContent;

        public void Init()
        {
            Close();
        }

        /// <summary>
        /// 기물 정보 윈도우 열기
        /// </summary>
        public void OpenUnitInfo(UnitInfo _unitInfo)
        {
            gameObject.SetActive(true);
            unitInfoContent.Init(_unitInfo);
        }

        public void Close()
        {
            gameObject.SetActive(false);
            unitInfoContent.Init(null);
        }
    }
}
