using UnityEngine;

namespace Assets.Scripts.UI.Window
{
    public class SwiperContentController : MonoBehaviour
    {
        [SerializeField]
        private void Start()
        {
            GlobalStatus.SetSizeSafeArea(GetComponent<RectTransform>());
        }
    }
}
