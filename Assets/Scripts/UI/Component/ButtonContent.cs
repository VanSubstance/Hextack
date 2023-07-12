using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Component
{
    /// <summary>
    /// 기본 버튼 컨텐츠
    /// </summary>
    public class ButtonContent : MonoBehaviour
    {
        [SerializeField]
        private string textForButton;
        [SerializeField]
        private Color colorForImage = Color.white;
        [SerializeField]
        private TextMeshProUGUI textButton;
        [SerializeField]
        private Image image;


        private void Awake()
        {
            textButton.text = textForButton;
            image.color = colorForImage;
        }
    }
}
