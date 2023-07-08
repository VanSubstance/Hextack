using UnityEngine;

namespace Assets.Scripts.Monster
{
    /// <summary>
    /// 몬스터 정보
    /// </summary>
    [CreateAssetMenu(fileName = "MonsterInfo", menuName = "Scriptables/Monster/Info", order = int.MaxValue)]
    public class MonsterInfo : ScriptableObject
    {
        /// <summary>
        /// 코드
        /// </summary>
        [HideInInspector]
        public string Code;

        [HideInInspector]
        /// <summary>
        /// 현재까지 생성된 숫자
        /// </summary>
        public int CntMonsterSummoned = 0;

        /// <summary>
        /// 한번에 생성되는 숫자 = 기본값 : 30
        /// </summary>
        public int CntMonsterMax = 30;

        /// <summary>
        /// 이름
        /// </summary>
        public string Name;

        /// <summary>
        /// 체력
        /// </summary>
        public int Hp;

        /// <summary>
        /// 1초에 이동하는 유니티 미터 = 기본값 : 2
        /// </summary>
        public float Spd = 2;

        public MonsterInfo Clone()
        {
            return Instantiate(this);
        }

        public MonsterInfo SetCode(string code)
        {
            Code = code;
            return this;
        }
    }
}
