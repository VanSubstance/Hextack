using Assets.Scripts.Battle;
using Assets.Scripts.Unit;
using UnityEngine;

namespace Assets.Scripts.UI.Fragment.Storage
{
    /// <summary>
    /// UnitstorageController 풀링도 겸함
    /// </summary>
    public class StorageFragmentController : SingletonObject<StorageFragmentController>
    {
        [SerializeField]
        private StorageSectionController storageSection;
        [SerializeField]
        private UnitStorageController unitStoragePrefab;

        /// <summary>
        /// 풀링
        /// </summary>
        private new void Awake()
        {
            base.Awake();
            for (int i = 0; i<100; i++)
            {
                GlobalStatus.UnitStoragePool.Enqueue(Instantiate(unitStoragePrefab, transform));
            }
        }

        /// <summary>
        /// 창고 프레그먼트 컨텐츠 초기화
        /// </summary>
        public void Init()
        {
            storageSection.Init();
        }

        /// <summary>
        /// 신규 차옥 유닛 오브젝트 호출 함수
        /// 풀에 있음 -> 꺼내주기; 없다 -> 생성해서 주기
        /// </summary>
        /// <returns></returns>
        public UnitStorageController GetUnitStorage()
        {
            UnitStorageController res;
            if ((res = GlobalStatus.GetUnitStorage()) == null)
            {
                return Instantiate(unitStoragePrefab, transform);
            }
            return res;
        }
    }
}
