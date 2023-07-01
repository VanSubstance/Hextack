using UnityEngine;

namespace Assets.Scripts.Unit
{
    public class UnitController : MonoBehaviour
    {
        private UnitLiveInfo info;

        public UnitController Init(UnitLiveInfo _info, bool isEnemy)
        {
            info = _info.Clone();

            return this;
        }
    }
}
