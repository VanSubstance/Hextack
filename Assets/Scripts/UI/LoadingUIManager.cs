using UnityEngine;
using TMPro;

namespace Assets.Scripts.UI
{
    public class LoadingUIManager : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI textLoading;
        [SerializeField]
        private GageController gage;

        private void Awake()
        {
            Debug.Log($"로딩 시작:: 목표 씬 => {GlobalStatus.NextScene}");
        }
    }
}
