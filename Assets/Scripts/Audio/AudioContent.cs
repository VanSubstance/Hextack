using UnityEngine;

namespace Assets.Scripts.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioContent : AbsPoolingContent<AudioInfo>
    {
        [SerializeField]
        private AudioSource src;

        public override void Clear()
        {
        }

        protected override bool InitExtra(AudioInfo _info)
        {
            src.clip = _info.Clip;
            transform.position = _info.Pos;
            ServerManager.Instance.ExecuteWithDelay(() =>
            {
                ReturnToPool();
            }, 1.5f);
            return true;
        }
    }
}
