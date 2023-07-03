using UnityEngine;

namespace Assets.Scripts.Battle
{
    public class EffectController : MonoBehaviour
    {
        [SerializeField]
        private string effectName;
        private ParticleSystem particle;
        private void Awake()
        {
            particle = GetComponent<ParticleSystem>();
            gameObject.SetActive(false);
        }

        public EffectController InitEffect(Vector3 targetPos)
        {
            transform.position = targetPos;
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
        }

        private void OnParticleSystemStopped()
        {
            Clear();
        }
    }
}
