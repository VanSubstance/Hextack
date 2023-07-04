using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class TextController : MonoBehaviour
    {
        private TextMeshProUGUI ugui;
        private Rigidbody rigid;
        private float originFontsize;

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
            originFontsize = ugui.fontSize;
            Text = string.Empty;
            rigid = GetComponent<Rigidbody>();
        }

        /// <summary>
        /// 초기화 함수
        /// </summary>
        public TextController Init(Vector2 screenPos, string targetText, Color textColor, float time = 1f, float sizeMultiplier = 1)
        {
            GetComponent<RectTransform>().anchoredPosition = screenPos + (Vector2.up * 15);
            Text = targetText;
            ugui.color = textColor;
            ugui.fontSize = originFontsize * sizeMultiplier;
            rigid.AddForce(Vector3.up * 20);
            StartCoroutine(CrTimer(time));
            return this;
        }

        /// <summary>
        /// 풀에 반납
        /// </summary>
        private void Clear()
        {
            Text = string.Empty;
            ugui.color = Color.white;
            ugui.fontSize = originFontsize;
            rigid.velocity = Vector3.zero;
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
