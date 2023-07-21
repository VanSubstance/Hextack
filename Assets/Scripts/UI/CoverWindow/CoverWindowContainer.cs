using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.CoverWindow
{
    /// <summary>
    /// 뒤 화면 클릭을 막는 전체 윈도우
    /// </summary>
    public class CoverWindowContainer : MonoBehaviour
    {
        /// <summary>
        /// 제목 ugui
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI textTitle;

        /// <summary>
        /// 컨텐츠 부모
        /// </summary>
        [SerializeField]
        private Transform contentParent, viewport;

        /// <summary>
        /// 기본 헤더 높이
        /// </summary>
        private int heightHeader = 160;

        /// <summary>
        /// 컨텐츠 딕셔너리
        /// </summary>
        private Dictionary<CoverWindowContentType, AbsCoverWindowContent> contentDict;

        /// <summary>
        /// 각 컨텐츠 불러와서 연결
        /// </summary>
        private void Awake()
        {
            // 자식들 컨텐츠 불러다가 연결
            contentDict = new Dictionary<CoverWindowContentType, AbsCoverWindowContent>();
            foreach (AbsCoverWindowContent cont in contentParent.GetComponentsInChildren<AbsCoverWindowContent>())
            {
                contentDict.Add(cont.ContentType, cont);
            }
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 해당 타입의 윈도우 열기
        /// </summary>
        /// <param name="type"></param>
        public void Open(ContentType type)
        {
            // 각 타입 별 열려야 하는 컨텐츠 열어주기
            int resHeight = +heightHeader;
            switch (type)
            {
                case ContentType.MenuOutgame:
                    // 음소거, 볼률
                    textTitle.text = $"메인 메뉴";
                    resHeight += ToggleContent(CoverWindowContentType.AudioOnoff, true);
                    resHeight += ToggleContent(CoverWindowContentType.AudioVolume, true);
                    break;
                case ContentType.MenuIngame:
                    // 음소거, 볼륨, 게임 나가기
                    textTitle.text = $"게임 나가기";
                    resHeight += ToggleContent(CoverWindowContentType.AudioOnoff, true);
                    resHeight += ToggleContent(CoverWindowContentType.AudioVolume, true);
                    resHeight += ToggleContent(CoverWindowContentType.IngameQuit, true);
                    break;
            }
            viewport.GetComponent<RectTransform>().sizeDelta = new Vector2(900, resHeight);
            gameObject.SetActive(true);
        }

        public new void SendMessage(string str)
        {
            Open((ContentType)System.Enum.Parse(typeof(ContentType), str));
        }

        /// <summary>
        /// 컨텐츠 토글
        /// </summary>
        /// <param name="isToOpen">true: 열기; false: 닫기</param>
        public int ToggleContent(CoverWindowContentType contentType, bool isToOpen)
        {
            // 닫기 = 닫고 0 반환
            if (!isToOpen)
            {
                contentDict[contentType].gameObject.SetActive(false);
                return 0;
            }
            // 열기 = 열고 높이 반환
            contentDict[contentType].gameObject.SetActive(true);
            return contentDict[contentType].Height;
        }

        /// <summary>
        /// 윈도우 닫기
        /// </summary>
        public void Close()
        {
            // 전부 닫아주기
            foreach (KeyValuePair<CoverWindowContentType, AbsCoverWindowContent> pair in contentDict)
            {
                ToggleContent(pair.Key, false);
            }
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 커버 윈도우 컨텐츠 종류
        /// </summary>
        [System.Serializable]
        public enum ContentType
        {
            /// <summary>
            /// 아웃게임에서 햄버거 > 메뉴
            /// </summary>
            MenuOutgame,
            /// <summary>
            /// 인게임에서 햄버거 > 메뉴
            /// </summary>
            MenuIngame,
        }
    }
}
