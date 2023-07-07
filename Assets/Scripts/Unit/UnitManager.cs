using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Unit
{
    public class UnitManager : SingletonObject<UnitManager>
    {
        [SerializeField]
        private UnitController prefab;

        public void Init()
        {
            GlobalDictionary.Prefab.Unit.Prefab = prefab;
            GlobalStatus.UnitPool = new Queue<UnitController>();
            GlobalStatus.UnitsActive = new List<UnitController>();
            GlobalStatus.Units = new UnitController[GlobalStatus.Radius * 2 + 1][];
            for (int i = 0; i <= GlobalStatus.Radius * 2; i++)
            {
                GlobalStatus.Units[i] = new UnitController[GlobalStatus.Radius * 2 + 1];
            }
            // 유닛 풀 10개 사전 생성
            GlobalStatus.UnitPool.Enqueue(Instantiate(prefab, transform));
        }

        /// <summary>
        /// 이번 라운드 유닛들 예상 방향 정보 연결 함수
        /// </summary>
        /// <param name="infos"></param>
        public void PreviewEnemies(UnitToken[] infos)
        {
            foreach (UnitToken info in infos)
            {
            }
        }

        /// <summary>
        /// 미리보기 걸려잇는 적들 실제 설치 함수
        /// </summary>
        public void SummonMonster(UnitInfo info, int idxEnterance)
        {
            Debug.Log($"해당 유닛 몬스터들 생성");
        }

        /// <summary>
        /// 유닛 오브젝트 생성 함수
        /// </summary>
        public UnitController GetNewUnit()
        {
            UnitController res;
            if ((res = GlobalStatus.GetUnit()) == null)
            {
                res = Instantiate(prefab, transform);
            }
            return res;
        }

        /// <summary>
        /// 전투가 종료되었는지 확인하는 함수
        /// </summary>
        /// <returns>0: 진행중; 1: 아군 승; 2: 적군 승; 3: 무승부;</returns>
        public int GetCurrentBattleStatus()
        {
            int cntAlly = 0, cntEnemy = 0;
            bool isAllNoTarget = true;
            GlobalStatus.UnitsActive.All((unit) =>
            {
                if (unit.IsLive)
                {
                    if (isAllNoTarget && unit.BattleController.HasTarget)
                    {
                        isAllNoTarget = false;
                    }
                    if (unit.IsEnemy)
                    {
                        cntEnemy++;
                    }
                    else
                    {
                        cntAlly++;
                    }
                }
                return true;
            });
            if (cntEnemy == 0)
            {
                if (cntAlly == 0)
                {
                    // 무승부
                    return 3;
                }
                // 승리
                return 1;
            }
            if (cntAlly == 0)
            {
                // 패배
                return 2;
            }
            if (isAllNoTarget)
            {
                // 교착 상태
                if (cntAlly == cntEnemy)
                {
                    // 무승부
                    return 3;
                }
                if (cntAlly > cntEnemy)
                {
                    // 승리
                    return 1;
                }
                return 2;
            }
            // 양쪽 다 1기 이상의 기물이 남아있음
            return 0;
        }
    }
}
