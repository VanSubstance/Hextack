using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Window
{
    public class FooterContentController : MonoBehaviour
    {
        [SerializeField]
        private Sprite sprite;
        [SerializeField]
        private string text;
        [SerializeField]
        private TextMeshProUGUI ugui;
        [SerializeField]
        private Image image;

        private Rect rectImage;
        private Vector2 originImageSize;
        private float originFontSize;

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
                    ugui.fontSize = originFontSize * 1.2f;
                    rectImage.size = originImageSize * 1.2f;
                }
                else
                {
                    // 원상복구
                    ugui.fontSize = originFontSize;
                    rectImage.size = originImageSize;
                }
            }
        }

        private void Awake()
        {
            image.sprite = sprite;
            ugui.text = text;
            originFontSize = ugui.fontSize;
            originImageSize = (rectImage = image.rectTransform.rect).size;
        }

        /// <summary>
        /// 타겟 프레그먼트로 이동
        /// </summary>
        /// <param name="targetIdx"></param>
        public void GoToFragment(int targetIdx)
        {
            SwiperController.Instance.GoToFragment(targetIdx, true);
        }
    }
}
