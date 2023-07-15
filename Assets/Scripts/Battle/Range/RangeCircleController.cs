using UnityEngine;

namespace Assets.Scripts.Battle.Range
{
    /// <summary>
    /// 원형 사거리 가시 오브젝트
    /// </summary>
    public class RangeCircleController : SingletonObject<RangeCircleController>
    {
        private new void Awake()
        {
            base.Awake();
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 활성화
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="range"></param>
        public void Activate(Vector3 pos, int range)
        {
            transform.position = new Vector3(pos.x, 1, pos.z);
            transform.localScale = (1 + (range * 2)) * GlobalDictionary.UnitRange * Vector3.one;
            gameObject.SetActive(true);
        }

        /// <summary>
        /// 비활성화
        /// </summary>
        public void Deactivate()
        {
            gameObject.SetActive(false);
        }
    }
}
