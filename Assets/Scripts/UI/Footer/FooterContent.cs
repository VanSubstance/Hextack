using Assets.Scripts.Common.MainManager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Footer
{
    public class FooterContent : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI ugui;
        [SerializeField]
        private Image image;

        private RectTransform rectImage;
        private float originFontSize, originImageSize;

        /// <summary>
        /// 현재 선택되었는지
        /// </summary>
        public bool IsSelected
        {
            set
            {
                if (value)
                {
                    // 하이라이트 = 크기 키우기
                    ugui.rectTransform.sizeDelta = Vector2.up * originFontSize * 1.2f;
                    rectImage.sizeDelta = Vector2.one * originImageSize * 1.2f;
                }
                else
                {
                    // 원상복구
                    ugui.rectTransform.sizeDelta = Vector2.up * originFontSize;
                    rectImage.sizeDelta = Vector2.one * originImageSize;
                }
            }
        }

        private void Awake()
        {
            originFontSize = ugui.rectTransform.sizeDelta.y;
            originImageSize = (rectImage = image.rectTransform).sizeDelta.y;
        }

        /// <summary>
        /// 타겟 프레그먼트로 이동
        /// </summary>
        /// <param name="targetIdx"></param>
        public void GoToFragment(int targetIdx)
        {
            CommonOutGameManager.Instance.FragmentContainer.GoToContent(targetIdx, true);
        }
    }
}
