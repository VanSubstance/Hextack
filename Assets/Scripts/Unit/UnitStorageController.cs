using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Unit
{
    public class UnitStorageController : MonoBehaviour
    {
        private UnitInfo unitInfo;
        private Image image;

        private void Awake()
        {
            image = GetComponent<Image>();
            gameObject.SetActive(false);
        }

        public UnitStorageController Init(UnitInfo _unitInfo, Transform parent)
        {
            unitInfo = _unitInfo;
            image.sprite = GlobalDictionary.Texture.Unit.data[unitInfo.Code];
            transform.SetParent(parent);
            gameObject.SetActive(true);
            return this;
        }

        public void OnClick()
        {
            Debug.Log($"{unitInfo.Title}");
        }
    }
}
