using Assets.Scripts.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Common.MainManager
{
    public class MainLoadingManager : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI textLoading;
        [SerializeField]
        private GageController gage;
        private AsyncOperation sceneLoading;

        private void Awake()
        {
            gage.Init(1, 0, null);
            Debug.Log($"로딩 시작:: 목표 씬 => {GlobalStatus.NextScene}");
            sceneLoading = SceneManager.LoadSceneAsync($"{GlobalStatus.NextScene}");
        }

        private void Update()
        {
            if (sceneLoading.isDone)
            {
                GlobalStatus.CurScene = GlobalStatus.NextScene;
                GlobalStatus.NextScene = string.Empty;
                sceneLoading.allowSceneActivation = true;
                return;
            }
            gage.ApplyValue(sceneLoading.progress, true);
        }
    }
}
