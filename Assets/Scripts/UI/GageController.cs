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

        /// <summary>
        /// 유저가 조작 가능한지
        /// </summary>
        public bool IsInteractable
        {
            set
            {
                gage.interactable = value;
            }
        }

        private Slider gage;
        private float maxValue, curValue;
        private Action callbackWhenZero, callbackWhenFull;
        public Vector2 AnchorPos
        {
            get
            {
                return GetComponent<RectTransform>().anchoredPosition;
            }
        }
        /// <summary>
        /// 값 조정
        /// </summary>
        public float Value
        {
            set
            {
                if (gage == null)
                {
                    gage = GetComponent<Slider>();
                    fill.color = fillColor;
                    background.color = backgroudColor;
                }
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
            IsInteractable = false;
            fill.color = fillColor;
            background.color = backgroudColor;
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 초기화 함수
        /// </summary>
        /// <param name="_maxValue"></param>
        public GageController Init(float _maxValue, float _initValue, Transform targetTr, Action _callbackWhenZero = null, Action _callbackWhenFull = null, Color? fillColor = null)
        {
            if (fillColor != null)
            {
                fill.color = (Color)fillColor;
            }
            else
            {
                fill.color = Color.red;
            }
            if (targetTr != null)
            {
                GetComponent<RectTransform>().position = targetTr.position + (GlobalDictionary.VectorToScreen * 10);
                GetComponent<RectTransform>().anchoredPosition += Vector2.up * 100;
            }
            maxValue = _maxValue;
            curValue = _initValue;
            Value = curValue / maxValue;
            callbackWhenZero = _callbackWhenZero;
            callbackWhenFull = _callbackWhenFull;
            gameObject.SetActive(true);
            return this;
        }

        /// <summary>
        /// 값 더하기/빼기 함수; 값이 0이 되면 콜백 함수 작동 + 풀에 반납
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="isFixValue">연산 적용이 아닌 통째로 적용일 경우</param>
        public void ApplyValue(float amount, bool isFixValue = false)
        {
            curValue = isFixValue ? amount : Mathf.Min(maxValue, amount + curValue);
            Value = curValue / maxValue;
            if (Value <= 0)
            {
                callbackWhenZero?.Invoke();
                gameObject.SetActive(false);
            }
            if (Value == 1)
            {
                callbackWhenFull?.Invoke();
            }
        }

        /// <summary>
        /// 종료 함수 실행 없이 강제로 풀에 반납
        /// </summary>
        public void Clear()
        {
            callbackWhenZero = null;
            gameObject.SetActive(false);
            GlobalStatus.HpGagePool.Enqueue(this);
        }
    }
}
