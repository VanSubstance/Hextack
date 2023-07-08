using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Monster
{
    /// <summary>
    /// 몬스터 풀링 컨텐츠
    /// </summary>
    public class MonsterController : AbsPoolingContent
    {
        public override void Clear()
        {
        }

        protected override bool InitExtra(AbsPoolingContent.Info _info)
        {
            if (_info is not Info info)
            {
                return false;
            }

            return true;
        }

        public new class Info : AbsPoolingContent.Info
        {

        }
    }
}
