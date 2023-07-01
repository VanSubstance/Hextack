using Assets.Scripts.Map;
using UnityEngine;

namespace Assets.Scripts.Unit
{

    [CreateAssetMenu(fileName = "UnitToken", menuName = "Scriptables/Unit Token", order = int.MaxValue)]
    public class UnitToken : HexCoordinate
    {
        public string Title;
        public new UnitToken Clone()
        {
            return Instantiate(this);
        }
    }
}
