using UnityEngine;

namespace Assets.Scripts.Audio
{
    public class AudioManager : AbsPoolingManager<AudioManager, AudioInfo>
    {
        public override int GetCountPoolForFirst()
        {
            return 100;
        }

        public override Transform GetParent()
        {
            return transform;
        }
    }
}
