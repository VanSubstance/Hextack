using UnityEngine;

namespace Assets.Scripts.UI.Window
{
    internal class WindowFullContainer : SingletonObject<WindowFullContainer>
    {
        /// <summary>
        /// 결과 윈도우 컨텐츠
        /// </summary>
        [SerializeField]
        private WindowContentResult windowResultContent;

        private void Start()
        {
            Close();
        }

        /// <summary>
        /// 타워 정보 오픈
        /// </summary>
        /// <param name="info"></param>
        public void Open(ResultInfo info)
        {
            Close();
            windowResultContent.Init(info);
            gameObject.SetActive(true);
        }

        /// <summary>
        /// 정보창 닫기
        /// </summary>
        public void Close()
        {
            gameObject.SetActive(false);
            windowResultContent.Close();
        }
    }
}
