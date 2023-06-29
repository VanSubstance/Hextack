using UnityEngine;

namespace Assets.Scripts.Map
{
    public class HexTileController : MonoBehaviour
    {
        public int x, y;

        public HexTileController Init(int _x, int _y)
        {
            x = _x;
            y = _y;
            return this;
        }
    }
}
