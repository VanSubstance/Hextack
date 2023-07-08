using UnityEngine;

namespace Assets.Scripts.Battle.Projectile
{
    /// <summary>
    /// 투사체 정보
    /// </summary>
    [System.Serializable]
    public class ProjectileInfo
    {
        /// <summary>
        /// 1회 당 발사하는 투사체 개수; 기본값 = 1
        /// </summary>
        public int CountPerOnce = 1;

        /// <summary>
        /// 투사체가 없는가 ? = 즉발기인가?; 기본값 = false
        /// </summary>
        public bool IsInstant = false;

        /// <summary>
        /// 투사체 속도; 기본값 = 4m/s
        /// </summary>
        public float Spd = 4;

        /// <summary>
        /// 데미지
        /// </summary>
        public int Damage;

        /// <summary>
        /// 쿨타임
        /// </summary>
        public float Cooltime;

        /// <summary>
        /// 사거리
        /// </summary>
        public float Range;

        /// <summary>
        /// 궤적 타입; 기본값 = 직선
        /// </summary>
        public ProjectileTrailType trailType;

        /// <summary>
        /// 투사체 색; 기본값 = 흰색
        /// </summary>
        public Color color;
    }
}
