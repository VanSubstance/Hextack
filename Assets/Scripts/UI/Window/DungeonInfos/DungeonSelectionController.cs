using Assets.Scripts.Dungeon;
using System;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Window.DungeonInfos
{
    public class DungeonSelectionController : MonoBehaviour, IPoolObject<DungeonSelectionController, DungeonInfo>
    {
        [SerializeField]
        private TextMeshProUGUI textTitle;

        private System.Action<DungeonSelectionController> ActionRetunToPool;
        private DungeonInfo dungeonInfo;

        public DungeonSelectionController Init(DungeonInfo param, Action<DungeonSelectionController> actionReturnToPool)
        {
            dungeonInfo = param;
            ActionRetunToPool = actionReturnToPool;
            textTitle.text = param.mapTitle;
            gameObject.SetActive(true);
            return this;
        }

        public void ReturnToPool()
        {
            ActionRetunToPool?.Invoke(this);
        }

        public void OpenDungeonInfo()
        {
            WindowController.Instance.OpenDungeonInfo(dungeonInfo);
        }
    }
}
