using Assets.Scripts.Map;
using Assets.Scripts.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
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
        private HexCoordinate coor;
        private bool isEnemy;
        private float timeAtk;
        private Coroutine attackCr;
        private Action actionClear;
        private GageController hpGage;

        /// <summary>
        /// 근처 적들 좌표 (배열 기준)
        /// </summary>
        private List<int[]> enemiesNear;

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
            coor = _coor;
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
            hpGage.Init(info.Hp, coor, () =>
            {
                OnDisable();
            });
            // 효과 우선 적용
            ExecuteEffect();
            // 이후 공격 코루틴 실행
            attackCr = StartCoroutine(CrBatlte());
            enabled = true;
        }

        /// <summary>
        /// 꺼질 때 기존 info 떨구기 + 기물 효과 취소 + 해당 코루틴 종료 함수 + 오브젝트 풀에 반납
        /// </summary>
        private void OnDisable()
        {
            CancelEffect();
            if (attackCr != null)
            {
                StopCoroutine(attackCr);
                attackCr = null;
            }
            info = null;
            actionClear?.Invoke();
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
        private void ExecuteEffect()
        {

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
            while (forceTarget.Count > 0)
            {
                HexCoordinate temp = forceTarget.Peek();
                if (GlobalStatus.Units[temp.x][temp.y].IsLive)
                {
                    // 도발 상태 = 그냥 바로 공격
                    ExecuteAttack(temp.x, temp.y);
                    return;
                }
                else
                {
                    // 대상이 죽었다 = Dequeue 후 다음 타겟 확인
                    forceTarget.Dequeue();
                }
            }
            // 사거리 안에 있는 가장 가까운 적 식별
            enemiesNear = CommonFunction.SeekCoorsInRange(coor.x, coor.y, coor.z, info.Range, isEnemy ? 2 : 1, true);
            ExecuteAttack(enemiesNear[0][0], enemiesNear[0][1]);
        }

        /// <summary>
        /// 배열 좌표 x, y 기물 공격
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void ExecuteAttack(int x, int y)
        {

        }

        /// <summary>
        /// 외부로부터의 효과 적용 함수
        /// </summary>
        public void ApplyEffect()
        {

        }
    }
}
