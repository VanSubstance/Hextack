using Assets.Scripts.Battle;
using Assets.Scripts.Map;
using Assets.Scripts.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Unit
{
    /// <summary>
    /// 인게임 전투를 관리하는 컨트롤러
    /// </summary>
    public class UnitBattleController : MonoBehaviour
    {
        /// <summary>
        /// 실제 전투에 사용하는 데이터
        /// </summary>
        private UnitInfo info;
        private HexCoordinate hexCoor;
        private bool isEnemy;
        private float timeAtk;
        private Coroutine attackCr;
        private Action actionClear;
        private GageController hpGage;

        /// <summary>
        /// 근처 적들 좌표 (배열 기준)
        /// </summary>
        private List<int[]> enemiesNear;
        public UnitController CurTarget;

        /// <summary>
        /// 강제 공격 타겟
        /// </summary>
        private Queue<HexCoordinate> forceTarget;

        private void Awake()
        {
            enabled = false;
            forceTarget = new Queue<HexCoordinate>();
        }

        /// <summary>
        /// 전투 스테이지 시작할 때마다 초기화 함수
        /// </summary>
        /// <param name="unitInfo"></param>
        public void Init(UnitInfo unitInfo, HexCoordinate _coor, bool _isEnemy, Action _actionClear)
        {
            info = unitInfo.Clone();
            hexCoor = _coor;
            isEnemy = _isEnemy;
            actionClear = _actionClear;
            if (info.Spd <= 0)
            {
                timeAtk = -1;
            }
            else
            {
                timeAtk = 1f / info.Spd;
            }
            // 체력 게이지 연결
            if ((hpGage = GlobalStatus.GetHpGageController()) == null)
            {
                hpGage = Instantiate(GlobalDictionary.Prefab.UI.data["Gage"], UIManager.Instance.transform).GetComponent<GageController>();
            }
            hpGage.Init(info.Hp, hexCoor, () =>
            {
                enabled = false;
            });
            // 사전 효과 우선 실행
            ExecuteEffect(true);
            // 이후 공격 코루틴 실행
        }

        /// <summary>
        /// 활성화 함수
        /// </summary>
        public void Enable()
        {
            attackCr = StartCoroutine(CrBatlte());
            enabled = true;
        }

        /// <summary>
        /// 강제로 종료 = 전투 종료 시 호출
        /// </summary>
        public void Disable()
        {
            hpGage.Clear();
            hpGage = null;
            enabled = false;
        }

        /// <summary>
        /// 꺼질 때 기존 info 떨구기 + 기물 효과 취소 + 해당 코루틴 종료 함수 + 오브젝트 풀에 반납
        /// </summary>
        private void OnDisable()
        {
            if (attackCr != null)
            {
                StopCoroutine(attackCr);
                attackCr = null;
            }
            if (hpGage != null)
            {
                // 전투 중 사망
                CancelEffect();
                info = null;
                actionClear?.Invoke();
            }
        }

        /// <summary>
        /// 공격 속도마다 공격 함수를 실행하는 코루틴 함수
        /// </summary>
        /// <returns></returns>
        private IEnumerator CrBatlte()
        {
            if (timeAtk <= 0)
            {
                // 공격 기능이 없다 = 코루틴 자체를 종료
                yield break;
            }
            while (true)
            {
                DecideTarget();
                yield return new WaitForSeconds(timeAtk);
            }
        }

        /// <summary>
        /// 기물 효과 실행 함수
        /// </summary>
        public void ExecuteEffect(bool isTimePrevious)
        {
            foreach (AbilityType abil in info.Abilities)
            {
                switch (abil)
                {
                    case AbilityType.Provoke:
                        // 도발 = 사거리 내 모든 적에게 강제로 타겟 부여
                        if (!isTimePrevious) break;
                        enemiesNear = CommonFunction.SeekCoorsInRange(hexCoor.x, hexCoor.y, hexCoor.z, info.Range, isEnemy ? 2 : 1);
                        foreach (int[] coor in enemiesNear)
                        {
                            GlobalStatus.Units[coor[0]][coor[1]].BattleController.ApplyProvoke(hexCoor);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// 기물 효과 취소 함수
        /// </summary>
        private void CancelEffect()
        {

        }

        /// <summary>
        /// 타겟 설정 함수
        /// </summary>
        private void DecideTarget()
        {
            CurTarget = null;
            while (forceTarget.Count > 0)
            {
                HexCoordinate temp = forceTarget.Peek();
                int[] conv = CommonFunction.ConvertCoordinate(temp);
                if (GlobalStatus.Units[conv[0]][conv[1]].IsLive)
                {
                    // 도발 상태 = 그냥 바로 공격
                    ExecuteAttack(conv[0], conv[1]);
                    return;
                }
                else
                {
                    // 대상이 죽었다 = Dequeue 후 다음 타겟 확인
                    forceTarget.Dequeue();
                }
            }
            // 사거리 안에 있는 가장 가까운 적 식별
            enemiesNear = CommonFunction.SeekCoorsInRange(hexCoor.x, hexCoor.y, hexCoor.z, info.Range, isEnemy ? 2 : 1, true);
            if (enemiesNear.Count != 0)
            {
                ExecuteAttack(enemiesNear[0][0], enemiesNear[0][1]);
            }
        }

        /// <summary>
        /// 배열 좌표 x, y 기물 공격
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void ExecuteAttack(int x, int y)
        {
            CurTarget = GlobalStatus.Units[x][y];
            ProjectileManager.Instance.GetNewProjectile().Init(Color.white, transform.position + Vector3.up, GlobalStatus.Units[x][y].transform.position + Vector3.up, () =>
            {
                try
                {
                    GlobalStatus.Units[x][y].BattleController.ApplyHp(-info.Damage);
                }
                catch (NullReferenceException)
                {
                    // 이미 대상이 죽음
                }
            });
        }

        /// <summary>
        /// 외부로부터의 효과 적용 함수
        /// </summary>
        public void ApplyEffect()
        {

        }

        /// <summary>
        /// 도발 적용
        /// </summary>
        /// <param name="targetCoor"></param>
        public void ApplyProvoke(HexCoordinate targetCoor)
        {
            forceTarget.Enqueue(targetCoor);
        }

        /// <summary>
        /// HP 데미지 적용
        /// </summary>
        /// <param name="amountToApply"></param>
        public void ApplyHp(int amountToApply)
        {
            if (amountToApply < 0)
            {
                // 데미지 텍스트 띄워주기
                UIManager.Instance.GetNewText().Init(hexCoor, $"{Mathf.Abs(amountToApply)}");
            }
            hpGage.ApplyValue(amountToApply);
        }
    }
}
