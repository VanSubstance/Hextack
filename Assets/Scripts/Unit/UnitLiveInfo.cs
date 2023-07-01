using Assets.Scripts.Map;
using UnityEngine;

namespace Assets.Scripts.Unit
{

    [CreateAssetMenu(fileName = "Unit", menuName = "Scriptables/Unit Info", order = int.MaxValue)]
    public class UnitLiveInfo : HexCoordinate
    {
        public string title;
        public new UnitLiveInfo Clone()
        {
            return Instantiate(this);
        }
    }
}
