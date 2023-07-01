using Assets.Scripts.Unit;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Choice
{
    public class ChoiceController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
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
        /// 마우스 위치 기준 현재 예상 설치 지점 추적 함수
        /// </summary>
        /// <param name="eventData"></param>
        public void OnDrag(PointerEventData eventData)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(eventData.position), out RaycastHit hit, 50f, GlobalDictionary.Layer.Map))
            {
                Debug.Log(hit.transform);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (newUnit == null)
            {
                if ((newUnit = GlobalStatus.GetUnitController()) == null)
                {
                    newUnit = Instantiate(GlobalDictionary.Prefab.Unit.Prefab, UnitManager.Instance.transform);
                }
                newUnit.Init(info.GetLiveInfo(), false);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (newUnit != null)
            {
                newUnit.Clear();
                newUnit = null;
            }
        }
    }
}
