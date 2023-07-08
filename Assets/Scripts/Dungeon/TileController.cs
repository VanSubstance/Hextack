using Assets.Scripts.Battle;
using Assets.Scripts.Tower;
using Assets.Scripts.UI.Manager;
using UnityEngine;
using System.Linq;

namespace Assets.Scripts.Dungeon
{
    /// <summary>
    /// 타워 설치용 타일 컨트롤러
    /// </summary>
    public class TileController : MonoBehaviour
    {
        /// <summary>
        /// 설치된 타워
        /// </summary>
        private TowerController towerEquipped;

        /// <summary>
        /// 타워 설치 함수
        /// </summary>
        /// <param name="towerCode"></param>
        public void InstallTower(string towerCode)
        {
            if (towerEquipped == null)
            {
                // 신규 설치
                towerEquipped = TowerManager.Instance.GetNewTower(towerCode, transform.position);
            }
        }

        private void OnMouseDown()
        {
        }

        private void OnMouseUp()
        {
            if (towerEquipped == null)
            {
                // 신규 설치
                if (CommonInGameManager.Instance.AmountStone < 10)
                {
                    UIInGameManager.Instance.TextWarning = $"설치 비용이 부족합니다 !";
                    return;
                }
                // 설치
                // 비용 차감
                CommonInGameManager.Instance.AmountStone -= 10;
                // 설치할 타워 티어 확률뽑기
                float randFloat = Random.Range(0f, 1f);
                int targetTier;
                if (randFloat < .9f)
                {
                    // 90% 확률로 일반 = 티어 1
                    targetTier = 1;
                }
                else if (randFloat < .99f)
                {
                    // 9% 확률로 레어 = 티어 2
                    targetTier = 2;
                }
                else if (randFloat < .999f)
                {
                    // .9% 확률로 유니크 = 티어 3
                    targetTier = 3;
                }
                else
                {
                    // .1% 확률로 에픽 = 티어 4
                    targetTier = 4;
                }

                // 아직 다른 티어 종류 타워가 없다 = 1로 고정
                targetTier = 1;

                // 해당 티어 내에서 랜덤 돌리기
                string[] codeArr = ServerData.Tower.data.Where((pair) =>
                {
                    return pair.Value.Tier == targetTier;
                }).Select((pair) => pair.Key).ToArray();
                InstallTower(codeArr[Random.Range(0, codeArr.Length)]);
                return;
            }
        }
    }
}
