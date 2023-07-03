using System.Collections;
using TMPro;
using UnityEngine;
using Assets.Scripts.Map;

namespace Assets.Scripts.UI.Text
{
    public class TextController : MonoBehaviour
    {
        private TextMeshProUGUI ugui;

        public string Text
        {
            set
            {
                ugui.text = value;
                if (value.Equals(string.Empty) || value.Equals(""))
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    if (!gameObject.activeSelf)
                    {
                        gameObject.SetActive(true);
                    }
                }
            }
        }

        private void Awake()
        {
            ugui = GetComponent<TextMeshProUGUI>();
            Text = string.Empty;
        }

        /// <summary>
        /// 초기화 함수
        /// </summary>
        /// <param name="_maxValue"></param>
        public TextController Init(HexCoordinate targetCoor, string targetText, float time = 1f)
        {
            int[] cvc = CommonFunction.ConvertCoordinate(targetCoor);
            GetComponent<RectTransform>().anchoredPosition = Camera.main.WorldToScreenPoint(GlobalStatus.Map[cvc[0]][cvc[1]].transform.position) + (Vector3.up * 30);
            Text = targetText;
            StartCoroutine(CrTimer(time));
            return this;
        }

        /// <summary>
        /// 풀에 반납
        /// </summary>
        private void Clear()
        {
            Text = string.Empty;
            GlobalStatus.textPoll.Enqueue(this);
        }

        /// <summary>
        /// 해당 시간 후 텍스트 파기
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private IEnumerator CrTimer(float time)
        {
            yield return new WaitForSeconds(time);
            Clear();
        }
    }
}
