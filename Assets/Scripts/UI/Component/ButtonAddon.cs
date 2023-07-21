using Assets.Scripts.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Component
{
    [RequireComponent(typeof(Button))]
    public class ButtonAddon : MonoBehaviour
    {
        private Button button;
        private void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                AudioManager.Instance.GetNewContent(new AudioInfo()
                {
                    Clip = ServerData.Sound.ClipForButtonClick,
                    Pos = transform.position,
                });
            });
        }
    }
}
