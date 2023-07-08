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
    }
}
