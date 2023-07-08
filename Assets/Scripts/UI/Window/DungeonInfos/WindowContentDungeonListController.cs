using Assets.Scripts.Dungeon;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.Window.DungeonInfos
{
    public class WindowContentDungeonListController : MonoBehaviour, IInitiable<List<DungeonInfo>>
    {
        [SerializeField]
        private ListViewController listView;
        private Queue<DungeonSelectionController> liveComponents;

        /// <summary>
        /// 던전 리스트 초기화
        /// </summary>
        /// <param name="param"></param>
        public void Init(List<DungeonInfo> param)
        {
            if (liveComponents == null)
            {
                liveComponents = new Queue<DungeonSelectionController>();
            }
            else
            {
                while (liveComponents.TryDequeue(out DungeonSelectionController dg))
                {
                    dg.ReturnToPool();
                }
            }
            if (param == null)
            {
                gameObject.SetActive(false);
                return;
            }
            foreach (DungeonInfo d in param)
            {
                liveComponents.Enqueue(listView.GetNewComponent<DungeonSelectionController>().Init(d, (obj) =>
                {
                    obj.gameObject.SetActive(false);
                }));
            }
            gameObject.SetActive(true);
        }
    }
}
