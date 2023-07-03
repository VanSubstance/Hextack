using Assets.Scripts.Unit;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Window
{
    public class InfoController : MonoBehaviour
    {
        [SerializeField]
        private Image image;
        [SerializeField]
        private TextMeshProUGUI textTitle, textDesc;
        private void Awake()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 기물 정보 초기화 및 활성화
        /// </summary>
        /// <param name="_info"></param>
        public void Init(UnitInfo _info)
        {
            if (gameObject.activeSelf) return;
            image.sprite = GlobalDictionary.Texture.Unit.data[_info.Code];
            textTitle.text = _info.Title;
            textDesc.text = _info.Desc;
            gameObject.SetActive(true);
        }

        /// <summary>
        /// 비활성화
        /// </summary>
        public void Clear()
        {
            gameObject.SetActive(false);
        }
    }
}
