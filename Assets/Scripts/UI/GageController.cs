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
        private Action callbackWhenZero;
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
        public GageController Init(float _maxValue, HexCoordinate targetCoor, Action _callbackWhenZero = null, bool isStartFromFull = true)
        {
            if (targetCoor != null)
            {
                int[] cvc = CommonFunction.ConvertCoordinate(targetCoor);
                Vector3 hexPos = GlobalStatus.Map[cvc[0]][cvc[1]].transform.position;
                if (Physics.Raycast(hexPos, GlobalDictionary.VectorToScreen, out RaycastHit hit, 40, GlobalDictionary.Layer.UI))
                {
                    GetComponent<RectTransform>().position = hit.point;
                    GetComponent<RectTransform>().anchoredPosition += Vector2.up * 70;
                }
            }
            maxValue = _maxValue;
            curValue = isStartFromFull ? _maxValue : 0;
            Value = curValue / maxValue;
            callbackWhenZero = _callbackWhenZero;
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
            curValue = Mathf.Min(maxValue, amount + curValue);
            Value = curValue / maxValue;
            if (Value <= 0)
            {
                callbackWhenZero?.Invoke();
                gameObject.SetActive(false);
                GlobalStatus.HpGagePool.Enqueue(this);
            }
        }

        /// <summary>
        /// 종료 함수 실행 없이 강제로 풀에 반납
        /// </summary>
        public void Clear()
        {
            gameObject.SetActive(false);
            GlobalStatus.HpGagePool.Enqueue(this);
        }
    }
}
