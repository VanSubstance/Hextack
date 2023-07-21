using UnityEngine;

namespace Assets.Scripts.Server
{
    /// <summary>
    /// 여기저기서 사용할 소리들 연결
    /// </summary>
    [CreateAssetMenu(fileName ="Sound", menuName = "Scriptables/Data/Preset/Sound", order = int.MaxValue)]
    public class PresetSound: ScriptableObject
    {
        /// <summary>
        /// 타워 설치
        /// </summary>
        public AudioClip ClipForInstall;

        /// <summary>
        /// 몬스터 소환
        /// </summary>
        public AudioClip ClipForMonster;

        /// <summary>
        /// 버튼 클릭
        /// </summary>
        public AudioClip ClipForButtonClick;
    }
}
