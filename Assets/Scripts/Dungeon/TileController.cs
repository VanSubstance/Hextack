using Assets.Scripts.Battle;
using Assets.Scripts.Tower;
using Assets.Scripts.UI.Manager;
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
        /// 타워 설치 함수
        /// </summary>
        /// <param name="towerCode"></param>
        public void InstallTower(string towerCode)
        {

        }

        private void OnMouseDown()
        {
        }

        private void OnMouseUp()
        {
            Debug.Log("클 릭 => 설치");
            Debug.Log($"{CommonInGameManager.Instance.AmountStone}");
            if (CommonInGameManager.Instance.AmountStone < 40)
            {
                UIInGameManager.Instance.TextWarning = $"설치 비용이 부족합니다 !";
            }
        }
    }
}
