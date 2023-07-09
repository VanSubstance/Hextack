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
            //controller = target as MonsterController;
            //Handles.color = Color.white;
            //Handles.DrawLine(controller.transform.position, controller.transform.position + Vector3.forward);
            //Handles.color = Color.red;
            //Handles.DrawLine(controller.transform.position, controller.ScreenPos);
        }
    }
}
