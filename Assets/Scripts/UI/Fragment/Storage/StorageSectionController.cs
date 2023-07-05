using UnityEngine;

namespace Assets.Scripts.UI.Fragment.Storage
{
    public class StorageSectionController : MonoBehaviour
    {
        [SerializeField]
        private StorageByTierController storageByTierPrefab;
        [SerializeField]
        private Transform contentParent;
        private StorageByTierController[] storageByTierList;

        /// <summary>
        /// 티어 별 기물 창고 초기화
        /// </summary>
        public void Init()
        {
            storageByTierList = new StorageByTierController[5];
            for (int i = 1; i < 5; i++)
            {
                storageByTierList[i] = Instantiate(storageByTierPrefab, contentParent).Init(i);
            }
        }
    }
}
