using Assets.Scripts.Unit;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Choice
{
    public class ChoiceController : MonoBehaviour
    {
        private Image image;
        private UnitInfo info;
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
    }
}
