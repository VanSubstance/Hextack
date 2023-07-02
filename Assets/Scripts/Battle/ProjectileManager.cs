using UnityEngine;

namespace Assets.Scripts.Battle
{
    public class ProjectileManager : SingletonObject<ProjectileManager>
    {
        [SerializeField]
        private ProjectileController projectilePrefab;

        /// <summary>
        /// 투사체 매니저 초기 세팅
        /// </summary>
        public void Init()
        {
            GlobalDictionary.Prefab.Battle.Projectile = projectilePrefab;
            // 풀링: 20개 사전 생성
            for (int i = 0; i < 20; i++)
            {
                GlobalStatus.ProjectilePool.Enqueue(Instantiate(projectilePrefab, transform));
            }
        }

        /// <summary>
        /// 신규 투사체 호출 함수
        /// 풀에 있음 -> 꺼내주기; 없다 -> 생성해서 주기
        /// </summary>
        /// <returns></returns>
        public ProjectileController GetNewProjectile()
        {
            ProjectileController res;
            if ((res = GlobalStatus.GetProjectile()) == null)
            {
                return Instantiate(projectilePrefab, transform);
            }
            return res;
        }
    }
}
