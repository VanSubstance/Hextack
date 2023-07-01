using Assets.Scripts.Unit;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Choice
{
    public class ChoiceController : MonoBehaviour, IDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private Image image;
        private UnitInfo info;
        private UnitController newUnit;
        private void Awake()
        {
            image = transform.GetChild(0).GetComponent<Image>();
        }

        public void Init(UnitInfo _info)
        {
            info = _info;
            if (image == null)
            {
                image = transform.GetChild(0).GetComponent<Image>();
            }
            image.sprite = GlobalDictionary.Texture.Unit.data[_info.Code];
        }

        /// <summary>
        /// 마우스를 따라가는 기물 생성 함수
        /// </summary>
        /// <param name="eventData"></param>
        public void OnDrag(PointerEventData eventData)
        {
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (newUnit == null)
            {
                newUnit = Instantiate(GlobalDictionary.Prefab.Unit.Prefab).Init(info.GetLiveInfo(), false);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (newUnit != null)
            {
                Destroy(newUnit);
            }
        }
    }
}
