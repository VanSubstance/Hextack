﻿using Assets.Scripts.Tower;
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
            Debug.Log($"");
        }
    }
}
