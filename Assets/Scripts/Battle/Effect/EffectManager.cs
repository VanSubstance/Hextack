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
        public EffectController ExecutNewEffect(string effectName, Vector3 targetPos, Color? color, float scale = 1, float duration = 0, Vector3? stareDirectionInCase = null)
        {
            EffectController res = null;
            try
            {
                if (!GlobalStatus.effectPool.Keys.Contains(effectName))
                {
                    // 최초
                    GlobalStatus.effectPool[effectName] = new Queue<EffectController>();
                }
                if (!GlobalStatus.effectPool[effectName].TryDequeue(out res))
                {
                    res = Instantiate(GlobalDictionary.Prefab.Effect.data[effectName], transform);
                }
                res.InitEffect(targetPos, color != null ? (Color)color : Color.white, effectName, scale, duration);
                if (stareDirectionInCase != null)
                {
                    res.transform.LookAt((Vector3)stareDirectionInCase);
                }
            } catch (KeyNotFoundException)
            {
                Debug.Log($"이펙트 정의되어있지 않음 ::: {effectName}");
            }
            return res;
        }
    }
}
