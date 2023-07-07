using Assets.Scripts.Common.Pooling;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Marker
{
    public class IconController : AbsPoolingContent
    {
        [SerializeField]
        private Image image;

        public override void Clear()
        {
        }

        protected override bool InitExtra(AbsPoolingContent.Info _info)
        {
            if (_info is not Info info)
            {
                return false;
            }
            int[] cvc = CommonFunction.ConvertCoordinate((int)info.HexCoor.x, (int)info.HexCoor.y, 0);
            Vector3 hexPos = GlobalStatus.Map[cvc[0]][cvc[1]].transform.position;
            GetComponent<RectTransform>().position = hexPos + (GlobalDictionary.VectorToScreen * 10);
            image.sprite = GlobalDictionary.Texture.Icon.data[info.Code];
            image.color = info.color;
            gameObject.SetActive(true);
            return true;
        }

        public new class Info : AbsPoolingContent.Info
        {
            /// <summary>
            /// 마커 이미지를 위한 코드
            /// </summary>
            public string Code;
            public Color color;
            public Vector2 HexCoor;
        }
    }
}
