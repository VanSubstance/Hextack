using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class TextController : AbsPoolingContent
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
        /// 해당 시간 후 텍스트 파기
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private IEnumerator CrTimer(float time)
        {
            yield return new WaitForSeconds(time);
            ReturnToPool();
        }

        protected override bool InitExtra(AbsPoolingContent.Info _info)
        {
            if (_info is not Info info)
                return false;
            transform.position = info.ScreenPos;
            Text = info.TargetText;
            ugui.color = info.TextColor;
            ugui.fontSize = originFontsize * info.SizeMultiplier;
            rigid.AddForce(Vector3.up * 20);
            StartCoroutine(CrTimer(info.Time));
            return true;
        }

        public override void Clear()
        {
            Text = string.Empty;
            ugui.color = Color.white;
            ugui.fontSize = originFontsize;
            rigid.velocity = Vector3.zero;
        }

        public new class Info : AbsPoolingContent.Info
        {
            public Vector3 ScreenPos;
            public string TargetText;
            public Color TextColor;
            public float Time = 1f;
            public float SizeMultiplier = 1;
        }
    }
}
