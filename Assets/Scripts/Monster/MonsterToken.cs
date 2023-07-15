using UnityEngine;

namespace Assets.Scripts.Monster
{
    /// <summary>
    /// 몬스터 지칭용 토큰 (코드, 생성 위치 명시)
    /// </summary>
    [CreateAssetMenu(fileName = "MonsterToken", menuName = "Scriptables/Monster/Token", order = int.MaxValue)]
    public class MonsterToken : ScriptableObject
    {
        public string Code;
        /// <summary>
        /// 생성 위치
        /// </summary>
        public int IdxEnterance = 0;
        public MonsterToken Clone()
        {
            return Instantiate(this);
        }
    }
}
