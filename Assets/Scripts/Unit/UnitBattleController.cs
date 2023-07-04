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
        /// <summary>
        /// 공격 1회에 걸리는 시간, 공격속도 가중치 (기준 1, 분모), 치명타율 가중치 (기준 0, 합)
        /// </summary>
        private float timeAtk, rateSpeed, rateCritical;
        private Coroutine attackCr;
        private Action actionClear;
        private GageController hpGage;

        /// <summary>
        /// 대상이 아군인지 적인지
        /// </summary>
        private bool IsTargetAlly;
        /// <summary>
        /// 스크린 투영 좌표
        /// </summary>
        private Vector2 screenPos
        {
            get
            {
                return hpGage.AnchorPos;
            }
        }

        /// <summary>
        /// 찾고자 하는 대상
        /// </summary>
        private int SeekTarget
        {
            get
            {
                return isEnemy ? (IsTargetAlly ? 1 : 2) : (IsTargetAlly ? 2 : 1);
            }
        }

        /// <summary>
        /// 근처 적들 좌표 (배열 기준)
        /// </summary>
        private List<int[]> targets;
        public bool HasTarget;

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
            IsTargetAlly = info.Abilities.Contains(AbilityType.TargetAlly);
            HasTarget = false;
            actionClear = _actionClear;
            if (info.AtkPerSecond <= 0)
            {
                timeAtk = -1;
            }
            else
            {
                timeAtk = 1f / info.AtkPerSecond;
            }
            rateCritical = 0;
            rateSpeed = 1;
            // 체력 게이지 연결
            hpGage = UIManager.Instance.GetNewGage();
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
                yield return new WaitForSeconds(timeAtk / rateSpeed);
            }
        }

        /// <summary>
        /// 최초 기물 효과 실행 함수
        /// </summary>
        public void ExecuteEffect(bool isTimePrevious)
        {
            targets = CommonFunction.SeekCoorsInRange(hexCoor.x, hexCoor.y, hexCoor.z, info.Range, SeekTarget);
            foreach (AbilityType abil in info.Abilities)
            {
                switch (abil)
                {
                    case AbilityType.Provoke:
                        // 도발 = 사거리 내 모든 적에게 강제로 타겟 부여
                        if (!isTimePrevious) break;
                        foreach (int[] coor in targets)
                        {
                            GlobalStatus.Units[coor[0]][coor[1]].BattleController.ApplyProvoke(hexCoor);
                        }
                        break;
                    case AbilityType.AttackSpeed:
                        // 공격속도 버프/너프
                        if (!isTimePrevious) break;
                        foreach (int[] coor in targets)
                        {
                            GlobalStatus.Units[coor[0]][coor[1]].BattleController.ApplyAtkSpeed(info.RateToAdd);
                        }
                        break;
                    case AbilityType.RateCritical:
                        // 치명타율 버프/너프
                        if (!isTimePrevious) break;
                        foreach (int[] coor in targets)
                        {
                            GlobalStatus.Units[coor[0]][coor[1]].BattleController.ApplyCriticalRate(info.RateToAdd);
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
            foreach (AbilityType abil in info.Abilities)
            {
                switch (abil)
                {
                    case AbilityType.Provoke:
                        // 도발 = 자동으로 해제됨
                        break;
                    case AbilityType.AttackSpeed:
                        // 공격속도 버프/너프 해제
                        foreach (int[] coor in targets)
                        {
                            GlobalStatus.Units[coor[0]][coor[1]].BattleController.ApplyAtkSpeed(-info.RateToAdd);
                        }
                        break;
                    case AbilityType.RateCritical:
                        // 치명타율 버프/너프 해제
                        foreach (int[] coor in targets)
                        {
                            GlobalStatus.Units[coor[0]][coor[1]].BattleController.ApplyCriticalRate(-info.RateToAdd);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// 타겟 설정 함수
        /// </summary>
        private void DecideTarget()
        {
            HasTarget = false;
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
            targets = CommonFunction.SeekCoorsInRange(hexCoor.x, hexCoor.y, hexCoor.z, info.Range, SeekTarget, !info.Abilities.Contains(AbilityType.Bounds));
            foreach (int[] target in targets)
            {
                ExecuteAttack(target[0], target[1]);
            }
        }

        /// <summary>
        /// 배열 좌표 x, y 기물 공격
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void ExecuteAttack(int x, int y)
        {
            HasTarget = true;
            ProjectileManager.Instance.GetNewProjectile().Init(Color.white, transform.position + Vector3.up, GlobalStatus.Units[x][y].transform.position + Vector3.up, () =>
            {
                try
                {
                    GlobalStatus.Units[x][y].BattleController.ApplyHp(-info.Damage, UnityEngine.Random.Range(0f, 1f) < GlobalStatus.InGame.RateCritical + rateCritical);
                }
                catch (NullReferenceException)
                {
                    // 이미 대상이 죽음
                }
            });
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
        /// 공격속도 가감치 합연산 적용
        /// </summary>
        /// <param name="rateToAdd"></param>
        public void ApplyAtkSpeed(float rateToAdd)
        {
            rateSpeed += rateToAdd;
        }

        /// <summary>
        /// 치명타율 가감치 합연산 적용
        /// </summary>
        /// <param name="rateToAdd"></param>
        public void ApplyCriticalRate(float rateToAdd)
        {
            rateCritical += rateToAdd;
        }

        /// <summary>
        /// HP 데미지 적용
        /// </summary>
        /// <param name="amountToApply"></param>
        public void ApplyHp(int amountToApply, bool isCrit)
        {
            if (amountToApply < 0)
            {
                // 데미지 텍스트 띄워주기
                amountToApply = (int)(amountToApply * (isCrit ? 1.5f : 1f));
                UIManager.Instance.GetNewText().Init(screenPos, $"{Mathf.Abs(amountToApply)}", isCrit ? new Color(1, .8f, 0, 1) : Color.white);
            }
            else
            {
                // 힐 텍스트 띄워주기
                UIManager.Instance.GetNewText().Init(screenPos, $"+{Mathf.Abs(amountToApply)}", new Color(.5f, 1, .8f, 1));
            }
            hpGage.ApplyValue(amountToApply);
        }
    }
}
