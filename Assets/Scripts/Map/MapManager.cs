using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Map
{
    /// <summary>
    /// 반지름만큼 정육각형 타일을 정육각형으로 생성 및 관리하는 매니저
    /// </summary>
    public class MapManager : SingletonObject<MapManager>
    {
        [SerializeField]
        private int radius;
        
        private new void Awake()
        {
            base.Awake();
            // 반지름만큼 생성
        }
    }
}
