using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Battle
{
    public class EffectManager : SingletonObject<EffectManager>
    {
        /// <summary>
        /// 신규 이펙트 실행
        /// </summary>
        /// <param name="effectName"></param>
        /// <param name="targetPos"></param>
        public void ExecutNewEffect(string effectName, Vector3 targetPos)
        {
            if (!GlobalStatus.effectPool.Keys.Contains(effectName))
            {
                // 최초
                GlobalStatus.effectPool[effectName] = new Queue<EffectController>();
            }
            Instantiate(GlobalDictionary.Prefab.Effect.data[effectName], transform).InitEffect(targetPos);
        }
    }
}
