using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Map;
using UnityEngine;

namespace Assets.Scripts.Monster
{

    [CreateAssetMenu(fileName = "Monster", menuName = "Scriptables/Monster Info", order = int.MaxValue)]
    public class MonsterInfo : HexCoordinate
    {
        public string title;
    }
}
