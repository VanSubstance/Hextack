using UnityEngine;

namespace Assets.Scripts.Monster
{

    [CreateAssetMenu(fileName = "MonsterToken", menuName = "Scriptables/Monster/Token", order = int.MaxValue)]
    public class MonsterToken : ScriptableObject
    {
        public string Code;
        public int IdxEnterance;
        public MonsterToken Clone()
        {
            return Instantiate(this);
        }
    }
}
