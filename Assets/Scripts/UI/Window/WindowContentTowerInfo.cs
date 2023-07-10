using Assets.Scripts.Tower;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Window
{
    public class WindowContentTowerInfo : AbsWindowContent<TowerInfo>
    {
        [SerializeField]
        private TextMeshProUGUI textTitle, textDesc;

        [SerializeField]
        private Image image;

        public override AbsWindowContent<TowerInfo> CloseExtra()
        {
            ServerData.InGame.LastTowerClicked = null;
            return this;
        }

        public override AbsWindowContent<TowerInfo> Init(TowerInfo parameter)
        {
            image.sprite = GlobalDictionary.Texture.Tower.data[parameter.Code];
            textTitle.text = parameter.Name;
            textDesc.text = parameter.Desc;
            gameObject.SetActive(true);
            return this;
        }
    }
}
