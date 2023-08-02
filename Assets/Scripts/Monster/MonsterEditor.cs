using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Monster
{
    [CustomEditor(typeof(MonsterController))]
    [CanEditMultipleObjects]
    public class MonsterEditor : UnityEditor.Editor
    {
        private MonsterController controller;
        private void OnSceneGUI()
        {
            // 현재 + 앞으로 목적지 표기
            controller = target as MonsterController;
            Handles.color = Color.white;
            try
            {
                Handles.DrawLine(controller.transform.position, controller.CurrentDestPos);
            }
            catch (System.NullReferenceException)
            {

            }
        }
    }
}
