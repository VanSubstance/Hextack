using Assets.Scripts.Battle;
using Assets.Scripts.Common.MainManager;
using Assets.Scripts.Map;
using Assets.Scripts.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public float rateSpeed, rateCritical;
        private List<Coroutine> activeCrList;
        private List<bool> hasTargetList;
        private Action actionClear;
        private GageController hpGage;
        /// <summary>
        /// 기물 파괴 시 실행되어야 할 함수
        /// </summary>
        private System.Action actionWhenDead;

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
        /// 근처 적들 좌표 (배열 기준)
        /// </summary>
        //private List<int[]> targets;
        public bool HasTarget
        {
            get
            {
                foreach (bool hasT in hasTargetList)
                {
                    if (hasT)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

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
            actionWhenDead = () => { };
            actionClear = _actionClear;
            rateCritical = 0;
            rateSpeed = 1;
            // 체력 게이지 연결
            hpGage = MainInGameManager.Instance.GetNewGage();
            hpGage.Init(info.Hp, info.Hp, hexCoor, () =>
            {
                if (_isEnemy)
                {
                    GlobalStatus.InGame.AccuGold += info.Gold;
                    GlobalStatus.InGame.AccuArtifact += (UnityEngine.Random.Range(0f, 1f) < .1f ? 1 : 0);
                }
                enabled = false;
            }, _isEnemy ? null : new Color(0, .9f, .6f, 1));
            // 사전 효과 우선 실행
            ExecutePreviousEffect();
            // 이후 공격 코루틴 실행
        }

        /// <summary>
        /// 찾고자 하는 대상
        /// </summary>
        /// <param name="isTargetAlly">현재 이 기물 기준 아군을 찾는 것인가 ?</param>
        private int SeekTarget(bool isTargetAlly)
        {
            return isEnemy ? (isTargetAlly ? 1 : 2) : (isTargetAlly ? 2 : 1);
        }

        /// <summary>
        /// 활성화 함수
        /// </summary>
        public void Enable()
        {
            activeCrList = new List<Coroutine>();
            hasTargetList = new List<bool>();
            int idx = 0;
            // 지속 효과 실행
            foreach (AbilityInfo ability in info.AbilityInfos)
            {
                if (!ability.isOnce)
                {
                    switch (ability.type)
                    {
                        case AbilityType.Damage:
                        case AbilityType.Heal:
                            hasTargetList.Add(false);
                            activeCrList.Add(StartCoroutine(CrTickEffect((_idx) =>
                            {
                                hasTargetList[_idx] = false;
                                List<int[]> temp = CommonFunction.SeekCoorsInRange(hexCoor.x, hexCoor.y, hexCoor.z, info.Range, SeekTarget(ability.isForAlly), !ability.isBound);
                                if (ability.type.Equals(AbilityType.Damage))
                                {
                                    while (forceTarget.Count > 0)
                                    {
                                        HexCoordinate tempCoor = forceTarget.Peek();
                                        int[] conv = CommonFunction.ConvertCoordinate(tempCoor);
                                        if (GlobalStatus.Units[conv[0]][conv[1]].IsLive)
                                        {
                                            // 도발 상태 = 그냥 바로 공격
                                            hasTargetList[_idx] = true;
                                            ExecuteHp(conv[0], conv[1], ability.amount * info.RateMultipleByLv * (ability.type.Equals(AbilityType.Damage) ? -1 : 1));
                                            break;
                                        }
                                        else
                                        {
                                            // 대상이 죽었다 = Dequeue 후 다음 타겟 확인
                                            forceTarget.Dequeue();
                                        }
                                    }
                                }

                                hasTargetList[_idx] = ability.type.Equals(AbilityType.Damage) && temp.Count > 0;
                                foreach (int[] target in temp)
                                {
                                    ExecuteHp(target[0], target[1], ability.amount * info.RateMultipleByLv * (ability.type.Equals(AbilityType.Damage) ? -1 : 1));
                                }
                            }, idx, ability.secondForOnce)));
                            idx++;
                            break;
                    }
                }
            }
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
            if (activeCrList != null)
            {
                activeCrList.All((cr) =>
                {
                    StopCoroutine(cr);
                    return true;
                });
                activeCrList = null;
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
        /// 쿨타임마다 효과를 실행하는 코루틴
        /// </summary>
        /// <returns></returns>
        private IEnumerator CrTickEffect(System.Action<int> actionToExecute, int idx, float time)
        {
            while (true)
            {
                actionToExecute?.Invoke(idx);
                yield return new WaitForSeconds(time / rateSpeed);
            }
        }

        /// <summary>
        /// 전투 시작 전 적용되어야 할 기물 효과 실행 함수
        /// </summary>
        public void ExecutePreviousEffect()
        {
            List<int[]> temp;
            foreach (AbilityInfo ability in info.AbilityInfos)
            {
                if (ability.isOnce)
                {
                    temp = CommonFunction.SeekCoorsInRange(hexCoor.x, hexCoor.y, hexCoor.z, info.Range, SeekTarget(ability.isForAlly), !ability.isBound);
                    switch (ability.type)
                    {
                        case AbilityType.Provoke:
                            // 도발 = 적에게 강제로 타겟 부여
                            foreach (int[] coor in temp)
                            {
                                GlobalStatus.Units[coor[0]][coor[1]].BattleController.ApplyProvoke(hexCoor);
                            }
                            break;
                        case AbilityType.AttackSpeed:
                            // 공격속도 버프/너프
                            foreach (int[] coor in temp)
                            {
                                GlobalStatus.Units[coor[0]][coor[1]].BattleController.ApplyAtkSpeed(ability.amount * info.RateMultipleByLv);
                                actionWhenDead += () =>
                                {
                                    GlobalStatus.Units[coor[0]][coor[1]].BattleController.ApplyAtkSpeed(-ability.amount * info.RateMultipleByLv);
                                };
                            }
                            break;
                        case AbilityType.RateCritical:
                            // 치명타율 버프/너프
                            foreach (int[] coor in temp)
                            {
                                GlobalStatus.Units[coor[0]][coor[1]].BattleController.ApplyCriticalRate(ability.amount * info.RateMultipleByLv);
                                actionWhenDead += () =>
                                {
                                    GlobalStatus.Units[coor[0]][coor[1]].BattleController.ApplyCriticalRate(-ability.amount * info.RateMultipleByLv);
                                };
                            }
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 기물 효과 취소 함수
        /// </summary>
        private void CancelEffect()
        {
            actionWhenDead?.Invoke();
        }

        /// <summary>
        /// 배열 좌표 x, y 기물 공격
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void ExecuteHp(int x, int y, float amountToApply)
        {
            ProjectileManager.Instance.GetNewProjectile().Init(Color.white, transform.position + Vector3.up, GlobalStatus.Units[x][y].transform.position + Vector3.up, () =>
            {
                try
                {
                    bool isCrit = amountToApply < 0 && UnityEngine.Random.Range(0f, 1f) < (GlobalStatus.InGame.RateCritical + rateCritical);
                    if (!isEnemy)
                    {
                        foreach (UnitInfo _info in ServerData.User.Deck)
                        {
                            if (_info.Code.Equals(info.Code))
                            {
                                _info.AccuDamage += (int)Mathf.Abs(amountToApply * (isCrit ? 1.5f : 1f));
                                break;
                            }
                        }
                    }
                    GlobalStatus.Units[x][y].BattleController.ApplyHp((int)amountToApply, isCrit);
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
        /// 추가 틱 효과 부여
        /// </summary>
        /// <param name="actionToApply"></param>
        /// <param name="time"></param>
        public void ApplyCrTickEffect(System.Action<int> actionToApply, float time)
        {
            hasTargetList.Add(false);
            int newIdx = hasTargetList.Count - 1;
            activeCrList.Add(StartCoroutine(CrTickEffect(actionToApply, newIdx, time)));
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
        /// HP 가감치 적용
        /// </summary>
        /// <param name="amountToApply"></param>
        public void ApplyHp(int amountToApply, bool isCrit)
        {
            if (amountToApply < 0)
            {
                // 데미지 텍스트 띄워주기
                amountToApply = (int)(amountToApply * (isCrit ? 1.5f : 1f));
                MainInGameManager.Instance.GetNewText().Init(screenPos, $"{Mathf.Abs(amountToApply)}", isCrit ? new Color(1, .8f, 0, 1) : Color.white, .5f, 1.3f);

                // 힐 이펙트 띄워주기
                EffectManager.Instance.ExecutNewEffect("Hit", transform.position + (Vector3.up * 2) + Vector3.back, Color.white);
            }
            else
            {
                // 힐 텍스트 띄워주기
                MainInGameManager.Instance.GetNewText().Init(screenPos, $"+{Mathf.Abs(amountToApply)}", new Color(.5f, 1, .8f, 1), .5f);

                // 힐 이펙트 띄워주기
                EffectManager.Instance.ExecutNewEffect("Heal", transform.position + (Vector3.up * 2) + Vector3.back, Color.white);
            }
            hpGage.ApplyValue(amountToApply);
        }
    }
}
