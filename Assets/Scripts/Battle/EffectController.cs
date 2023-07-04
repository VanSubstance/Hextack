using UnityEngine;

namespace Assets.Scripts.Battle
{
    public class EffectController : MonoBehaviour
    {
        [SerializeField]
        private string effectName;
        private ParticleSystem particle;
        private ParticleSystem.MainModule particleModule;
        private void Awake()
        {
            particle = GetComponent<ParticleSystem>();
            particleModule = particle.main;
            gameObject.SetActive(false);
        }

        public EffectController InitEffect(Vector3 targetPos, Color color)
        {
            transform.position = targetPos;
            gameObject.SetActive(true);
            particleModule.startColor = color;
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
        }

        private void OnParticleSystemStopped()
        {
            Clear();
        }
    }
}
