using UnityEngine;

namespace Assets.Scripts.Battle
{
    public class EffectController : MonoBehaviour
    {
        [SerializeField]
        private string effectName;
        [SerializeField]
        private ParticleSystem[] children;
        private ParticleSystem particle;
        private ParticleSystem.MainModule particleModule;
        private void Awake()
        {
            particle = GetComponent<ParticleSystem>();
            particleModule = particle.main;
            gameObject.SetActive(false);
        }

        public EffectController InitEffect(Vector3 targetPos, Color color, string _effectName, float scale, float duration)
        {
            effectName = _effectName;
            transform.position = targetPos;
            transform.localScale = Vector3.one * scale;
            particleModule.startColor = color;
            ParticleSystem.MainModule t;
            foreach (ParticleSystem child in children)
            {
                t = child.main;
                t.startColor = color;
                if (duration <= 0)
                {
                    t.loop = true;
                    t.startLifetime = 1;
                } else
                {
                    t.loop = false;
                    t.startLifetime = duration;
                }
            }
            gameObject.SetActive(true);
            if (particle != null)
            {
                particle.Play();
            }
            return this;
        }

        /// <summary>
        /// 풀에 반납
        /// </summary>
        public void Clear()
        {
            GlobalStatus.effectPool[effectName].Enqueue(this);
            gameObject.SetActive(false);
            ParticleSystem.MainModule t;
            foreach (ParticleSystem child in children)
            {
                t = child.main;
                t.loop = false;
                t.startLifetime = 1;
            }
        }

        private void OnParticleSystemStopped()
        {
            Clear();
        }
    }
}
