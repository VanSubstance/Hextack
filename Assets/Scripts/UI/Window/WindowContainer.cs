using UnityEngine;

namespace Assets.Scripts.UI.Window
{
    /// <summary>
    ///  윈도우 컨테이너
    /// </summary>
    public class WindowContainer : SingletonObject<WindowContainer>
    {
        /// <summary>
        /// 타워 정보 윈도우 컨텐츠
        /// </summary>
        [SerializeField]
        private WindowContentTowerInfo windowTowerInfo;

        private void Start()
        {
            Close();
        }

        /// <summary>
        /// 타워 정보 오픈
        /// </summary>
        /// <param name="info"></param>
        public void Open(Tower.TowerInfo info)
        {
            Close();
            windowTowerInfo.Init(info);
            gameObject.SetActive(true);
        }

        /// <summary>
        /// 정보창 닫기
        /// </summary>
        public void Close()
        {
            gameObject.SetActive(false);
            windowTowerInfo.Close();
        }
    }
}
