using UnityEngine;

namespace Assets.Scripts.Unit
{

    [CreateAssetMenu(fileName = "UnitToken", menuName = "Scriptables/Unit/Token", order = int.MaxValue)]
    public class UnitToken : ScriptableObject
    {
        public string Code;
        public int IdxEnterance;
        public UnitToken Clone()
        {
            return Instantiate(this);
        }
    }
}
