using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.DamageText
{
    public class TextController : AbsPoolingContent<TextInfo>
    {
        private TextMeshProUGUI ugui;
        private Rigidbody rigid;
        private Vector3 originScale;

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
            originScale = transform.localScale;
            Text = string.Empty;
            rigid = GetComponent<Rigidbody>();
        }

        /// <summary>
        /// 해당 시간 후 텍스트 파기
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private IEnumerator CrTimer(float time)
        {
            yield return new WaitForSeconds(time);
            ReturnToPool();
        }

        public override void Clear()
        {
            Text = string.Empty;
            ugui.color = Color.white;
            transform.localScale = originScale;
            rigid.velocity = Vector3.zero;
        }

        protected override bool InitExtra(TextInfo _info)
        {
            transform.position = _info.ScreenPos;
            Text = _info.TargetText;
            ugui.color = _info.TextColor;
            transform.localScale = originScale * _info.SizeMultiplier;
            rigid.AddForce(Vector3.up * 20);
            StartCoroutine(CrTimer(_info.Time));
            return true;
        }
    }
}
