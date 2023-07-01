using UnityEngine;

namespace Assets.Scripts.Map
{
    [CreateAssetMenu(fileName = "HexCoordinate", menuName = "Scriptables/Hex Tile Coordinate", order = int.MaxValue)]
    [System.Serializable]
    public class HexCoordinate : ScriptableObject
    {
        public int x, y, z;
        public HexCoordinate()
        {

        }
        public HexCoordinate(int _x, int _y, int _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }

        public virtual HexCoordinate Clone()
        {
            return Instantiate(this);
        }

        public HexCoordinate Reverse()
        {
            HexCoordinate res = CreateInstance<HexCoordinate>();
            res.x = -x;
            res.y = -y;
            return res;
        }

        public override string ToString()
        {
            return $"[{x}, {y}, {z}]";
        }
    }
}
