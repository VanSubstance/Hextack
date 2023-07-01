using Assets.Scripts.Map;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class GageController : MonoBehaviour
    {
        [SerializeField]
        private Image fill, background;
        [ColorUsage(showAlpha: true, order = int.MaxValue)]
        [SerializeField]
        private Color fillColor, backgroudColor;

        private Slider gage;
        private float maxValue, curValue;
        private Action callbackWhenDone;
        /// <summary>
        /// 값 조정
        /// </summary>
        public float Value
        {
            set
            {
                gage.value = value;
            }
            get
            {
                return gage.value;
            }
        }
        private void Awake()
        {
            gage = GetComponent<Slider>();
            fill.color = fillColor;
            background.color = backgroudColor;
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 초기화 함수
        /// </summary>
        /// <param name="_maxValue"></param>
        public GageController Init(float _maxValue, HexCoordinate targetCoor, Action _callbackWhenDone = null)
        {
            int[] cvc = CommonFunction.ConvertCoordinate(targetCoor);
            GetComponent<RectTransform>().anchoredPosition = Camera.main.WorldToScreenPoint(GlobalStatus.Map[cvc[0]][cvc[1]].transform.position) + (Vector3.up * 25);
            maxValue = _maxValue;
            curValue = _maxValue;
            Value = maxValue / curValue;
            callbackWhenDone = _callbackWhenDone;
            gameObject.SetActive(true);
            return this;
        }

        /// <summary>
        /// 값 더하기/빼기 함수; 값이 0이 되면 콜백 함수 작동 + 풀에 반납
        /// </summary>
        /// <param name="amount"></param>
        public void ApplyValue(float amount)
        {
            curValue = Mathf.Min(maxValue, amount + curValue);
            Value = maxValue / curValue;
            if (Value <= 0)
            {
                callbackWhenDone?.Invoke();
                gameObject.SetActive(false);
                GlobalStatus.HpGagePool.Enqueue(this);
            }
        }
    }
}
