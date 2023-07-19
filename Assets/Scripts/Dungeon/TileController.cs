using Assets.Scripts.Battle;
using Assets.Scripts.Tower;
using Assets.Scripts.UI.Manager;
using System.Linq;
using UnityEngine;

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
        /// 해당 티어 타워 설치
        /// </summary>
        public void BuildTower(int targetTier)
        {
            // 설치할 타워 티어 확률뽑기
            float randFloat = Random.Range(0f, 1f);
            if (randFloat < .9f)
            {
                // 90% 확률로 목표 티어
            }
            else if (randFloat < .99f)
            {
                // 9% 확률로 티어 +1
                targetTier += 1;
            }
            else if (randFloat < .999f)
            {
                // .9% 확률로 티어 +2
                targetTier += 2;
            }
            else
            {
                // .1% 확률로 티어 +3
                targetTier += 3;
            }
            // 최대 티어 = 4
            targetTier = Mathf.Min(targetTier, 4);
            Debug.Log($"Tier:: {targetTier}");
            // 해당 티어 내에서 랜덤 돌리기
            string[] codeArr = ServerData.Tower.data.Where((pair) =>
            {
                return pair.Value.Tier == targetTier;
            }).Select((pair) => pair.Key).ToArray();
            InstallTower(codeArr[Random.Range(0, codeArr.Length)]);
        }

        /// <summary>
        /// 타워 설치 함수
        /// </summary>
        /// <param name="towerCode"></param>
        public void InstallTower(string towerCode)
        {
            if (towerEquipped == null)
            {
                // 신규 설치
                TowerInfo info = ServerData.Tower.data[towerCode].Clone();
                info.Position = transform.position;
                info.TileInstalled = this;
                towerEquipped = (TowerController)TowerManager.Instance.GetNewContent(info);
            }
        }

        /// <summary>
        /// 장착된 타워 제거 + 풀링 반납
        /// </summary>
        public void RemoveTower()
        {
            TowerManager.Instance.TowerLiveList.Remove(towerEquipped);
            towerEquipped.ReturnToPool();
            towerEquipped = null;
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
                ServerData.InGame.AmountStoneUsage += 10;
                //InstallTower(codeArr[1]);
                BuildTower(1);
                return;
            }
        }
    }
}
