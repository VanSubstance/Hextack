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
            textDesc.text = GetFilledDescription(_info);
            gameObject.SetActive(true);
        }

        /// <summary>
        /// 비활성화
        /// </summary>
        public void Clear()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 설명 채워서 반환
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        private string GetFilledDescription(UnitInfo _info)
        {
            string res = _info.Desc;
            float temp = _info.AbilityInfos[0].amount;
            temp *= temp < 1 ? 100 : 1;
            temp = Mathf.Abs(temp);
            res = res.Replace("{amount}", $"{temp}");
            res = res.Replace("{timeCool}", $"{_info.AbilityInfos[0].secondForOnce * _info.RateMultipleByLv}");
            res = res.Replace("\\n", "\n");
            return res;
        }
    }
}
